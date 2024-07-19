using Entradas.Server.Services.EmailService;

namespace Entradas.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail([FromBody] EmailRequestDto request)
        {
            await _emailService.SendEmailRegistroUsuarioAsync(request);

            return Ok("El Email fue enviado satisfactoriamente.");
        }
    }
}
