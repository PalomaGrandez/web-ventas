namespace Entradas.Shared.DTO.UsuarioDto
{
    public class UsuarioListadoDto
    {
        public List<UsuarioItemDto> Usuarios { get; set; } = new();
        public int Paginas { get; set; }
        public int PaginaActual { get; set; }
        public int RegistrosTotales { get; set; }
    }
}
