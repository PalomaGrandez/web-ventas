
using Entradas.Shared.DTO.EventoEntradaDto;
using Entradas.Shared.DTO.EventoFechaDto;
using Entradas.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Entradas.Client.Services.EventoFechaService
{
    public class EventoFechaService : IEventoFechaService
    {
        private readonly HttpClient _http;
        
        public EventoFechaService(HttpClient http)
        {
            _http = http;        
        }
        public List<EventoFecha> EventoFechas { get; set; } = new();
        public string Mensaje { get; set; } = string.Empty;
        public int PaginaActual { get; set; } = 1;
        public int PaginasTotales { get; set; } = 0;

        public event Action? OnChange;

        public async Task<ServiceResponse<int>> CreateEventoFecha(EventoFecha eventoFecha)
        {
            var response = await _http.PostAsJsonAsync("api/EventoFecha/CreateEventoFecha", eventoFecha);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return result!;
        }
        public async Task GetEventoFechasPorEvento(int pagina, int eventoId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<EventoFechaPaginadoDto>>($"api/EventoFecha/GetEventoFechasPorEvento?pagina={pagina}&eventoId={eventoId}");

            if (response != null && response.Data != null)
            {
                EventoFechas = response.Data.EventoFechas;

                PaginaActual = response.Data.PaginaActual;
                PaginasTotales = response.Data.Paginas;
            }

            if (EventoFechas.Count == 0)
            {
                Mensaje = response!.Message;
            }
            OnChange?.Invoke();
        }

        public async Task<ServiceResponse<EventoFecha>> GetEventoFechaPorId(int eventoFechaId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<EventoFecha>>($"api/EventoFecha/GetEventoFechaPorId?eventoFechaId={eventoFechaId}");
            return response!;
        }

        public async Task<ServiceResponse<int>> UpdateEventoFecha(EventoFecha eventoFecha)
        {
            var result = await _http.PutAsJsonAsync("api/EventoFecha/UpdateEventoFecha", eventoFecha);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return response!;            
        }

        public async Task<ServiceResponse<int>> DeleteEventoFecha(int eventoFechaId)
        {
            var result = await _http.PutAsJsonAsync($"api/EventoFecha/DeleteEventoFecha?eventoFechaId={eventoFechaId}", eventoFechaId);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return response!;
        }
    }
}
