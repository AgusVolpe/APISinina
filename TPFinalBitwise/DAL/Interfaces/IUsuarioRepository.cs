using TPFinalBitwise.DTO;
using TPFinalBitwise.Models;

namespace TPFinalBitwise.DAL.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        public Task<bool> EsUsuarioUnico(string usuario);
        public Task<UsuarioDatosDTO> Registro(UsuarioRegistroDTO usuarioRegistroDTO);
        public Task<UsuarioRespuestaLoginDTO> Login(UsuarioLoginDTO usuarioLoginDTO);
        public Task<Usuario> ObtenerUsuarioPorId(string usuarioId);
    }
}
