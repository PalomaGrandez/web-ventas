

namespace Entradas.Server.Services.OrdenService
{
    public interface IOrdenService
    {
        Task<ServiceResponse<int>> CreateOrden(OrdenRegistroDto dto);
        Task<ServiceResponse<OrdenPaginadoDto>> GetOrdenesPaginado(int pagina);
        Task<ServiceResponse<Orden>> GetOrdenPorId(int ordenId);
        Task<ServiceResponse<int>> UpdateOrden(OrdenActualizarDto dto);
        Task<ServiceResponse<OrdenPaginadoDto>> GetOrdenesPorUsuario(int pagina, int usuarioId);
        Task<ServiceResponse<List<VwOrden>>> GetOrdenPorOrdenIdPorUsuarioId(int ordenId, int usuarioId);
        Task<ServiceResponse<List<VwOrdenTicket>>> GetOrdenTicketPorOrdenId(int ordenId);
        Task<ServiceResponse<VwOrdenTicket>> GetOrdenTicketPorOrdenTicketId(int ordenTicketId);
        Task<List<OrdenDetalleRegistroDto>> GetOrdenDetallePorOrdenId(int ordenId);
        Task<ServiceResponse<int>> GenerarTickets(int ordenId);
        //Task<ServiceResponse<int>> GenerarTicketPdf(int ordenId);
        Task<ServiceResponse<int>> TicketNominar(OrdenTicketActualizarDto dto);
    }
}
