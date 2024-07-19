﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Entradas.Shared.Models
{
    public partial class OrdenDetalle
    {
        public int OrdenId { get; set; }
        public int EventoId { get; set; }
        public int EventoEntradaId { get; set; }
        public int EventoFechaId { get; set; }
        public int? Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? PrecioTotal { get; set; }
        public bool? FlagDescuento { get; set; }

        public virtual Evento Evento { get; set; }
        public virtual EventoEntrada EventoEntrada { get; set; }
        public virtual EventoFecha EventoFecha { get; set; }
        public virtual Orden Orden { get; set; }
    }
}