using Entradas.Shared.Models;

namespace Entradas.Server.Services.EventoService
{
    public interface IEventoService
    {
        Task<ServiceResponse<int>> CreateEvento(EventoRegistroDto dto);
        Task<ServiceResponse<int>> UpdateEvento(EventoRegistroDto dto);
        Task<ServiceResponse<int>> DeleteEvento(int eventoId);
        Task<ServiceResponse<List<Evento>>> GetEventos();
        Task<ServiceResponse<EventoPaginadoDto>> GetEventosPaginado(int pagina);

        Task<ServiceResponse<int>> ReducirCapacidadEvento(List<EventoEntradaCompraDto> entradasCompradas);

        Task<ServiceResponse<EventoEntrada>> ObtenerEventoEntradaDisponible(int eventoEntradaId);

        Task<ServiceResponse<EventoPaginadoDto>> BuscarEventoPaginado(int pagina, string? nombre, string? informacion, string? ubicacion);
        Task<ServiceResponse<List<Evento>>> BuscarEvento(string? nombre, string? informacion, string? ubicacion);
        Task<ServiceResponse<Evento>> GetEventoPorId(int eventoId);


    }
}
