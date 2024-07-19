namespace Entradas.Client.Services.BannerService
{
    public interface IBannerService
    {
        event Action? OnChange;
        public List<Banner> Banners { get; set; }
        public string Mensaje { get; set; }
        public int PaginaActual { get; set; }
        public int PaginasTotales { get; set; }
        Task GetBanners();
        Task GetBannersPaginado(int pagina);
        Task<ServiceResponse<Banner>> GetBannerById(int id);
        Task CreateBanner(Banner banner);
        Task UpdateBanner(Banner banner);
    }
}
