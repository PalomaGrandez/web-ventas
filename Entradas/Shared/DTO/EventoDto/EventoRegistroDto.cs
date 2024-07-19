using System.ComponentModel.DataAnnotations;

namespace Entradas.Shared.DTO.EventoDto
{
    public class EventoRegistroDto
    {
        public int EventoId { get; set; }

        [Required(ErrorMessage = "El campo de nombre es requerido.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo de información es requerido.")]
        public string Informacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo de ubicación es requerido.")]
        public string Ubicacion { get; set; } = string.Empty;
        public string Imagen { get; set; } = string.Empty;

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una Categoría")]
        public int? CategoriaId { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Debe ingresar la capacidad total del evento")]
        public decimal CapacidadTotal { get; set; }

    }
}
