using Entradas.Shared.DTO.EventoEntradaDto;
using Entradas.Shared.DTO.EventoFechaDto;
using Entradas.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Entradas.Server.Services.EventoEntradaService
{
    public class EventoEntradaService : IEventoEntradaService
    {
        private readonly DataContext _context;

        public EventoEntradaService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<int>> CreateEventoEntrada(EventoEntrada eventoEntrada)
        {
            try
            {
                var resul = _context.EventoEntrada.Add(eventoEntrada);
                var dbResult = await _context.SaveChangesAsync();

                return new ServiceResponse<int>
                {
                    Data = dbResult,
                    Message = resul.ToString(),
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

        public async Task<ServiceResponse<int>> DeleteEventoEntrada(int eventoEntradaId)
        {
            var dbEventoEntrada = await _context.EventoEntrada
                                    .FirstOrDefaultAsync(c => c.EventoEntradaId == eventoEntradaId);

            if (dbEventoEntrada == null)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Evento Entrada no encontrado."
                };
            }

            dbEventoEntrada.FlagEliminado = true;

            var result = _context.EventoEntrada.Update(dbEventoEntrada);
            var dbResult = await _context.SaveChangesAsync();

            return new ServiceResponse<int>
            {
                Data = dbResult,
                Message = "Evento Entrada eliminado exitosamente."
            };
        }

        public async Task<ServiceResponse<EventoEntrada>> GetEventoEntradaPorId(int eventoEntradaId)
        {
            ServiceResponse<EventoEntrada> response = new();

            var eventoEntrada = await _context.EventoEntrada
                                .FirstOrDefaultAsync(c => c.EventoEntradaId == eventoEntradaId);

            if (eventoEntrada == null)
            {
                response.Success = false;
                response.Message = "No se encontro la entrada para el evento.";
                return response;
            }

            response.Data = eventoEntrada;
            response.Message = "Entrada encontrada";
            return response;
        }

        public async Task<ServiceResponse<EventoEntradaPaginadoDto>> GetEventoEntradasPaginado(int pagina)
        {
            ServiceResponse<EventoEntradaPaginadoDto> response = new();

            var resultadosPorPagina = 6f;
            var registrosTotales = _context.EventoEntrada
     .Where(p => p.FlagEliminado == false)
     .Count();

            Console.WriteLine($"Total de registros sin eliminar: {registrosTotales}");

            var cantidadPaginas = Math.Ceiling(registrosTotales / resultadosPorPagina);
            if (registrosTotales > 0)
            {
                var eventoEntradas = await _context.EventoEntrada.
                        Where(p => p.FlagEliminado == false).
                        OrderBy(c => c.EventoEntradaId).
                        Skip((pagina - 1) * (int)resultadosPorPagina).
                        Take((int)resultadosPorPagina).
                        ToListAsync();

                if (eventoEntradas == null)
                {
                    response.Success = false;
                    response.Message = "No se encontraron entradas registradas para el evento.";
                    return response;
                }
                else
                {
                    EventoEntradaPaginadoDto eventoEntradaPaginadoDto = new()
                    {
                        EventoEntradas = eventoEntradas,
                        PaginaActual = pagina,
                        Paginas = (int)cantidadPaginas,
                        RegistrosTotales = registrosTotales
                    };

                    response.Success = true;
                    response.Data = eventoEntradaPaginadoDto;

                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "No se encontraron entradas registradas para el evento.";
                return response;
            }
        }

        public async Task<ServiceResponse<EventoEntradaPaginadoDto>> GetEventoEntradasPorEvento(int pagina, int eventoId)
        {
            ServiceResponse<EventoEntradaPaginadoDto> response = new();

            var resultadosPorPagina = 6f;
            int registrosTotales;
            List<EventoEntrada> eventoEntradas = new();

            if (eventoId > 0)
            {
                registrosTotales = await _context.EventoEntrada
                                                 .Where(p => p.EventoId == eventoId && p.FlagEliminado == false)
                                                 .CountAsync();

                eventoEntradas = await _context.EventoEntrada
                                                .Where(p => p.EventoId == eventoId && p.FlagEliminado == false)
                                                .OrderBy(p => p.EventoEntradaId)
                                                .Skip((pagina - 1) * (int)resultadosPorPagina)
                                                .Take((int)resultadosPorPagina)
                                                .ToListAsync();
            }
            else
            {
                registrosTotales = await _context.EventoEntrada
                                                 .Where(p => p.FlagEliminado == false)
                                                 .CountAsync();

                eventoEntradas = await _context.EventoEntrada
                                                .Where(p => p.FlagEliminado == false)
                                                .OrderBy(p => p.EventoEntradaId)
                                                .Skip((pagina - 1) * (int)resultadosPorPagina)
                                                .Take((int)resultadosPorPagina)
                                                .ToListAsync();
            }

            var cantidadPaginas = Math.Ceiling(registrosTotales / resultadosPorPagina);

            if (eventoEntradas.Any())
            {
                EventoEntradaPaginadoDto eventoEntradaPaginadoDto = new()
                {
                    EventoEntradas = eventoEntradas,
                    PaginaActual = pagina,
                    Paginas = (int)cantidadPaginas,
                    RegistrosTotales = registrosTotales
                };

                response.Success = true;
                response.Data = eventoEntradaPaginadoDto;
                response.Message = $"{eventoEntradas.Count} entradas registradas.";
            }
            else
            {
                response.Success = false;
                response.Message = "No se encontraron entradas registradas para el evento.";
            }

            return response;
        }

        public async Task<ServiceResponse<int>> UpdateEventoEntrada(EventoEntrada eventoEntrada)
        {
            var dbEventoEntrada = await _context.EventoEntrada
                                .FirstOrDefaultAsync(c => c.EventoEntradaId == eventoEntrada.EventoEntradaId);

            if (dbEventoEntrada == null)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Evento Entrada no encontrado."
                };
            }

            dbEventoEntrada.Tipo = eventoEntrada.Tipo;
            dbEventoEntrada.PrecioRegular = eventoEntrada.PrecioRegular;
            dbEventoEntrada.PrecioDescuento = eventoEntrada.PrecioDescuento;
            dbEventoEntrada.FechaVigenciaDescuento = eventoEntrada.FechaVigenciaDescuento;
            dbEventoEntrada.Capacidad = eventoEntrada.Capacidad;

            var result = _context.EventoEntrada.Update(dbEventoEntrada);
            var dbResult = await _context.SaveChangesAsync();

            return new ServiceResponse<int>
            {
                Data = dbResult,
                Message = "Evento Entrada actualizado exitosamente."
            };
        }
    }
}
