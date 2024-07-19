using Entradas.Shared.DTO.OrdenDto;
using Entradas.Shared.Models;

namespace Entradas.Server.Mappers
{
    public static class OrdenMapper
    {
        public static Orden ToEntity(OrdenRegistroDto dto) => new()
        {
            UsuarioId= dto.UsuarioId,
            FechaOrden= dto.FechaOrden,
            PrecioTotal= dto.PrecioTotal,
            Estado= dto.Estado,
            MedioPago=dto.MedioPago,            
        };
    }
}
