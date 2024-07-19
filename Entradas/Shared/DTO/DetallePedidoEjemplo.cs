using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entradas.Shared.DTO
{
    public class DetallePedidoEjemplo
    {
        public DetallePedidoEjemplo(string ticket, decimal precio, int cantidad, decimal precioTotal )
        {
            Ticket = ticket;
            Precio = precio;
            Cantidad = cantidad;
            PrecioTotal = precioTotal;
        }
        public string Ticket { get; set; }= string.Empty;   
        public decimal Precio { get; set; }= decimal.Zero;
        public int Cantidad { get; set; }= 0;
        public decimal PrecioTotal { get; set;}= decimal.Zero;
    }
}
