namespace Entradas.Shared.DTO.BannerDto
{
    public class BannerRegistroDto
    {
        public int BannerId { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public bool FlagActivo { get; set; }
    }
}
