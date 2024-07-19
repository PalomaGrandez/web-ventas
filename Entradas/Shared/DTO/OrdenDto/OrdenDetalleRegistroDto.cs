using System.ComponentModel.DataAnnotations;

namespace Entradas.Shared.DTO.OrdenDto
{
    public class OrdenDetalleRegistroDto
    {
        [Required (ErrorMessage ="Debe seleccionar un evento")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar un evento.")]
        public int? EventoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una entrada para el evento.")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una entrada para el evento.")]
        public int? EventoEntradaId { get; set; }

        public string EntradaTipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una fecha para el evento.")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una fecha para el evento.")]
        public int? EventoFechaId { get; set; }

        public DateTime Fecha { get; set; }
        public decimal PrecioRegular { get; set; }

        [Required(ErrorMessage = "La cantidad de entradas debe ser mayor a 0.")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "La cantidad de entradas debe ser mayor a 0.")]
        public int Cantidad { get; set; }
        public decimal PrecioTotal { get; set; }
    }
}
