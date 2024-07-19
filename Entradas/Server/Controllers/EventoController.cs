using Entradas.Server.Helpers;
using Entradas.Shared.Models;

namespace Entradas.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventoController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [Authorize(Roles =Roles.ADMIN)]
        [HttpPost]
        [Route("CreateEvento")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateEvento(EventoRegistroDto dto)
        {
            var result = await _eventoService.CreateEvento(dto);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetEventos")]
        public async Task<ActionResult<ServiceResponse<List<Evento>>>> GetEventos()
        {
            var result = await _eventoService.GetEventos();
            return Ok(result);
        }


        [HttpGet]
        [Route("GetEventosPaginado")]
        public async Task<ActionResult<ServiceResponse<EventoPaginadoDto>>> GetEventosPaginado([FromQuery] int pagina)
        {
            var result = await _eventoService.GetEventosPaginado(pagina);
            return Ok(result);
        }

        [HttpGet]
        [Route("BuscarEventoPaginado")]
        public async Task<ActionResult<ServiceResponse<EventoPaginadoDto>>> BuscarEventoPaginado([FromQuery] int pagina, 
                                                                                                [FromQuery] string? nombre,
                                                                                                [FromQuery] string? informacion,
                                                                                                [FromQuery] string? ubicacion)
        {
            var result = await _eventoService.BuscarEventoPaginado(pagina,nombre,informacion,ubicacion);
            return Ok(result);
        }

        //[Authorize(Roles = Roles.ADMIN)]
        [HttpGet]
        [Route("GetEventoPorId")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> GetEventoPorId([FromQuery] int eventoId)
        {
            var result = await _eventoService.GetEventoPorId(eventoId);
            return Ok(result);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPut]
        [Route("UpdateEvento")]
        public async Task<ActionResult<ServiceResponse<int>>> UpdateEvento(EventoRegistroDto dto)
        {
            var result = await _eventoService.UpdateEvento(dto);
            return Ok(result);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPut]
        [Route("DeleteEvento")]
        public async Task<ActionResult<ServiceResponse<int>>> DeleteEvento(int eventoId)
        {
            var result = await _eventoService.DeleteEvento(eventoId);
            return Ok(result);
        }
    }
}
