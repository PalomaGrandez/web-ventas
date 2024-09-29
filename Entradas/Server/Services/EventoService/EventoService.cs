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



        public async Task<ServiceResponse<EventoEntrada>> ObtenerEventoEntradaDisponible(int eventoEntradaId)
        {
            var eventoEntrada = await _context.EventoEntrada.FirstOrDefaultAsync(e => e.EventoEntradaId == eventoEntradaId);

            if (eventoEntrada == null)
            {
                return new ServiceResponse<EventoEntrada>
                {
                    Success = false,
                    Message = "Evento no encontrado"
                };
            }

            return new ServiceResponse<EventoEntrada>
            {
                Data = eventoEntrada,
                Success = true
            };
        }

        public async Task<ServiceResponse<int>> ReducirCapacidadEvento(List<EventoEntradaCompraDto> entradasCompradas)
        {
            var response = new ServiceResponse<int>();

            foreach (var entrada in entradasCompradas)
            {
                // Obtener la entrada específica por su ID en EventoEntrada
                var eventoEntrada = await _context.EventoEntrada.FirstOrDefaultAsync(e => e.EventoEntradaId == entrada.EventoEntradaId);
                if (eventoEntrada == null)
                {
                    response.Success = false;
                    response.Message = $"Evento entrada con ID {entrada.EventoEntradaId} no encontrada.";
                    return response;
                }

                // Verificar si hay suficiente capacidad en la categoría de entrada específica
                if (eventoEntrada.Capacidad < entrada.CantidadComprada)
                {
                    response.Success = false;
                    response.Message = $"No hay suficiente capacidad disponible para la entrada con ID {entrada.EventoEntradaId}.";
                    return response;
                }

                // Reducir la capacidad en la categoría de entrada específica
                eventoEntrada.Capacidad -= entrada.CantidadComprada;

                // Obtener el evento asociado para actualizar su capacidad total
                var evento = await _context.Evento.FirstOrDefaultAsync(e => e.EventoId == eventoEntrada.EventoId);
                if (evento == null)
                {
                    response.Success = false;
                    response.Message = $"Evento asociado con la entrada {entrada.EventoEntradaId} no encontrado.";
                    return response;
                }

                // Validar que CapacidadTotal es suficiente en el evento (opcional)
                if (evento.CapacidadTotal < entrada.CantidadComprada)
                {
                    response.Success = false;
                    response.Message = $"Capacidad total insuficiente para el evento con ID {evento.EventoId}.";
                    return response;
                }

                // Reducir la capacidad total del evento
                evento.CapacidadTotal -= entrada.CantidadComprada;

                // Actualizar ambas tablas
                _context.EventoEntrada.Update(eventoEntrada);
                _context.Evento.Update(evento);
            }

            // Guardar los cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Capacidad actualizada correctamente para todas las entradas.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al guardar los cambios: {ex.Message}";
            }

            return response;
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

    public class EventoEntradaCompraDto
    {
        public int EventoEntradaId { get; set; }
        public int CantidadComprada { get; set; }
    }

}
