using System.ComponentModel.DataAnnotations;

namespace Entradas.Shared.DTO.CategoriaDto
{
    public class CategoriaRegistroDto
    {
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
