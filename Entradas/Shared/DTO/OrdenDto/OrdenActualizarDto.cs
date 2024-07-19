namespace Entradas.Shared.DTO.OrdenDto
{
    public class OrdenActualizarDto
    {
        public int OrdenId { get; set; }
        public DateTime? FechaOrden { get; set; }        
        public decimal? PrecioTotal { get; set; }
        public string Estado { get; set; }=string.Empty;
        public string MedioPago { get; set; } = string.Empty;
        public string NumeroOperacion { get; set; } = string.Empty;
    }
}
