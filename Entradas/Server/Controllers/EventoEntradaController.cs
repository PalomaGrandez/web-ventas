using Entradas.Server.Helpers;
using Entradas.Shared.DTO.EventoEntradaDto;

namespace Entradas.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoEntradaController : ControllerBase
    {
        private readonly IEventoEntradaService _eventoEntradaService;

        public EventoEntradaController(IEventoEntradaService eventoEntradaService)
        {
            _eventoEntradaService = eventoEntradaService;
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPost]
        [Route("CreateEventoEntrada")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateEventoEntrada(EventoEntrada eventoEntradaRegistro)
        {
            var result = await _eventoEntradaService.CreateEventoEntrada(eventoEntradaRegistro);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetEventoEntradasPaginado")]
        public async Task<ActionResult<ServiceResponse<EventoEntradaPaginadoDto>>> GetEventoEntradasPaginado([FromQuery] int pagina)
        {
            var result = await _eventoEntradaService.GetEventoEntradasPaginado(pagina);
            return Ok(result);
        }


        [HttpGet]
        [Route("GetEventoEntradasPorEvento")]
        public async Task<ActionResult<ServiceResponse<EventoEntradaPaginadoDto>>> GetEventoEntradasPorEvento([FromQuery] int pagina, [FromQuery] int eventoId)
        {
            var result = await _eventoEntradaService.GetEventoEntradasPorEvento(pagina, eventoId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetEventoEntradaPorId")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> GetEventoEntradaPorId([FromQuery] int eventoEntradaId)
        {
            var result = await _eventoEntradaService.GetEventoEntradaPorId(eventoEntradaId);
            return Ok(result);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPut]
        [Route("UpdateEventoEntrada")]
        public async Task<ActionResult<ServiceResponse<int>>> UpdateEventoEntrada(EventoEntrada eventoEntrada)
        {
            var result = await _eventoEntradaService.UpdateEventoEntrada(eventoEntrada);
            return Ok(result);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPut]
        [Route("DeleteEventoEntrada")]
        public async Task<ActionResult<ServiceResponse<int>>> DeleteEventoEntrada(int eventoEntradaId)
        {
            var result = await _eventoEntradaService.DeleteEventoEntrada(eventoEntradaId);
            return Ok(result);
        }
    }
}
