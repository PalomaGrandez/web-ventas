using Entradas.Shared.DTO.EventoEntradaDto;
using Entradas.Shared.DTO.EventoFechaDto;
using Entradas.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Entradas.Server.Services.EventoFechaService
{
    public class EventoFechaService : IEventoFechaService
    {
        private readonly DataContext _context;

        public EventoFechaService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<int>> CreateEventoFechas(EventoFecha eventoFecha)
        {
            try
            {
                var result = _context.EventoFecha.Add(eventoFecha);
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

        public async Task<ServiceResponse<int>> DeleteEventoFecha(int eventoFechaId)
        {

            var dbEventoFecha = await _context.EventoFecha
                                    .FirstOrDefaultAsync(c => c.EventoFechaId == eventoFechaId);

            if (dbEventoFecha == null)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Evento Fecha no encontrado."
                };
            }

            //var evento = EventoMapper.ToEntity(dto);
            dbEventoFecha.FlagEliminado = true;

            var result = _context.EventoFecha.Update(dbEventoFecha);
            var dbResult = await _context.SaveChangesAsync();

            return new ServiceResponse<int>
            {
                Data = dbResult,
                Message = "Evento Fecha eliminado exitosamente."
            };
        }

        public async Task<ServiceResponse<EventoFecha>> GetEventoFechaPorId(int eventoFechaId)
        {
            ServiceResponse<EventoFecha> response = new();

            var eventoFecha = await _context.EventoFecha
                                .FirstOrDefaultAsync(c => c.EventoFechaId == eventoFechaId);

            if (eventoFecha == null)
            {
                response.Success = false;
                response.Message = "No se encontro la fecha para el evento.";
                return response;
            }

            response.Data = eventoFecha;
            response.Message = "Fecha encontrada";
            return response;
        }

        public Task<ServiceResponse<List<EventoFecha>>> GetEventoFechas()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<EventoFechaPaginadoDto>> GetEventoFechasPorEvento(int pagina, int eventoId)
        {
            ServiceResponse<EventoFechaPaginadoDto> response = new();

            var resultadosPorPagina = 6f;
            var registrosTotales = _context.EventoFecha.Count();
            var cantidadPaginas = Math.Ceiling(registrosTotales / resultadosPorPagina);

            if (registrosTotales > 0)
            {
                List<EventoFecha> eventoFechas = new();

                if (eventoId > 0)
                {
                    eventoFechas = await _context.EventoFecha
                            //.Include(p => p.Cliente)
                            .Where(p => p.EventoId == eventoId && p.FlagEliminado == false)
                            .OrderBy(p => p.EventoFechaId)
                            .Skip((pagina - 1) * (int)resultadosPorPagina)
                            .Take((int)resultadosPorPagina)
                            .ToListAsync();

                    registrosTotales = _context.EventoFecha.Where(p => p.EventoId == eventoId && p.FlagEliminado == false).Count();
                    cantidadPaginas = Math.Ceiling(registrosTotales / resultadosPorPagina);
                }
                else
                {
                    eventoFechas = await _context.EventoFecha
                            //.Include(p => p.Cliente)
                            .Where(p => p.FlagEliminado == false)
                            .OrderBy(p => p.EventoFechaId)
                            .Skip((pagina - 1) * (int)resultadosPorPagina)
                            .Take((int)resultadosPorPagina)
                            .ToListAsync();

                    registrosTotales = eventoFechas.Count;
                    cantidadPaginas = Math.Ceiling(registrosTotales / resultadosPorPagina);
                }

                if (eventoFechas == null)
                {
                    response.Success = false;
                    response.Message = "No se encontraron fechas registrados para el evento.";
                    return response;
                }
                else
                {
                    EventoFechaPaginadoDto eventoFechaPaginadoDto = new()
                    {
                        EventoFechas = eventoFechas,
                        PaginaActual = pagina,
                        Paginas = (int)cantidadPaginas,
                        RegistrosTotales = registrosTotales
                    };

                    response.Success = true;
                    response.Data = eventoFechaPaginadoDto;
                    response.Message = $"{eventoFechas.Count} fechas registradas.";
                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "No se encontraron fechas registradas para el evento.";
                return response;
            }
        }

        public async Task<ServiceResponse<int>> UpdateEventoFechas(EventoFecha eventoFecha)
        {
            var dbEventoFecha = await _context.EventoFecha
                                    .FirstOrDefaultAsync(c => c.EventoFechaId == eventoFecha.EventoFechaId);

            if (dbEventoFecha == null)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Fecha no encontrado."
                };
            }

            dbEventoFecha.Fecha = eventoFecha.Fecha;

            var result = _context.EventoFecha.Update(dbEventoFecha);
            var dbResult = await _context.SaveChangesAsync();

            return new ServiceResponse<int>
            {
                Data = dbResult,
                Message = "Fecha actualizado exitosamente."
            };
        }


    }
}