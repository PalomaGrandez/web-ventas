using Entradas.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entradas.Shared.DTO.EventoEntradaDto
{
    public class EventoEntradaPaginadoDto
    {
        public List<EventoEntrada> EventoEntradas { get; set; } = new();
        public int Paginas { get; set; }
        public int PaginaActual { get; set; }
        public int RegistrosTotales { get; set; }
    }
}
