using Microsoft.AspNetCore.Identity;

namespace TPFinalBitwise.Models
{
    public class Usuario : IdentityUser
    {
        public string? Nombre { get; set; }
    }
}
