namespace Entradas.Client.Services.EventoFechaService
{
    public interface IEventoFechaService
    {
        event Action? OnChange;
        public List<EventoFecha> EventoFechas { get; set; }
        public string Mensaje { get; set; }
        public int PaginaActual { get; set; }
        public int PaginasTotales { get; set; }
        Task<ServiceResponse<int>> CreateEventoFecha(EventoFecha eventoFecha);
        Task<ServiceResponse<int>> UpdateEventoFecha(EventoFecha eventoFecha);
        Task<ServiceResponse<int>> DeleteEventoFecha(int eventoFechaId);
        Task GetEventoFechasPorEvento(int pagina, int eventoId);
        Task<ServiceResponse<EventoFecha>> GetEventoFechaPorId(int eventoFechaId);
    }
}
