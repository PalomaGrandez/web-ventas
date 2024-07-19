using Entradas.Shared.Models;

namespace Entradas.Shared.DTO.EventoDto
{
    public class EventoPaginadoDto
    {
        public List<Evento> Eventos { get; set; } = new();
        public int Paginas { get; set; }
        public int PaginaActual { get; set; }
        public int RegistrosTotales { get; set; }
    }
}
