using TPFinalBitwise.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using TPFinalBitwise.DAL.Implementaciones;
using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.Utilidades;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient(typeof(IProductoRepository), typeof(ProductoRepository));
builder.Services.AddTransient(typeof(ICategoriaRepository), typeof(CategoriaRepository));
builder.Services.AddTransient(typeof(IItemRepository), typeof(ItemRepository));
builder.Services.AddAutoMapper(typeof(AutomapperProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
