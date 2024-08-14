namespace Entradas.Client.Services.OrdenService
{
    public class OrdenService : IOrdenService
    {
        private readonly HttpClient _http;
        private readonly ISessionStorageService _sessionStorage;
        public OrdenService(HttpClient http, ISessionStorageService sessionStorage)
        {
            _http = http;
            _sessionStorage = sessionStorage;
        }
        public List<Orden> Ordenes { get; set; } = new List<Orden>();
        public string Mensaje { get; set; } = string.Empty;
        public int PaginaActual { get; set; } = 1;
        public int PaginasTotales { get; set; } = 0;

        public event Action? OnChange;
        public async Task AgregarItemLocal(OrdenDetalleRegistroDto ordenRegistroDto)
        {
            List<OrdenDetalleRegistroDto> orden = await _sessionStorage.GetItemAsync<List<OrdenDetalleRegistroDto>>("orden") ?? new List<OrdenDetalleRegistroDto>();

            var itemRepetido = orden.FirstOrDefault(x => x.EventoId == ordenRegistroDto.EventoId
                                                         && x.EventoEntradaId == ordenRegistroDto.EventoEntradaId
                                                         && x.EventoFechaId == ordenRegistroDto.EventoFechaId);

            if (itemRepetido == null)
            {
                orden.Add(ordenRegistroDto);
            }
            else
            {
                // Mantén la cantidad que ya se había actualizado antes, sin sobrescribirla
                itemRepetido.Cantidad += ordenRegistroDto.Cantidad;

                // Solo actualiza las propiedades que no han sido cambiadas manualmente
                itemRepetido.Fecha = ordenRegistroDto.Fecha;
                itemRepetido.EntradaTipo = ordenRegistroDto.EntradaTipo;
                itemRepetido.PrecioRegular = ordenRegistroDto.PrecioRegular;
                itemRepetido.PrecioTotal = itemRepetido.Cantidad * itemRepetido.PrecioRegular;
            }

            await _sessionStorage.SetItemAsync("orden", orden);
        }

        public async Task<ServiceResponse<int>> CreateOrden(OrdenRegistroDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/Orden/CreateOrden", dto);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return result!;
        }
        public async Task LimpiarItemLocal()
        {
            List<OrdenDetalleRegistroDto> orden = new();
            await _sessionStorage.SetItemAsync("orden", orden);
        }
        public async Task<List<OrdenDetalleRegistroDto>> ObtenerOrdenDetalleLocal()
        {
            var orden = await _sessionStorage.GetItemAsync<List<OrdenDetalleRegistroDto>>("orden");

            if (orden == null)
            {
                orden = new();
                return orden;
            }
            return orden;
        }
        public async Task RemoverItemLocal(int eventoId, int eventoEntradaId)
        {
            var orden = await _sessionStorage.GetItemAsync<List<OrdenDetalleRegistroDto>>("orden");

            if (orden == null)
            {
                return;
            }

            var ordenItem = orden.Find(x => x.EventoId == eventoId &&
                                            x.EventoEntradaId == eventoEntradaId);
            if (ordenItem != null)
            {
                orden.Remove(ordenItem);
                await _sessionStorage.SetItemAsync("orden", orden);
            }
        }
        public async Task GetOrdenesPaginado(int pagina)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<OrdenPaginadoDto>>($"api/orden/GetOrdenesPaginado?pagina={pagina}");

            if (response != null && response.Data != null)
            {
                Ordenes = response.Data.Ordenes;
                PaginaActual = response.Data.PaginaActual;
                PaginasTotales = response.Data.Paginas;
            }

            if (Ordenes.Count == 0)
            {
                Mensaje = response!.Message;
            }
            OnChange?.Invoke();
        }
        public async Task<ServiceResponse<Orden>> GetOrdenPorId(int ordenId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<Orden>>($"api/Orden/GetOrdenPorId?ordenId={ordenId}");

            return response!;
        }
        public async Task<ServiceResponse<int>> UpdateOrden(OrdenActualizarDto dto)
        {
            var result = await _http.PutAsJsonAsync("api/Orden/UpdateOrden", dto);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return response!;
        }
        public async Task GetOrdenesPorUsuario(int pagina, int usuarioId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<OrdenPaginadoDto>>($"api/Orden/GetOrdenPorUsuario?pagina={pagina}&usuarioId={usuarioId}");

            if (response?.Data != null)
            {
                Ordenes = response.Data.Ordenes;
                PaginaActual = response.Data.PaginaActual;
                PaginasTotales = response.Data.Paginas;
            }
            else
            {
                Mensaje = response?.Message ?? "No se pudo obtener la respuesta del servidor.";
            }

            OnChange?.Invoke();
        }

        public async Task<ServiceResponse<List<VwOrden>>> GetOrdenPorOrdenIdPorUsuarioId(int ordenId, int usuarioId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<VwOrden>>>($"api/Orden/GetOrdenPorOrdenIdPorUsuarioId?ordenId={ordenId}&usuarioId={usuarioId}");

            return response ?? new ServiceResponse<List<VwOrden>>();
        }


        public async Task<ServiceResponse<int>> GenerarTickets(int ordenId)
        {
            var response = await _http.PostAsJsonAsync($"api/Orden/GenerarTickets?ordenId={ordenId}", ordenId);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return result!;
        }

        public async Task<ServiceResponse<List<VwOrdenTicket>>> GetOrdenTicketPorOrdenId(int ordenId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<VwOrdenTicket>>>($"api/Orden/GetOrdenTicketPorOrdenId?ordenId={ordenId}");

            return response!;
        }

        public async Task<ServiceResponse<VwOrdenTicket>> GetOrdenTicketPorOrdenTicketId(int ordenTicketId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<VwOrdenTicket>>($"api/Orden/GetOrdenTicketPorOrdenTicketId?ordenTicketId={ordenTicketId}");

            return response!;
        }

        public async Task<ServiceResponse<int>> TicketNominar(OrdenTicketActualizarDto dto)
        {
            var result = await _http.PutAsJsonAsync("api/Orden/TicketNominar", dto);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return response!;
        }
    }
}
