using Entradas.Server.Helpers;
using Entradas.Shared.DTO.EventoFechaDto;

namespace Entradas.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoFechaController : ControllerBase
    {
        private readonly IEventoFechaService _eventoFechaService;

        public EventoFechaController(IEventoFechaService eventoFechaService)
        {
            _eventoFechaService = eventoFechaService;
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPost]
        [Route("CreateEventoFecha")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateEventoFecha(EventoFecha eventoFecha)
        {
            var result = await _eventoFechaService.CreateEventoFechas(eventoFecha);
            return Ok(result);
        }


        [HttpGet]
        [Route("GetEventoFechasPorEvento")]
        public async Task<ActionResult<ServiceResponse<EventoFechaPaginadoDto>>> GetEventoFechasPorEvento([FromQuery] int pagina, [FromQuery] int eventoId)
        {
            var result = await _eventoFechaService.GetEventoFechasPorEvento(pagina, eventoId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetEventoFechaPorId")]
        public async Task<ActionResult<ServiceResponse<EventoFecha>>> GetEventoFechaPorId([FromQuery] int eventoFechaId)
        {
            var result = await _eventoFechaService.GetEventoFechaPorId(eventoFechaId);
            return Ok(result);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPut]
        [Route("UpdateEventoFecha")]
        public async Task<ActionResult<ServiceResponse<int>>> UpdateEventoFecha(EventoFecha eventoFecha)
        {
            var result = await _eventoFechaService.UpdateEventoFechas(eventoFecha);
            return Ok(result);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPut]
        [Route("DeleteEventoFecha")]
        public async Task<ActionResult<ServiceResponse<int>>> DeleteEventoEntrada(int eventoFechaId)
        {
            var result = await _eventoFechaService.DeleteEventoFecha(eventoFechaId);
            return Ok(result);
        }
    }
}
