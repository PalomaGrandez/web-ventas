using Entradas.Shared.Models;

namespace Entradas.Shared.DTO.OrdenDto
{
    public class OrdenPaginadoDto
    {
        public List<Orden> Ordenes{ get; set; } = new();
        public int Paginas { get; set; }
        public int PaginaActual { get; set; }
        public int RegistrosTotales { get; set; }
    }
}
