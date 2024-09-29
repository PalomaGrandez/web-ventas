using Entradas.Shared.DTO.UsuarioDto;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Entradas.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(HttpClient http, AuthenticationStateProvider authenticationStateProvider)
        {
            _http = http;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public List<UsuarioItemDto> Usuarios { get; set; } = new List<UsuarioItemDto>();
        public string Mensaje { get; set; } = string.Empty;
        public int PaginaActual { get; set; } = 1;
        public int PaginasTotales { get; set; } = 0;

        public event Action? OnChange;

        public async Task<string> GetUserRole()
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            var rolClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (rolClaim != null)
            {
                return rolClaim.Value;
            }
            return string.Empty;
        }
        public async Task<int> GetUserId()
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                return int.Parse(userIdClaim.Value);
            }
            return 0;
        }

        public async Task GetUsuarios(bool paginar = false, int pagina = 1)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<UsuarioListadoDto>>
                                         ($"api/Auth/GetUsuarios?paginar={paginar}&pagina={pagina}");

            if (response != null && response.Data != null)
            {
                Usuarios = response.Data.Usuarios;
                PaginaActual = response.Data.PaginaActual;
                PaginasTotales = response.Data.Paginas;
                Mensaje = response.Message;

            }

            if (Usuarios.Count == 0)
            {
                Mensaje = "No se encontraron usuarios registrados";
            }
            OnChange?.Invoke();
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var isAuthenticated = authState.User.Identity.IsAuthenticated;
            return isAuthenticated;
        }

        public async Task<ServiceResponse<string>> Login(UsuarioLoginDto request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/Login", request);
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }


        public async Task<bool> EmailExists(string email)
        {
            var response = await _http.GetFromJsonAsync<bool>($"api/Auth/CheckEmailExists/{email}");
            return response;
        }

        public async Task<bool> UsernameExists(string username)
        {
            var response = await _http.GetFromJsonAsync<bool>($"api/Auth/CheckUsernameExists/{username}");
            return response;
        }

        public async Task<ServiceResponse<int>> Registro(UsuarioRegistroDto request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/Registro", request);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();   

            return response;
        }
    }
}
