using Entradas.Shared.DTO;
using Entradas.Shared.DTO.UsuarioDto;
using Entradas.Shared.Wrappers;

namespace Entradas.Server.Services.AuthService
{
    public interface IAuthService
    {

        Task<bool> CheckEmailExists(string email);
        Task<bool> CheckUsernameExists(string username);
        Task<ServiceResponse<int>> Registro(UsuarioRegistroDto request);
        Task<ServiceResponse<string>> Login(UsuarioLoginDto request);
        //Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);
        //Task<bool> IsUserAuthenticated();

        Task<ServiceResponse<UsuarioListadoDto>> GetUsuarios(bool paginar = false, int pagina = 1);
        Task<ServiceResponse<Usuario>> GetUsuarioById(int usuarioId);
    }
}
