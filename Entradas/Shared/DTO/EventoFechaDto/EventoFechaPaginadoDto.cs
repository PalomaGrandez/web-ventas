using Entradas.Shared.Models;

namespace Entradas.Shared.DTO.EventoFechaDto
{
    public class EventoFechaPaginadoDto
    {
        public List<EventoFecha> EventoFechas { get; set; } = new();
        public int Paginas { get; set; }
        public int PaginaActual { get; set; }
        public int RegistrosTotales { get; set; }
    }
}
