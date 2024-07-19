using Entradas.Shared.DTO.BannerDto;

namespace Entradas.Server.Services.BannerService
{
    public interface IBannerService
    {
        Task<ServiceResponse<List<Banner>>> GetBanners();
        Task<ServiceResponse<BannerListadoDto>> GetBannersPaginado(int pagina);
        Task<ServiceResponse<Banner>> GetBannerById(int BannerId);
        Task<ServiceResponse<Banner>> CreateBanner(Banner banner);
        Task<ServiceResponse<Banner>> UpdateBanner(Banner banner);
    }
}
