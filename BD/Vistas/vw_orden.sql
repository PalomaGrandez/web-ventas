--select * from Entradas.Orden
--select * from entradas.OrdenDetalle
create or alter view vw_Orden
as
select o.OrdenId, 
		o.UsuarioId,
		e.Nombre as Evento,
		ef.Fecha as Fecha,
		od.Cantidad,
		od.PrecioUnitario,
		od.PrecioTotal,
		ee.Tipo as TipoEntrada
from Entradas.Orden O inner join 
	 Entradas.OrdenDetalle OD on O.OrdenId=OD.OrdenId inner join
	 Entradas.Evento E on e.EventoId=od.EventoId inner join 
	 Entradas.EventoEntrada EE on ee.EventoEntradaId=od.EventoEntradaId inner join
	 Entradas.EventoFecha EF on ef.EventoFechaId=od.EventoFechaId
--where od.OrdenId=1 and o.UsuarioId=1

--select * from Entradas.EventoEntrada

--update Entradas.EventoEntrada
--set tipo = UPPER(tipo)
go
select * from vw_Orden