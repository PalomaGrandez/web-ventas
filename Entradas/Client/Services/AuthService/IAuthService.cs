using Entradas.Shared.DTO;
using Entradas.Shared.DTO.UsuarioDto;

namespace Entradas.Client.Services.AuthService
{
    public interface IAuthService
    {
        event Action? OnChange;
        public List<UsuarioItemDto> Usuarios { get; set; }
        public string Mensaje { get; set; }
        public int PaginaActual { get; set; }
        public int PaginasTotales { get; set; }
        Task GetUsuarios(bool paginar = false, int pagina = 1);
        Task<ServiceResponse<int>> Registro(UsuarioRegistroDto request);
        Task<ServiceResponse<string>> Login(UsuarioLoginDto request);
        Task<bool> IsUserAuthenticated();
        Task<string> GetUserRole();
        Task<int> GetUserId();

        Task<bool> EmailExists(string email);
        Task<bool> UsernameExists(string username);
    }
}
