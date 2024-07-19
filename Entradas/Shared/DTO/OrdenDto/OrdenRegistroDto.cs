namespace Entradas.Shared.DTO.OrdenDto
{
    public class OrdenRegistroDto
    {
        public int OrdenId { get; set; }
        public int? UsuarioId { get; set; }
        public DateTime? FechaOrden { get; set; }
        public decimal? PrecioTotal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string MedioPago { get; set; } = string.Empty;
        public List<OrdenDetalleRegistroDto> Items { get; set; } = new();
    }
}
