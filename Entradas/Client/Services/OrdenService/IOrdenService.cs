namespace Entradas.Client.Services.OrdenService
{
    public interface IOrdenService
    {
        event Action? OnChange;
        List<Orden> Ordenes { get; set; }
        public string Mensaje { get; set; }
        public int PaginaActual { get; set; }
        public int PaginasTotales { get; set; }
        Task AgregarItemLocal(OrdenDetalleRegistroDto orderRegistroDto);
        Task<List<OrdenDetalleRegistroDto>> ObtenerOrdenDetalleLocal();
        Task RemoverItemLocal(int eventoId, int eventoEntradaId, int eventoFechaId);
        Task LimpiarItemLocal();
        Task<ServiceResponse<int>> CreateOrden(OrdenRegistroDto dto);
        Task GetOrdenesPaginado(int pagina);
        Task<ServiceResponse<Orden>> GetOrdenPorId(int ordenId);
        Task<ServiceResponse<int>> UpdateOrden(OrdenActualizarDto dto);
        Task GetOrdenesPorUsuario(int pagina, int usuarioId);
        Task<ServiceResponse<List<VwOrden>>> GetOrdenPorOrdenIdPorUsuarioId(int ordenId, int usuarioId);
        Task<ServiceResponse<List<VwOrdenTicket>>> GetOrdenTicketPorOrdenId(int ordenId);
        Task<ServiceResponse<VwOrdenTicket>> GetOrdenTicketPorOrdenTicketId(int ordenTicketId);
        Task<ServiceResponse<int>> GenerarTickets(int ordenId);
        Task<ServiceResponse<int>> TicketNominar(OrdenTicketActualizarDto dto);
    }
}
