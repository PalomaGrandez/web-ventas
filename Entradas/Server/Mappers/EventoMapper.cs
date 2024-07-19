using Entradas.Shared.Models;

namespace Entradas.Server.Mappers
{
    public static class EventoMapper
    {
        public static Evento ToEntity(EventoRegistroDto dto) => new()
        {
            EventoId = dto.EventoId,
            Nombre = dto.Nombre,
            Informacion = dto.Informacion,
            Ubicacion = dto.Ubicacion,
            Imagen = dto.Imagen,
            CategoriaId = dto.CategoriaId,
            CapacidadTotal = dto.CapacidadTotal
        };
    }
}
