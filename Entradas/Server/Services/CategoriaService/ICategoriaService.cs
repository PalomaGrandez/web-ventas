using Entradas.Shared.DTO.CategoriaDto;

namespace Entradas.Server.Services.CategoriaService
{
    public interface ICategoriaService
    {
        Task<ServiceResponse<List<Categoria>>> GetCategorias();
        Task<ServiceResponse<CategoriaPaginadoDto>> GetCategoriasPaginado(int pagina);
        Task<ServiceResponse<Categoria>> GetCategoriaById(int CategoriaId);        
        Task<ServiceResponse<Categoria>> CreateCategoria(Categoria categoria);
        Task<ServiceResponse<Categoria>> UpdateCategoria(Categoria categoria);
    }
}
