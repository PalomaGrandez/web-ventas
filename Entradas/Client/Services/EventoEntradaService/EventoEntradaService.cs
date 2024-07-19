
using Entradas.Shared.DTO.EventoEntradaDto;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http.Json;

namespace Entradas.Client.Services.EventoEntradaService
{
    public class EventoEntradaService : IEventoEntradaService
    {
        private readonly HttpClient _http;        

        public EventoEntradaService(HttpClient http)
        {
            _http = http;            
        }
        public List<EventoEntrada> EventoEntradas { get; set; } = new();
        public string Mensaje { get; set; } = string.Empty;
        public int PaginaActual { get; set; } = 1;
        public int PaginasTotales { get; set; } = 0;

        public event Action? OnChange;

        public async Task<ServiceResponse<int>> CreateEventoEntrada(EventoEntrada eventoEntrada)
        {
            var response = await _http.PostAsJsonAsync("api/EventoEntrada/CreateEventoEntrada", eventoEntrada);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return result;
        }

        public async Task<ServiceResponse<int>> DeleteEvento(int eventoEntradaId)
        {
            var result = await _http.PutAsJsonAsync($"api/EventoEntrada/DeleteEventoEntrada?eventoEntradaId={eventoEntradaId}", eventoEntradaId);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return response!;
        }

        public async Task<ServiceResponse<EventoEntrada>> GetEventoEntradaPorId(int eventoEntradaId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<EventoEntrada>>($"api/EventoEntrada/GetEventoEntradaPorId?eventoEntradaId={eventoEntradaId}");
            return response;
        }

        public async Task GetEventoEntradasPaginado(int pagina)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<EventoEntradaPaginadoDto>>($"api/EventoEntrada/GetEventoEntradasPaginado?pagina={pagina}");

            if (response != null && response.Data != null)
            {
                EventoEntradas = response.Data.EventoEntradas;
                PaginaActual = response.Data.PaginaActual;
                PaginasTotales = response.Data.Paginas;
            }

            if (EventoEntradas.Count == 0)
            {
                Mensaje = response!.Message;
            }
            OnChange?.Invoke();
        }

        public async Task GetEventoEntradasPorEvento(int pagina, int eventoId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<EventoEntradaPaginadoDto>>($"api/EventoEntrada/GetEventoEntradasPorEvento?pagina={pagina}&eventoId={eventoId}");

            if (response != null && response.Data != null)
            {
                EventoEntradas = response.Data.EventoEntradas;

                PaginaActual = response.Data.PaginaActual;
                PaginasTotales = response.Data.Paginas;
            }

            if (EventoEntradas.Count == 0)
            {
                Mensaje = response!.Message;
            }
            OnChange?.Invoke();
        }

        public async Task<ServiceResponse<int>> UpdateEventoEntrada(EventoEntrada eventoEntrada)
        {
            var result = await _http.PutAsJsonAsync("api/EventoEntrada/UpdateEventoEntrada", eventoEntrada);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return response!;
        }
    }
}
