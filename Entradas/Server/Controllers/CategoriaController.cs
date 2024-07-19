using Entradas.Server.Helpers;
using Entradas.Shared.DTO.CategoriaDto;

namespace Entradas.Server.Controllers
{
    [Authorize (Roles =Roles.ADMIN)]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        [Route("GetCategorias")]
        public async Task<ActionResult<ServiceResponse<List<Categoria>>>> GetCategorias()
        {
            var result = await _categoriaService.GetCategorias();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetCategoriasPaginado")]
        public async Task<ActionResult<ServiceResponse<CategoriaPaginadoDto>>> GetCategoriasPaginado([FromQuery] int pagina)
        {
            var result = await _categoriaService.GetCategoriasPaginado(pagina);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetCategoriaById")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> GetCategoriaById([FromQuery] int CategoriaId)
        {
            var result = await _categoriaService.GetCategoriaById(CategoriaId);
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateCategoria")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> CreateCategoria(Categoria categoria)
        {
            var result = await _categoriaService.CreateCategoria(categoria);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateCategoria")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> UpdateCategoria(Categoria categoria)
        {
            var result = await _categoriaService.UpdateCategoria(categoria);
            return Ok(result);
        }
    }
}
