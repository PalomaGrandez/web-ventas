namespace Entradas.Client.Services.EventoEntradaService
{
    public interface IEventoEntradaService
    {
        event Action? OnChange;
        public List<EventoEntrada> EventoEntradas { get; set; }
        public string Mensaje { get; set; }
        public int PaginaActual { get; set; }
        public int PaginasTotales { get; set; }
        Task<ServiceResponse<int>> CreateEventoEntrada(EventoEntrada eventoEntrada);
        Task<ServiceResponse<int>> UpdateEventoEntrada(EventoEntrada eventoEntrada);
        Task<ServiceResponse<int>> DeleteEvento(int eventoEntradaId);
        Task GetEventoEntradasPaginado(int pagina);
        Task GetEventoEntradasPorEvento(int pagina,int eventoId);
        Task<ServiceResponse<EventoEntrada>> GetEventoEntradaPorId(int eventoEntradaId);
    }
}
