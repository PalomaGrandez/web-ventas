using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entradas.Shared.DTO
{
    public class UsuarioRegistroDto
    {
        public int UsuarioId { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public string Nombres { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 3)]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 3)]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 3)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage ="El campo de contraseña es requerido."), StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
