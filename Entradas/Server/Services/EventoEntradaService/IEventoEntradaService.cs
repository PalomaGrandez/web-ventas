using Entradas.Shared.DTO.EventoEntradaDto;
using Entradas.Shared.Models;

namespace Entradas.Server.Services.EventoEntradaService
{
    public interface IEventoEntradaService
    {
        Task<ServiceResponse<int>> CreateEventoEntrada(EventoEntrada eventoEntrada);
        Task<ServiceResponse<int>> UpdateEventoEntrada(EventoEntrada eventoEntrada);
        Task<ServiceResponse<int>> DeleteEventoEntrada(int eventoEntradaId);
        Task<ServiceResponse<EventoEntradaPaginadoDto>> GetEventoEntradasPaginado(int pagina);
        Task<ServiceResponse<EventoEntradaPaginadoDto>> GetEventoEntradasPorEvento(int pagina, int eventoId);
        Task<ServiceResponse<EventoEntrada>> GetEventoEntradaPorId(int eventoEntradaId);

    }
}
