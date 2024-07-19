namespace Entradas.Client.Services.CategoriaService
{
    public interface ICategoriaService
    {
        event Action? OnChange;
        public List<Categoria> Categorias { get; set; }
        public string Mensaje { get; set; }
        public int PaginaActual { get; set; }
        public int PaginasTotales { get; set; }
        Task GetCategoriasPaginado(int pagina);
        Task GetCategorias();
        Task<ServiceResponse<Categoria>> GetCategoriaById(int id);        
        Task CreateCategoria(Categoria categoria);
        Task UpdateCategoria(Categoria categoria);
    }
}
