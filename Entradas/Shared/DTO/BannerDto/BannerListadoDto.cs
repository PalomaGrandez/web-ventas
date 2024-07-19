using Entradas.Shared.Models;

namespace Entradas.Shared.DTO.BannerDto
{
    public class BannerListadoDto
    {
        public List<Banner> Banners { get; set; } = new();
        public int Paginas { get; set; }
        public int PaginaActual { get; set; }
        public int RegistrosTotales { get; set; }
    }
}
