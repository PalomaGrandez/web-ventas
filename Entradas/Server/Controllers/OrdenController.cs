using Entradas.Server.Helpers;
using Entradas.Server.Services.EmailService;
using Entradas.Server.Services.OrdenService;
using Entradas.Shared.DTO.OrdenDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Entradas.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenController : ControllerBase
    {
        private readonly IOrdenService _ordenService;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;

        public OrdenController(IOrdenService ordenService, IEmailService emailService, IAuthService authService)
        {
            _ordenService = ordenService;
            _emailService = emailService;
            _authService = authService;
        }

        [Authorize(Roles = Roles.CUSTOMER)]
        [HttpPost]
        [Route("CreateOrden")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateOrden(OrdenRegistroDto dto)
        {
            var response = await _ordenService.CreateOrden(dto);
            if (response.Success)
            {
                //var usuarioResponse = await _authService.GetUsuarioById((int)dto.UsuarioId!);
                //var usuario = usuarioResponse.Data;

                //var ordenResponse = await _ordenService.GetOrdenPorOrdenIdPorUsuarioId(response.Data, (int)dto.UsuarioId);
                //var orden = ordenResponse.Data;

                //EmailRequestDto emailRequestDto = new();
                //emailRequestDto.Para = usuario!.Email;
                //emailRequestDto.Asunto = "Pedido Registrado";
                var orderId = response.Data;
                await _emailService.SendEmailRegistroPedidoAsync(orderId, (int)dto.UsuarioId!);

                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPut]
        [Route("UpdateOrden")]
        public async Task<ActionResult<ServiceResponse<int>>> UpdateOrden(OrdenActualizarDto dto)
        {
            var response = await _ordenService.UpdateOrden(dto);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        [Route("GetOrdenesPaginado")]
        public async Task<ActionResult<ServiceResponse<OrdenPaginadoDto>>> GetOrdenesPaginado([FromQuery] int pagina)
        {
            var response = await _ordenService.GetOrdenesPaginado(pagina);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        [Route("GetOrdenPorId")]
        public async Task<ActionResult<ServiceResponse<OrdenPaginadoDto>>> GetOrdenPorId([FromQuery] int ordenId)
        {
            var response = await _ordenService.GetOrdenPorId(ordenId);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        [Route("GetOrdenPorUsuario")]
        public async Task<ActionResult<ServiceResponse<OrdenPaginadoDto>>> GetOrdenPorUsuario([FromQuery] int pagina, [FromQuery] int usuarioId)
        {
            var response = await _ordenService.GetOrdenesPorUsuario(pagina, usuarioId);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        [Route("GetOrdenPorOrdenIdPorUsuarioId")]
        public async Task<ActionResult<ServiceResponse<OrdenPaginadoDto>>> GetOrdenPorOrdenIdPorUsuarioId([FromQuery] int ordenId, [FromQuery] int usuarioId)
        {
            var response = await _ordenService.GetOrdenPorOrdenIdPorUsuarioId(ordenId, usuarioId);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPost]
        [Route("GenerarTickets")]
        public async Task<ActionResult<ServiceResponse<OrdenPaginadoDto>>> GenerarTickets([FromQuery] int ordenId)
        {
            var response = await _ordenService.GenerarTickets(ordenId);
            //var ordenResponse = await _ordenService.GetOrdenPorId(ordenId);
            //var orden = ordenResponse.Data;

            if (response.Success)
            {
                var ordenResponse = await _ordenService.GetOrdenPorId(ordenId);
                var orden = ordenResponse.Data;

                var usuarioResponse = await _authService.GetUsuarioById((int)orden!.UsuarioId!);
                var usuario = usuarioResponse.Data;

                EmailRequestDto emailRequestDto = new()
                {
                    Para = usuario!.Email,
                    Asunto = "Tickets Generados"
                };

                await _emailService.SendEmailGenerarTicketsAsync(emailRequestDto,ordenId );

                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpGet]
        [Route("GetOrdenTicketPorOrdenId")]
        public async Task<ActionResult<ServiceResponse<VwOrdenTicket>>> GetOrdenTicketPorOrdenId([FromQuery] int ordenId)
        {
            var response = await _ordenService.GetOrdenTicketPorOrdenId(ordenId);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        [Route("GetOrdenTicketPorOrdenTicketId")]
        public async Task<ActionResult<ServiceResponse<VwOrdenTicket>>> GetOrdenTicketPorOrdenTicketId([FromQuery] int ordenTicketId)
        {
            var response = await _ordenService.GetOrdenTicketPorOrdenTicketId(ordenTicketId);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [Authorize(Roles = Roles.CUSTOMER)]
        [HttpPut]
        [Route("TicketNominar")]
        public async Task<ActionResult<ServiceResponse<VwOrdenTicket>>> TicketNominar(OrdenTicketActualizarDto dto)
        {
            var response = await _ordenService.TicketNominar(dto);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
