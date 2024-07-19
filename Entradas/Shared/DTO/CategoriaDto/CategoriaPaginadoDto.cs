using Entradas.Shared.Models;

namespace Entradas.Shared.DTO.CategoriaDto
{
    public class CategoriaPaginadoDto
    {
        public List<Categoria> Categorias { get; set; } = new();
        public int Paginas { get; set; }
        public int PaginaActual { get; set; }
        public int RegistrosTotales { get; set; }
    }
}
