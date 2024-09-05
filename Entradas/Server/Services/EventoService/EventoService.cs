using Entradas.Server.Mappers;
using Entradas.Shared.Models;

namespace Entradas.Server.Services.EventoService
{
    public class EventoService : IEventoService
    {
        private readonly DataContext _context;

        public EventoService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<int>> CreateEvento(EventoRegistroDto dto)
        {
            try
            {
                Evento evento = new();
                evento = EventoMapper.ToEntity(dto);

                var result = _context.Evento.Add(evento);
                var dbResult = await _context.SaveChangesAsync();

                return new ServiceResponse<int>
                {
                    Data = dbResult,
                    Message = result.ToString(),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<int>
                {
                    Data = 0,
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<ServiceResponse<Evento>> GetEventoPorId(int eventoId)
        {
            ServiceResponse<Evento> response = new();

            var evento = await _context.Evento.FirstOrDefaultAsync(c => c.EventoId == eventoId);

            if (evento == null)
            {
                response.Success = false;
                response.Message = "No se encontró el evento.";
                return response;
            }
            response.Data = evento;
            response.Message = "Evento encontrado";
            return response;
        }

        public async Task<ServiceResponse<List<Evento>>> GetEventos()
        {
            ServiceResponse<List<Evento>> response = new();

            var eventos = await _context.Evento
                .Where(e => e.FlagEliminado == false) // Filtrar por eventos no eliminados
                .ToListAsync();

            if (eventos == null || eventos.Count == 0)
            {
                response.Success = false;
                response.Message = "No se encontraron eventos registrados.";
                return response;
            }
            else
            {
                response.Success = true;
                response.Data = eventos;
                response.Message = $"{eventos.Count} eventos encontrados.";
                return response;
            }
        }

        public async Task<ServiceResponse<EventoPaginadoDto>> GetEventosPaginado(int pagina)
        {
            ServiceResponse<EventoPaginadoDto> response = new();

            var resultadosPorPagina = 6f;
            var registrosTotales = await _context.Evento
                .Where(x => x.FlagEliminado == false) // Filtrar por eventos no eliminados
                .CountAsync();

            var cantidadPaginas = Math.Ceiling(registrosTotales / resultadosPorPagina);

            if (registrosTotales > 0)
            {
                var eventos = await _context.Evento
                    .Where(x => x.FlagEliminado == false) // Filtrar por eventos no eliminados
                    .OrderBy(c => c.EventoId)
                    .Skip((pagina - 1) * (int)resultadosPorPagina)
                    .Take((int)resultadosPorPagina)
                    .ToListAsync();

                if (eventos == null || eventos.Count == 0)
                {
                    response.Success = false;
                    response.Message = "No se encontraron eventos registrados.";
                    return response;
                }
                else
                {
                    EventoPaginadoDto eventoPaginadoDto = new()
                    {
                        Eventos = eventos,
                        PaginaActual = pagina,
                        Paginas = (int)cantidadPaginas,
                        RegistrosTotales = registrosTotales
                    };

                    response.Success = true;
                    response.Data = eventoPaginadoDto;

                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "No se encontraron eventos registrados.";
                return response;
            }
        }

        public async Task<ServiceResponse<EventoPaginadoDto>> BuscarEventoPaginado(int pagina, string? nombre, string? informacion, string? ubicacion)
        {
            var response = new ServiceResponse<EventoPaginadoDto>();

            try
            {
                var resultadosPorPagina = 6;
                var eventosQueryable = _context.Evento
                    .Where(x => x.FlagEliminado == false) // Filtrar por eventos no eliminados
                    .AsQueryable();

                if (!string.IsNullOrEmpty(nombre))
                    eventosQueryable = eventosQueryable.Where(x => x.Nombre.Contains(nombre));

                if (!string.IsNullOrEmpty(informacion))
                    eventosQueryable = eventosQueryable.Where(x => x.Informacion.Contains(informacion));

                if (!string.IsNullOrEmpty(ubicacion))
                    eventosQueryable = eventosQueryable.Where(x => x.Ubicacion.Contains(ubicacion));

                var registrosTotales = await eventosQueryable.CountAsync();

                if (registrosTotales == 0)
                {
                    response.Success = false;
                    response.Message = "No se encontraron eventos registrados.";
                    return response;
                }

                var cantidadPaginas = (int)Math.Ceiling(registrosTotales / (double)resultadosPorPagina);

                var eventos = await eventosQueryable
                    .OrderBy(c => c.EventoId)
                    //.Skip((pagina - 1) * resultadosPorPagina)
                    //.Take(resultadosPorPagina)
                    .ToListAsync();

                var eventoPaginadoDto = new EventoPaginadoDto
                {
                    Eventos = eventos,
                    PaginaActual = pagina,
                    Paginas = cantidadPaginas,
                    RegistrosTotales = registrosTotales
                };

                response.Success = true;
                response.Data = eventoPaginadoDto;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Se produjo un error al buscar eventos: " + ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Evento>>> BuscarEvento(string? nombre, string? informacion, string? ubicacion)
        {
            var response = new ServiceResponse<List<Evento>>();

            try
            {
                var eventosQueryable = _context.Evento
                    .Where(x => x.FlagEliminado == false) // Filtrar por eventos no eliminados
                    .AsQueryable();

                if (!string.IsNullOrEmpty(nombre))
                    eventosQueryable = eventosQueryable.Where(x => x.Nombre.Contains(nombre));

                if (!string.IsNullOrEmpty(informacion))
                    eventosQueryable = eventosQueryable.Where(x => x.Informacion.Contains(informacion));

                if (!string.IsNullOrEmpty(ubicacion))
                    eventosQueryable = eventosQueryable.Where(x => x.Ubicacion.Contains(ubicacion));

                var registrosTotales = await eventosQueryable.CountAsync();

                if (registrosTotales == 0)
                {
                    response.Success = false;
                    response.Message = "No se encontraron eventos registrados.";
                    return response;
                }

                var eventos = await eventosQueryable
                    .OrderBy(c => c.EventoId)
                    .ToListAsync();
                
                response.Success = true;
                response.Data = eventos;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Se produjo un error al buscar eventos: " + ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<int>> UpdateEvento(EventoRegistroDto dto)
        {
            var dbEvento = await _context.Evento.FirstOrDefaultAsync(c => c.EventoId == dto.EventoId);

            if (dbEvento == null)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Evento no encontrado."
                };
            }

            dbEvento.Nombre = dto.Nombre;
            dbEvento.Informacion = dto.Informacion;
            dbEvento.Ubicacion = dto.Ubicacion;
            dbEvento.Imagen = dto.Imagen;
            dbEvento.CategoriaId = dto.CategoriaId;
            dbEvento.CapacidadTotal = dto.CapacidadTotal;

            var result = _context.Evento.Update(dbEvento);
            var dbResult = await _context.SaveChangesAsync();

            return new ServiceResponse<int>
            {
                Data = dbResult,
                Message = "Evento actualizado exitosamente."
            };
        }

        public async Task<ServiceResponse<int>> DeleteEvento(int eventoId)
        {
            var dbEvento = await _context.Evento.FirstOrDefaultAsync(c => c.EventoId == eventoId);

            if (dbEvento == null)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Evento no encontrado."
                };
            }

            dbEvento.FlagEliminado = true;

            var result = _context.Evento.Update(dbEvento);
            var dbResult = await _context.SaveChangesAsync();

            return new ServiceResponse<int>
            {
                Data = dbResult,
                Message = "Evento eliminado exitosamente."
            };
        }
    }
}
