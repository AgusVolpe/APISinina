using AutoMapper;
using TPFinalBitwise.DAL.DataContext;
using TPFinalBitwise.DTO;
using TPFinalBitwise.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TPFinalBitwise.DAL.Interfaces;

namespace TPFinalBitwise.DAL.Implementaciones
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        private string claveSecreta;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;


        public UsuarioRepository(ApplicationDbContext context, IConfiguration config, UserManager<Usuario> userManager,
                                    RoleManager<IdentityRole> roleManager, IMapper mapper) : base(context)
        {
            _context = context;
            claveSecreta = config.GetValue<string>("Settigns:PasswordSecreta");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<bool> EsUsuarioUnico(string usuario)
        {
            var validacion = await _context.Usuarios.FirstOrDefaultAsync(u => u.UserName == usuario);
            if (validacion == null)
            {
                return true;
            }
            return false;
        }

        public async Task<UsuarioRespuestaLoginDTO> Login(UsuarioLoginDTO usuarioLoginDTO)
        {
            var usuarioEncontrado = await _context.Usuarios.FirstOrDefaultAsync(
                                            u => u.UserName.ToLower() == usuarioLoginDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(usuarioEncontrado, usuarioLoginDTO.Password);

            if(usuarioEncontrado == null || isValid == false)
            {
                return new UsuarioRespuestaLoginDTO
                {
                    Usuario = null,
                    Token = ""
                };
            }

            var roles = await _userManager.GetRolesAsync(usuarioEncontrado);

            var manejadorToken = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(claveSecreta);
            var key = Encoding.ASCII.GetBytes("PasswordSecretaParaElTrabajoFinalDelCursoDeBitwise");
            
            var tokenInformacion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuarioEncontrado.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenInformacion);
            var usuarioRespuestaLoginDTO = new UsuarioRespuestaLoginDTO()
            {
                Usuario = _mapper.Map<UsuarioDatosDTO>(usuarioEncontrado),
                Token = manejadorToken.WriteToken(token)
            };
            return usuarioRespuestaLoginDTO;
        }

        public async Task<UsuarioDatosDTO> Registro(UsuarioRegistroDTO usuarioRegistroDTO)
        {
            
            var usuarioNuevo = new Usuario()
            {
                UserName = usuarioRegistroDTO.UserName,
                Nombre = usuarioRegistroDTO.Nombre,
                Email = usuarioRegistroDTO.UserName,
                NormalizedEmail = usuarioRegistroDTO.UserName.ToUpper()
            };

            var resultado = await _userManager.CreateAsync(usuarioNuevo, usuarioRegistroDTO.Password); 
            if(resultado.Succeeded)
            {
                //Este paso se hace solo la primera vez y es para crear los roles
                if(!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("Visitante"));
                    await _roleManager.CreateAsync(new IdentityRole("Registrado"));
                }

                var rol = usuarioRegistroDTO.Role;
                await _userManager.AddToRoleAsync(usuarioNuevo, rol);
                var usuarioRetornado = await _context.Usuarios.FirstOrDefaultAsync(u => u.UserName == usuarioRegistroDTO.UserName);

                return _mapper.Map<UsuarioDatosDTO>(usuarioRetornado);
            }
            return null; 
        }

        public async Task<Usuario> ObtenerUsuarioPorId(string usuarioId)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
        }
    }
}
