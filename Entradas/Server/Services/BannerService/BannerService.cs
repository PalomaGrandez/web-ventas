using Entradas.Shared.DTO.BannerDto;
using Entradas.Shared.Models;

namespace Entradas.Server.Services.BannerService
{
    public class BannerService : IBannerService
    {
        private readonly DataContext _context;

        public BannerService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Banner>> GetBannerById(int BannerId)
        {
            ServiceResponse<Banner> response = new();

            var banner = await _context.Banner.FirstOrDefaultAsync(c => c.BannerId == BannerId);

            if (banner == null)
            {
                response.Success = false;
                response.Message = "No se encontro el banner.";
                return response;
            }
            response.Data = banner;
            return response;
        }

        public async Task<ServiceResponse<List<Banner>>> GetBanners()
        {
            ServiceResponse<List<Banner>> response = new();

            var banners = await _context.Banner
                                    .Where(x => x.FlagActivo == true)
                                    .ToListAsync();

            if (banners == null)
            {
                response.Success = false;
                response.Message = "No se encontraron banners registrados.";
                return response;
            }
            else
            {
                response.Success = true;
                response.Data = banners;
                response.Message = $"{banners.Count} banners encontrados";
                return response;
            }
        }

        public async Task<ServiceResponse<BannerListadoDto>> GetBannersPaginado(int pagina)
        {
            ServiceResponse<BannerListadoDto> response = new();

            var resultadosPorPagina = 10f;
            var registrosTotales = _context.Banner.Count();
            var cantidadPaginas = Math.Ceiling(registrosTotales / resultadosPorPagina);
            if (registrosTotales > 0)
            {
                var banners = await _context.Banner.
                        OrderBy(c => c.BannerId).
                        Skip((pagina - 1) * (int)resultadosPorPagina).
                        Take((int)resultadosPorPagina).
                        ToListAsync();

                if (banners == null)
                {
                    response.Success = false;
                    response.Message = "No se encontraron banners registrados.";
                    return response;
                }
                else
                {
                    BannerListadoDto bannerListadoDto = new()
                    {
                        Banners = banners,
                        PaginaActual = pagina,
                        Paginas = (int)cantidadPaginas,
                        RegistrosTotales = registrosTotales
                    };

                    response.Success = true;
                    response.Data = bannerListadoDto;

                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "No se encontraron Banners registrados.";
                return response;
            }
        }

        public async Task<ServiceResponse<Banner>> UpdateBanner(Banner banner)
        {
            var dbBanner = await _context.Banner.FirstOrDefaultAsync(c => c.BannerId == banner.BannerId);

            if (dbBanner == null)
            {
                return new ServiceResponse<Banner>
                {
                    Success = false,
                    Message = "Banner no encontrado."
                };
            }

            dbBanner.Imagen = banner.Imagen;
            dbBanner.FlagActivo = banner.FlagActivo;

            await _context.SaveChangesAsync();

            return new ServiceResponse<Banner>
            {
                Data = dbBanner,
                Message = "Banner actualizado exitosamente."
            };
        }
        public async Task<ServiceResponse<Banner>> CreateBanner(Banner banner)
        {
            try
            {
                var result = _context.Banner.Add(banner);
                await _context.SaveChangesAsync();

                return new ServiceResponse<Banner>
                {
                    Data = banner,
                    Message = result.ToString(),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Banner>
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
