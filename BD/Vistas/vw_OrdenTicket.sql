create or alter view Entradas.vw_OrdenTicket
as
SELECT ot.[OrdenTicketId]
      ,ot.[OrdenId]
	  ,e.Nombre as Evento
	  ,ee.Tipo
	  ,ef.Fecha
      ,ot.[Nombres]
      ,ot.[ApellidoPaterno]
      ,ot.[ApellidoMaterno]
      ,ot.[TipoDocumento]
      ,ot.[NumeroDocumento]
      ,ot.[FlagNominado]
  FROM Entradas.OrdenTicket ot inner join 
  Entradas.Evento e on e.EventoId =ot.EventoId inner join 
  Entradas.EventoEntrada ee on ot.EventoEntradaId=ee.EventoEntradaId inner join 
  Entradas.EventoFecha ef on ot.EventoFechaId=ef.EventoFechaId

  --GO
  --SELECT * 
  --FROM Entradas.OrdenTicket
  --WHERE OrdenId=1

  --select * from Entradas.OrdenDetalle