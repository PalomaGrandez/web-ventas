using Entradas.Server.Mappers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using QRCoder;
using Microsoft.AspNetCore.Mvc;

namespace Entradas.Server.Services.OrdenService
{
    public class OrdenService : IOrdenService
    {
        private readonly DataContext _context;

        public OrdenService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<int>> CreateOrden(OrdenRegistroDto dto)
        {
            var response = new ServiceResponse<int>();

            try
            {
                var items = new List<OrdenDetalle>();

                foreach (var item in dto.Items)
                {
                    var ordenDetalle = new OrdenDetalle
                    {
                        EventoId = (int)item.EventoId!,
                        EventoEntradaId = (int)item.EventoEntradaId!,
                        EventoFechaId = (int)item.EventoFechaId!,
                        Cantidad = (int)item.Cantidad,
                        PrecioUnitario = (decimal)item.PrecioRegular,
                        PrecioTotal = (decimal)item.PrecioTotal,
                        FlagDescuento = false
                    };

                    items.Add(ordenDetalle);
                }

                var orden = OrdenMapper.ToEntity(dto);

                orden.OrdenDetalle = items;

                var result = _context.Orden.Add(orden);
                var dbResult = await _context.SaveChangesAsync();

                response.Data = result.Entity.OrdenId;
                response.Message = result.ToString();
                return response;
            }
            catch (Exception ex)
            {
                response.Data = 0;
                response.Success = false;
                response.Message = ex.ToString();
                return response;
            }
        }
        
        /*
        public async Task<ServiceResponse<FileStreamResult>> GenerarTicketPdf(int ordenTicketId)
        {
            // Generar el código QR
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(ordenTicketId.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);

            // Guardar el código QR como imagen
            string qrImagePath = Path.Combine(Path.GetTempPath(), "codigo_qr.png");
            qrCodeImage.Save(qrImagePath);

            // Crear el PDF
            Document doc = new Document();
            string pdfPath = Path.Combine(Path.GetTempPath(), "ticket.pdf");
            PdfWriter.GetInstance(doc, new FileStream(pdfPath, FileMode.Create));
            doc.Open();

            var ordenTicket = await GetOrdenTicketPorOrdenTicketId(ordenTicketId);

            // Agregar texto al PDF
            Font font = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            Paragraph paragraph = new Paragraph();
            paragraph.Add(new Chunk($"Nombre: {ordenTicket.Data.Nombres}\n", font));
            paragraph.Add(new Chunk($"Apellido Paterno: {ordenTicket.Data.ApellidoPaterno}\n", font));
            paragraph.Add(new Chunk($"Apellido Materno: {ordenTicket.Data.ApellidoMaterno}\n", font));
            paragraph.Add(new Chunk($"Tipo de documento: {ordenTicket.Data.TipoDocumento}\n", font));
            paragraph.Add(new Chunk($"Número de documento: {ordenTicket.Data.NumeroDocumento}\n", font));
            doc.Add(paragraph);

            doc.Close();

            // Eliminar la imagen temporal del código QR
            System.IO.File.Delete(qrImagePath);

            // Insertar el código QR en el PDF
            iTextSharp.text.Image qrImage = iTextSharp.text.Image.GetInstance(qrImagePath);
            qrImage.SetAbsolutePosition(50, 700); // Posición del código QR en el PDF
            qrImage.ScaleAbsolute(200, 200); // Tamaño del código QR en el PDF
            doc.Add(qrImage);

            // Devolver el PDF como un archivo descargable
            byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfPath);
            System.IO.File.Delete(pdfPath); // Eliminar el archivo PDF temporal

                                            // Devolver el PDF como un FileStreamResult
            var stream = new FileStream(pdfPath, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }*/

        public async Task<ServiceResponse<int>> GenerarTickets(int ordenId)
        {
            // Buscar la orden en la base de datos
            var dbOrden = await _context.Orden.FirstOrDefaultAsync(c => c.OrdenId == ordenId);

            // Verificar si la orden no existe
            if (dbOrden == null)
            {
                return new ServiceResponse<int>
                {
                    Data = 0,
                    Message = "No se encontró la orden.",
                    Success = false
                };
            }

            // Verificar si los tickets para esta orden ya han sido generados anteriormente
            if (dbOrden.TicketGenerado)
            {
                return new ServiceResponse<int>
                {
                    Data = 0,
                    Message = $"Los tickets para la orden {ordenId} ya han sido generados anteriormente.",
                    Success = false
                };
            }

            // Obtener los detalles de la orden de la base de datos
            var dbOrdenDetalle = await _context.OrdenDetalle
                                                .Where(x => x.OrdenId == ordenId)
                                                .ToListAsync();

            // Verificar si no se encontraron detalles de la orden
            if (dbOrdenDetalle == null || dbOrdenDetalle.Count == 0)
            {
                return new ServiceResponse<int>
                {
                    Data = 0,
                    Message = "No se encontraron detalles de la orden.",
                    Success = false
                };
            }

            // Lista para almacenar los nuevos tickets
            List<OrdenTicket> tickets = new();

            // Generar tickets basados en los detalles de la orden
            foreach (var detalle in dbOrdenDetalle)
            {
                for (int i = 0; i < detalle.Cantidad; i++)
                {
                    var ticket = new OrdenTicket
                    {
                        OrdenId = detalle.OrdenId,
                        EventoId = detalle.EventoId,
                        EventoEntradaId = detalle.EventoEntradaId,
                        EventoFechaId = detalle.EventoFechaId,
                        Nombres = string.Empty,
                        ApellidoPaterno = string.Empty,
                        ApellidoMaterno = string.Empty,
                        TipoDocumento = string.Empty,
                        NumeroDocumento = string.Empty,
                        FlagNominado = false
                    };

                    // Agregar el nuevo ticket a la lista
                    tickets.Add(ticket);
                }
            }

            // Agregar los tickets a la base de datos
            await _context.OrdenTicket.AddRangeAsync(tickets);

            // Marcar la orden como generada
            dbOrden.TicketGenerado = true;

            // Actualizar la orden en la base de datos
            _context.Orden.Update(dbOrden);

            // Guardar los cambios en la base de datos y obtener el número de registros afectados
            var dbResult = await _context.SaveChangesAsync();

            // Devolver una respuesta de servicio con la cantidad de tickets generados
            return new ServiceResponse<int>
            {
                Data = tickets.Count,
                Message = $"{tickets.Count} tickets generados."
            };
        }


        public async Task<ServiceResponse<OrdenPaginadoDto>> GetOrdenesPaginado(int pagina)
        {
            // Inicializar la respuesta del servicio
            ServiceResponse<OrdenPaginadoDto> response = new();

            try
            {
                // Definir la cantidad de resultados por página
                var resultadosPorPagina = 10f;

                // Obtener el total de registros en la base de datos
                var registrosTotales = await _context.Orden.CountAsync();

                // Calcular la cantidad de páginas necesarias
                var cantidadPaginas = Math.Ceiling(registrosTotales / resultadosPorPagina);

                // Verificar si existen registros en la base de datos
                if (registrosTotales > 0)
                {
                    // Obtener la lista de eventos paginados desde la base de datos
                    var ordenes = await _context.Orden
                        .OrderBy(c => c.OrdenId)
                        .Skip((pagina - 1) * (int)resultadosPorPagina)
                        .Take((int)resultadosPorPagina)
                        .ToListAsync();

                    // Verificar si se encontraron eventos
                    if (ordenes == null || ordenes.Count == 0)
                    {
                        response.Success = false;
                        response.Message = "No se encontraron órdenes registradas.";
                    }
                    else
                    {
                        // Crear el DTO para eventos paginados
                        OrdenPaginadoDto ordenPaginadoDto = new()
                        {
                            Ordenes = ordenes,
                            PaginaActual = pagina,
                            Paginas = (int)cantidadPaginas,
                            RegistrosTotales = registrosTotales
                        };

                        response.Success = true;
                        response.Data = ordenPaginadoDto;
                    }
                }
                else
                {
                    // No se encontraron eventos registrados
                    response.Success = false;
                    response.Message = "No se encontraron órdenes registradas.";
                }
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                response.Success = false;
                response.Message = "Ocurrió un error al obtener las órdenes. Detalles: " + ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<OrdenPaginadoDto>> GetOrdenesPorUsuario(int pagina, int usuarioId)
        {
            // Inicializar la respuesta del servicio
            ServiceResponse<OrdenPaginadoDto> response = new();

            try
            {
                // Definir la cantidad de resultados por página
                var resultadosPorPagina = 10f;

                // Obtener el total de registros en la base de datos
                var registrosTotales = await _context.Orden
                                                .Where(x => x.UsuarioId == usuarioId)
                                                .CountAsync();

                // Calcular la cantidad de páginas necesarias
                var cantidadPaginas = Math.Ceiling(registrosTotales / resultadosPorPagina);

                // Verificar si existen registros en la base de datos
                if (registrosTotales > 0)
                {
                    // Obtener la lista de eventos paginados desde la base de datos
                    var ordenes = await _context.Orden
                        .Where(x => x.UsuarioId == usuarioId)
                        .OrderByDescending(c => c.FechaOrden)
                        .Skip((pagina - 1) * (int)resultadosPorPagina)
                        .Take((int)resultadosPorPagina)
                        .ToListAsync();

                    // Verificar si se encontraron eventos
                    if (ordenes == null || ordenes.Count == 0)
                    {
                        response.Success = true;
                        response.Message = "No se encontraron órdenes registradas.";
                    }
                    else
                    {
                        // Crear el DTO para eventos paginados
                        OrdenPaginadoDto ordenPaginadoDto = new()
                        {
                            Ordenes = ordenes,
                            PaginaActual = pagina,
                            Paginas = (int)cantidadPaginas,
                            RegistrosTotales = registrosTotales
                        };

                        response.Success = true;
                        response.Data = ordenPaginadoDto;
                    }
                }
                else
                {
                    // No se encontraron eventos registrados
                    response.Success = true;
                    response.Message = "No se encontraron órdenes registradas.";
                }
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                response.Success = false;
                response.Message = $"Ocurrió un error al obtener las órdenes. Detalles: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<Orden>> GetOrdenPorId(int ordenId)
        {
            // Inicializar la respuesta del servicio
            ServiceResponse<Orden> response = new();

            try
            {
                // Obtener la orden por su ID desde la base de datos
                var orden = await _context.Orden
                    .FirstOrDefaultAsync(c => c.OrdenId == ordenId);

                // Verificar si la orden no fue encontrada
                if (orden == null)
                {
                    response.Success = false;
                    response.Message = "No se encontró la orden.";
                }
                else
                {
                    // Configurar la respuesta exitosa con la orden encontrada
                    response.Data = orden;
                    response.Message = "Orden encontrada";
                }

                return response;
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                response.Success = false;
                response.Message = $"Error al obtener la orden. Detalles: {ex.Message}";
                return response;
            }
        }

        public async Task<ServiceResponse<List<VwOrden>>> GetOrdenPorOrdenIdPorUsuarioId(int ordenId, int usuarioId)
        {
            var response = new ServiceResponse<List<VwOrden>>();

            try
            {
                var orden = await _context.VwOrden
                                        .Where(x => x.OrdenId == ordenId && x.UsuarioId == usuarioId)
                                        .ToListAsync();

                // Verificar si se encontraron registros
                if (orden.Any())
                {
                    // Si se encontraron registros, establecer el éxito en verdadero y asignar los datos
                    response.Success = true;
                    response.Data = orden;
                }
                else
                {
                    // Si no se encontraron registros, establecer el éxito en falso y asignar un mensaje
                    response.Success = false;
                    response.Message = "No se encontró la orden buscada.";
                }
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados y asignar un mensaje de error
                response.Success = false;
                response.Message = "Ocurrió un error al obtener las órdenes. Detalles: " + ex.Message;
            }

            return response;
        }


        public async Task<ServiceResponse<List<VwOrdenTicket>>> GetOrdenTicketPorOrdenId(int ordenId)
        {
            // Inicializar la respuesta del servicio
            var response = new ServiceResponse<List<VwOrdenTicket>>();

            try
            {
                // Obtener la lista de eventos desde la base de datos
                var ordenTicket = await _context.VwOrdenTicket
                    .Where(x => x.OrdenId == ordenId)
                    .ToListAsync();

                // Verificar si se encontraron registros
                if (ordenTicket.Any())
                {
                    // Si se encontraron registros, establecer el éxito en verdadero y asignar los datos
                    response.Success = true;
                    response.Data = ordenTicket;
                }
                else
                {
                    // Si no se encontraron registros, establecer el éxito en falso y asignar un mensaje
                    response.Success = false;
                    response.Message = "No se encontró la orden buscada.";
                }
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                response.Success = false;
                response.Message = "Ocurrió un error al obtener las órdenes. Detalles: " + ex.Message;
            }

            // Devolver la respuesta del servicio
            return response;
        }

        public async Task<ServiceResponse<VwOrdenTicket>> GetOrdenTicketPorOrdenTicketId(int ordenTicketId)
        {
            // Inicializar la respuesta del servicio
            var response = new ServiceResponse<VwOrdenTicket>();

            try
            {
                // Obtener la lista de eventos desde la base de datos
                var ordenTicket = await _context.VwOrdenTicket
                    .FirstOrDefaultAsync(x => x.OrdenTicketId == ordenTicketId);

                // Verificar si se encontraron registros
                if (ordenTicket != null)
                {
                    // Si se encontraron registros, establecer el éxito en verdadero y asignar los datos
                    response.Success = true;
                    response.Data = ordenTicket;
                }
                else
                {
                    // Si no se encontraron registros, establecer el éxito en falso y asignar un mensaje
                    response.Success = false;
                    response.Message = "No se encontró el ticket buscado.";
                }
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                response.Success = false;
                response.Message = "Ocurrió un error al obtener las órdenes. Detalles: " + ex.Message;
            }

            // Devolver la respuesta del servicio
            return response;
        }

        public async Task<ServiceResponse<int>> TicketNominar(OrdenTicketActualizarDto dto)
        {
            try
            {
                // Buscar la orden en la base de datos por el ID proporcionado
                var dbOrdenTicket = await _context.OrdenTicket
                                                .FirstOrDefaultAsync(c => c.OrdenTicketId == dto.OrdenTicketId);

                // Verificar si la orden no fue encontrada
                if (dbOrdenTicket == null)
                {
                    // Devolver una respuesta de servicio con un mensaje de error
                    return new ServiceResponse<int>
                    {
                        Success = false,
                        Message = "Ticket no encontrado."
                    };
                }

                // Actualizar los campos de la orden con los valores proporcionados en el DTO
                dbOrdenTicket.Nombres = dto.Nombres;
                dbOrdenTicket.ApellidoPaterno = dto.ApellidoPaterno;
                dbOrdenTicket.ApellidoMaterno = dto.ApellidoMaterno;
                dbOrdenTicket.TipoDocumento = dto.TipoDocumento;
                dbOrdenTicket.NumeroDocumento = dto.NumeroDocumento;
                dbOrdenTicket.FlagNominado = true;

                // Marcar la orden como modificada en el contexto de la base de datos
                _context.OrdenTicket.Update(dbOrdenTicket);

                // Guardar los cambios en la base de datos y obtener el número de registros afectados
                var dbResult = await _context.SaveChangesAsync();

                // Devolver una respuesta de servicio exitosa con la cantidad de registros afectados
                return new ServiceResponse<int>
                {
                    Data = dbResult,
                    Success = true,
                    Message = "Ticket nominado exitosamente."
                };
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados y devolver una respuesta con el error
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Ocurrió un error al nominar el ticket. Detalles: " + ex.Message
                };
            }
        }


        // Método asincrónico para actualizar una orden
        public async Task<ServiceResponse<int>> UpdateOrden(OrdenActualizarDto dto)
        {
            // Buscar la orden en la base de datos por el ID proporcionado
            var dbOrden = await _context.Orden
                                        .FirstOrDefaultAsync(c => c.OrdenId == dto.OrdenId);

            // Verificar si la orden no fue encontrada
            if (dbOrden == null)
            {
                // Devolver una respuesta de servicio con un mensaje de error
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Orden no encontrada."
                };
            }

            // Actualizar los campos de la orden con los valores proporcionados en el DTO
            dbOrden.Estado = dto.Estado;
            dbOrden.MedioPago = dto.MedioPago;
            dbOrden.NumeroOperacion = dto.NumeroOperacion;

            // Marcar la orden como modificada en el contexto de la base de datos
            var result = _context.Orden.Update(dbOrden);

            // Guardar los cambios en la base de datos y obtener el número de registros afectados
            var dbResult = await _context.SaveChangesAsync();

            // Devolver una respuesta de servicio exitosa con la cantidad de registros afectados
            return new ServiceResponse<int>
            {
                Data = dbResult,
                Message = "Orden actualizada exitosamente."
            };
        }

    }
}
