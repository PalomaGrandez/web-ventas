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

        [Required, StringLength(30, MinimumLength = 3)]
        public string NombreUsuario { get; set; } = string.Empty;



        [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El campo debe contener solo letras.")]
        public string TipoDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El número de documento debe tener entre 8 y 20 caracteres.")]
        public string NumeroDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage ="El campo de contraseña es requerido."), StringLength(30, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
