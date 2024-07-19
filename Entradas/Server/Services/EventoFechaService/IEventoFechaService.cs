using Entradas.Shared.DTO.EventoEntradaDto;
using Entradas.Shared.DTO.EventoFechaDto;
using Entradas.Shared.Models;

namespace Entradas.Server.Services.EventoFechaService
{
    public interface IEventoFechaService
    {
        Task<ServiceResponse<int>> CreateEventoFechas(EventoFecha eventoFecha);
        Task<ServiceResponse<int>> UpdateEventoFechas(EventoFecha eventoFecha);
        Task<ServiceResponse<int>> DeleteEventoFecha(int eventoFechaId);
        Task<ServiceResponse<List<EventoFecha>>> GetEventoFechas();
        Task<ServiceResponse<EventoFechaPaginadoDto>> GetEventoFechasPorEvento(int pagina, int eventoId);
        Task<ServiceResponse<EventoFecha>> GetEventoFechaPorId(int eventoFechaId);
    }
}
