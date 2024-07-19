using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entradas.Shared.DTO.UsuarioDto
{
    public class UsuarioLoginDto
    {
        [Required (ErrorMessage ="El campo email es un campo requerido.")]
        [EmailAddress (ErrorMessage ="El campo email no es un formato de email valido.")]
        public string Email { get; set; } = string.Empty;

        [Required (ErrorMessage ="Debe ingresar una contraseña.")]
        public string Password { get; set; } = string.Empty;
    }
}
