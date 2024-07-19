using Entradas.Shared.DTO.CategoriaDto;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Entradas.Client.Services.CategoriaService
{
    public class CategoriaService : ICategoriaService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public CategoriaService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }
        public List<Categoria> Categorias { get; set; } = new List<Categoria>();
        public string Mensaje { get; set; } = string.Empty;
        public int PaginaActual { get; set; } = 1;
        public int PaginasTotales { get; set; } = 0;

        public event Action? OnChange;

        public async Task CreateCategoria(Categoria categoria)
        {
            var response = await _http.PostAsJsonAsync("api/Categoria/CreateCategoria", categoria);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Categoria>>();

            Console.WriteLine($"Crear Categoria: {result.Message}");

            _navigationManager.NavigateTo("admin/categoria/categoria-listado");
        }

        public async Task<ServiceResponse<Categoria>> GetCategoriaById(int id)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<Categoria>>($"api/Categoria/GetCategoriaById?CategoriaId={id}");
            return response!;
        }

        public async Task GetCategorias()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Categoria>>>($"api/Categoria/GetCategorias");
            if (response != null && response.Data != null)
            {
                Categorias = response.Data;
            }

            if (Categorias.Count == 0)
            {
                Mensaje = "No se encontraorn categorias registradas.";
            }
        }

        public async Task GetCategoriasPaginado(int pagina)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<CategoriaPaginadoDto>>($"api/Categoria/GetCategoriasPaginado?pagina={pagina}");
            if (response != null && response.Data != null)
            {
                Categorias = response.Data.Categorias;
                PaginaActual = response.Data.PaginaActual;
                PaginasTotales = response.Data.Paginas;
            }

            if (Categorias.Count == 0)
            {
                Mensaje = "No se encontraron categorias registradas.";
            }

            OnChange?.Invoke();
        }

        public async Task UpdateCategoria(Categoria categoria)
        {
            await _http.PutAsJsonAsync("api/Categoria/UpdateCategoria", categoria);
            //_navigationManager.NavigateTo("Maestros/categorias");
        }
    }
}
