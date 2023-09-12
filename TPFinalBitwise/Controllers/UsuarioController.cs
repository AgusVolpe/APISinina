using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.DTO;
using TPFinalBitwise.Models;

namespace TPFinalBitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IGenericRepository<Usuario> _repository;
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepository;
        protected RespuestaAPI _respuestaAPI;

        public UsuarioController(IGenericRepository<Usuario> repository, IMapper mapper, IUsuarioRepository usuarioRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            this._respuestaAPI = new RespuestaAPI();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDatosDTO>>> ObtenerTodos()
        {
            var usuarios = await _repository.ObtenerTodos();
            var usuariosDTO = _mapper.Map<IEnumerable<UsuarioDatosDTO>>(usuarios);
            return Ok(usuariosDTO);
        }

        [HttpGet("{id}", Name = "GetUsuario")]
        public async Task<ActionResult<IEnumerable<UsuarioDatosDTO>>> Obtener(string id)
        {
            var usuario = await _usuarioRepository.ObtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return NotFound(); 
            }
            var usuarioDTO = _mapper.Map<UsuarioDatosDTO>(usuario);
            return Ok(usuarioDTO);
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Insertar([FromBody] UsuarioRegistroDTO usuarioRegistroDTO)
        {
            var nombreValidado = await _usuarioRepository.EsUsuarioUnico(usuarioRegistroDTO.UserName);
            if (!nombreValidado)
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.EsExitoso = false;
                _respuestaAPI.MensajesError.Add("Ya existe un usuario en la Base de Datos con el nombre indicado");
                return BadRequest(_respuestaAPI);
            }

            var usuario = await _usuarioRepository.Registro(usuarioRegistroDTO);
            if (usuario == null)
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.EsExitoso = false;
                _respuestaAPI.MensajesError.Add("Error: no se pudo registrar el usuario");
                return BadRequest(_respuestaAPI);
            }
            _respuestaAPI.StatusCode = HttpStatusCode.OK;
            _respuestaAPI.EsExitoso = true;
            return Ok(_respuestaAPI);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO usuarioLoginDTO)
        {
            var respuestaLogin = await _usuarioRepository.Login(usuarioLoginDTO);
            if (respuestaLogin.Usuario == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.EsExitoso = false;
                _respuestaAPI.MensajesError.Add("Nombre de usuario o contraseña incorrectos");
                return BadRequest(_respuestaAPI);
            }
            _respuestaAPI.StatusCode = HttpStatusCode.OK;
            _respuestaAPI.EsExitoso = true;
            _respuestaAPI.Resultado = respuestaLogin;
            return Ok(_respuestaAPI);

        }
    }
}
