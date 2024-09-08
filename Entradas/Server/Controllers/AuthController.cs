

using Entradas.Server.Helpers;
using Entradas.Server.Services.EmailService;

namespace Entradas.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService,IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }


        [HttpGet("CheckEmailExists/{email}")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            var exists = await _authService.CheckEmailExists(email);
            return Ok(exists);
        }

        [HttpGet("CheckUsernameExists/{username}")]
        public async Task<ActionResult<bool>> CheckUsernameExists(string username)
        {
            var exists = await _authService.CheckUsernameExists(username);
            return Ok(exists);
        }


        [HttpPost]
        [Route("Registro")]
        public async Task<ActionResult<ServiceResponse<UsuarioRegistroDto>>> Registro(UsuarioRegistroDto request)
        {
            var response = await _authService.Registro(request);
            if (response.Success)
            {
                EmailRequestDto emailRequest = new()
                {
                    Para = request.Email,
                    Asunto = "Fabrica de Entradas - Registro"
                };
                //await _emailService.SendEmailRegistroUsuarioAsync(emailRequest);
                return Ok(response);
            }
            return BadRequest(response);
        }



        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UsuarioLoginDto request)
        {
            var response = await _authService.Login(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Roles =Roles.ADMIN )]
        [HttpGet]
        [Route("GetUsuarios")]
        public async Task<ActionResult<ServiceResponse<UsuarioListadoDto>>> GetUsuarios(bool paginar, int pagina=1)
        {
            var response = await _authService.GetUsuarios(paginar, pagina);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
