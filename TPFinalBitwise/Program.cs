using TPFinalBitwise.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using TPFinalBitwise.DAL.Implementaciones;
using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using TPFinalBitwise.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt => opt.CacheProfiles.Add("CachePorDefecto", new CacheProfile() { Duration = 60 }));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticacion JWT usando el esquema Bearer. \r\n\r\n " +
        "Ingresa el texto 'Bearer[espacio]token' \r\n\r\n" +
        "Ejemplo: \"Bearer ashiausdbjaksbn546f4asdffqa5ef8\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient(typeof(IProductoRepository), typeof(ProductoRepository));
builder.Services.AddTransient(typeof(ICategoriaRepository), typeof(CategoriaRepository));
builder.Services.AddTransient(typeof(IItemRepository), typeof(ItemRepository));
builder.Services.AddTransient(typeof(IVentaRepository), typeof(VentaRepository));
builder.Services.AddTransient(typeof(IUsuarioRepository), typeof(UsuarioRepository));
builder.Services.AddAutoMapper(typeof(AutomapperProfile));

//Soporte para autenticacion con .Net Identity
builder.Services.AddIdentity<Usuario, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

var clave = builder.Configuration.GetValue<string>("Settings:PasswordSecreta");
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(clave)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });




builder.Services.AddResponseCaching();

builder.Services.AddCors(p => p.AddPolicy("PolicyCors", policy =>
{
    policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
}));

/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PolicyCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

/*app.MapControllerRoute(
   name: "default", 
   pattern: "{controller}/{action=Index}/{id?}");
*/

app.Run();
