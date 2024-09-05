namespace Entradas.Client.Services.EventoService
{
    public class EventoService : IEventoService
    {
        private readonly HttpClient _http;
        
        public EventoService(HttpClient http)
        {
            _http = http;
            
        }
        public List<Evento> Eventos { get; set; } = new List<Evento>();
        public List<Evento> EventosBusqueda { get; set; } = new List<Evento>();
        public string Mensaje { get; set; } = string.Empty;
        public int PaginaActual { get; set; } = 1;
        public int PaginasTotales { get; set; } = 0;

        public event Action? OnChange;

        public async Task BuscarEventoPaginado(int pagina, string? nombre, string? informacion, string? ubicacion)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<EventoPaginadoDto>>($"api/evento/BuscarEventoPaginado?pagina={pagina}&nombre={nombre}&informacion={informacion}&ubicacion={ubicacion}");

            if (response != null && response.Data != null)
            {
                Eventos = response.Data.Eventos;
                PaginaActual = response.Data.PaginaActual;
                PaginasTotales = response.Data.Paginas;
            }

            if (Eventos.Count == 0)
            {
                Mensaje = response!.Message;
            }

            OnChange?.Invoke();
        }

        public async Task GetEventosPaginado(int pagina)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<EventoPaginadoDto>>($"api/evento/GetEventosPaginado?pagina={pagina}");

            if (response != null && response.Data != null)
            {
                Eventos = response.Data.Eventos;
                PaginaActual = response.Data.PaginaActual;
                PaginasTotales = response.Data.Paginas;
            }

            if (Eventos.Count == 0)
            {
                Mensaje = response!.Message;
            }

            OnChange?.Invoke();
        }

        public async Task<ServiceResponse<int>> CreateEvento(EventoRegistroDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/Evento/CreateEvento", dto);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return result;
        }

        public async Task<ServiceResponse<int>> DeleteEvento(int eventoId)
        {
            var result = await _http.PutAsJsonAsync($"api/Evento/DeleteEvento?eventoId={eventoId}", eventoId);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return response!;
        }

        public async Task<ServiceResponse<Evento>> GetEventoPorId(int id)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<Evento>>($"api/Evento/GetEventoPorId?eventoId={id}");
            return response!;
        }

        public async Task GetEventos()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Evento>>>($"api/Evento/GetEventos");

            if (response != null && response.Data != null)
            {
                Eventos = response.Data;
            }

            if (Eventos.Count == 0)
            {
                Mensaje = "No se encontraorn eventos registrados";
            }
        }

      

        public async Task<ServiceResponse<int>> UpdateEvento(EventoRegistroDto dto)
        {
            var result = await _http.PutAsJsonAsync("api/Evento/UpdateEvento", dto);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return response!;
        }

        public async Task BuscarEvento(string? nombre, string? informacion, string? ubicacion)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Evento>>>($"api/evento/BuscarEvento?nombre={nombre}&informacion={informacion}&ubicacion={ubicacion}");

            if (response != null && response.Data != null)
            {
                EventosBusqueda = response.Data;
            }

            if (EventosBusqueda.Count == 0)
            {
                Mensaje = response!.Message;
            }

            OnChange?.Invoke();
        }
    }
}
