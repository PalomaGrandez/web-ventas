using Entradas.Shared.DTO.BannerDto;
using Entradas.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Entradas.Client.Services.BannerService
{
    public class BannerService : IBannerService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public BannerService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }
        public List<Banner> Banners { get; set; } = new();
        public string Mensaje { get; set; }
        public int PaginaActual { get; set; }
        public int PaginasTotales { get; set; }

        public event Action? OnChange;

        public async Task CreateBanner(Banner banner)
        {
            var response = await _http.PostAsJsonAsync("api/Banner/CreateBanner", banner);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Banner>>();

            Console.WriteLine($"Crear Banner: {result.Message}");

            _navigationManager.NavigateTo("admin/banner/banner-listado");
        }

        public async Task<ServiceResponse<Banner>> GetBannerById(int id)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<Banner>>($"api/Banner/GetBannerById?BannerId={id}");
            return response!;
        }

        public async Task GetBanners()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Banner>>>($"api/Banner/GetBanners");
            if (response != null && response.Data != null)
            {
                Banners = response.Data;
            }

            if (Banners.Count == 0)
            {
                Mensaje = "No se encontraron banners registrados.";
            }
        }

        public async Task GetBannersPaginado(int pagina)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<BannerListadoDto>>($"api/Banner/GetBannersPaginado?pagina={pagina}");
            if (response != null && response.Data != null)
            {
                Banners = response.Data.Banners;
                PaginaActual = response.Data.PaginaActual;
                PaginasTotales = response.Data.Paginas;
            }

            if (Banners.Count == 0)
            {
                Mensaje = "No se encontraron banners registrados.";
            }

            OnChange?.Invoke();
        }

        public async Task UpdateBanner(Banner banner)
        {
            await _http.PutAsJsonAsync("api/Banner/UpdateBanner", banner);
        }
    }
}
