using Entradas.Shared.DTO.CategoriaDto;

namespace Entradas.Server.Services.CategoriaService
{
    public class CategoriaService : ICategoriaService
    {
        private readonly DataContext _context;

        public CategoriaService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<Categoria>> CreateCategoria(Categoria categoria)
        {            
            try {
                var result = _context.Categoria.Add(categoria);
                await _context.SaveChangesAsync();

                return new ServiceResponse<Categoria>
                {
                    Data= categoria,
                    Message = result.ToString(),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Categoria>
                {
                    Message = ex.Message,
                    Success = false
                };
            }                        
        }

        public async Task<ServiceResponse<Categoria>> GetCategoriaById(int CategoriaId)
        {
            ServiceResponse<Categoria> response = new();

            var categoria = await _context.Categoria.FirstOrDefaultAsync(c => c.CategoriaId == CategoriaId);
            if (categoria == null)
            {
                response.Success = false;
                response.Message = "No se encontro la categoria.";
                return response;
            }
            response.Data = categoria;
            return response;
        }

        public async Task<ServiceResponse<List<Categoria>>> GetCategorias()
        {
            ServiceResponse<List<Categoria>> response = new();

            var categorias = await _context.Categoria.ToListAsync();

            if (categorias == null)
            {
                response.Success = false;
                response.Message = "No se encontraron categorias registradas.";
                return response;
            }
            else
            {
                response.Success = true;
                response.Data = categorias;
                response.Message = $"{categorias.Count} categorias encontradas";
                return response;
            }
        }

        public async Task<ServiceResponse<CategoriaPaginadoDto>> GetCategoriasPaginado(int pagina)
        {
            ServiceResponse<CategoriaPaginadoDto> response = new();

            var resultadosPorPagina = 10f;
            var registrosTotales = _context.Categoria.Count();
            var cantidadPaginas = Math.Ceiling(registrosTotales / resultadosPorPagina);
            if (registrosTotales > 0)
            {
                var categorias = await _context.Categoria.                        
                        OrderBy(c => c.CategoriaId).
                        Skip((pagina - 1) * (int)resultadosPorPagina).
                        Take((int)resultadosPorPagina).
                        ToListAsync();

                if (categorias == null)
                {
                    response.Success = false;
                    response.Message = "No se encontraron categorias registradas.";
                    return response;
                }
                else
                {
                    CategoriaPaginadoDto categoriaPaginado = new()
                    {
                        Categorias = categorias,
                        PaginaActual = pagina,
                        Paginas = (int)cantidadPaginas,
                        RegistrosTotales = registrosTotales
                    };

                    response.Success = true;
                    response.Data = categoriaPaginado;

                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "No se encontraron categorias registradas.";
                return response;
            }
        }

        public async Task<ServiceResponse<Categoria>> UpdateCategoria(Categoria categoria)
        {
            var dbCategoria = await _context.Categoria.FirstOrDefaultAsync(c => c.CategoriaId == categoria.CategoriaId);

            if (dbCategoria == null)
            {
                return new ServiceResponse<Categoria>
                {
                    Success = false,
                    Message = "Categoria no encontrada."
                };
            }

            dbCategoria.Nombre = categoria.Nombre;           

            await _context.SaveChangesAsync();

            return new ServiceResponse<Categoria>
            {
                Data = dbCategoria,
                Message = "Categoria actualizada exitosamente."
            };
        }
    }
}
