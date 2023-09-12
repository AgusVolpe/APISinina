using System.ComponentModel.DataAnnotations;

namespace TPFinalBitwise.DTO
{
    public class UsuarioLoginDTO
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string? Password { get; set; }
    }
}
