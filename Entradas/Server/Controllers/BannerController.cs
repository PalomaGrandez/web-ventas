using Entradas.Server.Helpers;
using Entradas.Shared.DTO.BannerDto;

namespace Entradas.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        
        [HttpGet]
        [Route("GetBanners")]
        public async Task<ActionResult<ServiceResponse<List<Banner>>>> GetBanners()
        {
            var result = await _bannerService.GetBanners();
            return Ok(result);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpGet]
        [Route("GetBannersPaginado")]
        public async Task<ActionResult<ServiceResponse<BannerListadoDto>>> GetBannersPaginado([FromQuery] int pagina)
        {
            var result = await _bannerService.GetBannersPaginado(pagina);
            return Ok(result);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpGet]
        [Route("GetBannerById")]
        public async Task<ActionResult<ServiceResponse<Banner>>> GetBannerById([FromQuery] int BannerId)
        {
            var result = await _bannerService.GetBannerById(BannerId);
            return Ok(result);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPost]
        [Route("CreateBanner")]
        public async Task<ActionResult<ServiceResponse<Banner>>> CreateBanner(Banner banner)
        {
            var result = await _bannerService.CreateBanner(banner);
            return Ok(result);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPut]
        [Route("UpdateBanner")]
        public async Task<ActionResult<ServiceResponse<Banner>>> UpdateBanner(Banner banner)
        {
            var result = await _bannerService.UpdateBanner(banner);
            return Ok(result);
        }
    }
}
