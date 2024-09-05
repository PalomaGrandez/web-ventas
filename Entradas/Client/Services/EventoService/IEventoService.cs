namespace Entradas.Client.Services.EventoService
{
    public interface IEventoService
    {
        event Action? OnChange;
        public List<Evento> Eventos { get; set; }
        public List<Evento> EventosBusqueda { get; set; }
        public string Mensaje { get; set; }
        public int PaginaActual { get; set; }
        public int PaginasTotales { get; set; }
        Task<ServiceResponse<int>> CreateEvento(EventoRegistroDto dto);
        Task<ServiceResponse<int>> UpdateEvento(EventoRegistroDto dto);
        Task<ServiceResponse<int>> DeleteEvento(int eventoId);
        Task GetEventos();
        Task GetEventosPaginado(int pagina);
        Task BuscarEventoPaginado(int pagina, string? nombre, string? informacion, string? ubicacion);
        Task BuscarEvento(string? nombre, string? informacion, string? ubicacion);
        Task<ServiceResponse<Evento>> GetEventoPorId(int id);
    }
}

