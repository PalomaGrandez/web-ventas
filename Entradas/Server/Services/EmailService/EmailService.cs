using System.Net.Mail;
using System.Net;
using System.Text;

namespace Entradas.Server.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IOrdenService _ordenService;

        public EmailService(IConfiguration configuration, IAuthService authService, IOrdenService ordenService)
        {
            _configuration = configuration;
            _authService = authService;
            _ordenService = ordenService;
        }

        //public async Task SendEmailRegistroPedidoAsync(EmailRequestDto dto, int ordenId, int usuarioId)
        public async Task SendEmailRegistroPedidoAsync(int ordenId, int usuarioId)
        {
            //Parametros de Email
            var smtpSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = smtpSettings["SmtpServer"];
            var smtpPort = int.Parse(smtpSettings["SmtpPort"]);
            var smtpUsername = smtpSettings["SmtpUsername"];
            var smtpPassword = smtpSettings["SmtpPassword"];
            var senderEmail = smtpSettings["SenderEmail"];


            var usuarioResponse = await _authService.GetUsuarioById(usuarioId!);
            var usuario = usuarioResponse.Data;

            var ordenResponse = await _ordenService.GetOrdenPorOrdenIdPorUsuarioId(ordenId, (int)usuarioId);
            var orden = ordenResponse.Data;

            EmailRequestDto emailRequestDto = new()
            {
                Para = usuario!.Email,
                Asunto = "Pedido Registrado"
            };

            using (var client = new SmtpClient(smtpServer))
            {
                client.Port = smtpPort;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                var message = new MailMessage(senderEmail,
                                            emailRequestDto.Para,
                                            emailRequestDto.Asunto,
                                            BuildEmailBodyRegistroPedido(orden!))
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message);
            }
        }

        public async Task SendEmailRegistroUsuarioAsync(EmailRequestDto dto)
        {
            var smtpSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = smtpSettings["SmtpServer"];
            var smtpPort = int.Parse(smtpSettings["SmtpPort"]);
            var smtpUsername = smtpSettings["SmtpUsername"];
            var smtpPassword = smtpSettings["SmtpPassword"];
            var senderEmail = smtpSettings["SenderEmail"];

            using (var client = new SmtpClient(smtpServer))
            {
                client.Port = smtpPort;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                var message = new MailMessage(senderEmail, dto.Para, dto.Asunto, BuildEmailBodyRegistroUsuario(dto.Para))
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message);
            }
        }
        private string BuildEmailBodyRegistroUsuario(string usuario)
        {
            var body = @"
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Agradecimiento por Registro</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        margin: 0;
                        padding: 0;
                        background-color: #f5f5f5;
                    }
                    .container {
                        max-width: 600px;
                        margin: 50px auto;
                        padding: 20px;
                        background-color: #fff;
                        border-radius: 5px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }
                    h1 {
                        text-align: center;
                        color: #333;
                    }
                    p {
                        text-align: center;
                        color: #666;
                    }
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>¡Gracias por Registrarte!</h1>
                    <p>¡Hola " + usuario + @"!</p>
                    <p>Gracias por registrarte en nuestra página web. Estamos encantados de tenerte como parte de nuestra comunidad.</p>
                    <p>¡Esperamos que disfrutes de todos los beneficios que nuestra página tiene para ofrecer!</p>
                    <p>Atentamente,<br>El Equipo de Fabrica de Entradas</p>
                </div>
            </body>
            </html>
        ";

            return body.ToString();
        }

        private string BuildEmailBodyRegistroPedido(List<VwOrden> orden)
        {
            var body = new StringBuilder();

            body.AppendLine("<!DOCTYPE html>");
            body.AppendLine("<html lang=\"es\">");
            body.AppendLine("<head>");
            body.AppendLine("    <meta charset=\"UTF-8\">");
            body.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            body.AppendLine("    <title>Resumen de Compra</title>");
            body.AppendLine("    <style>");

            body.AppendLine("body {");
            body.AppendLine("    font-family: 'Arial', sans-serif;");
            body.AppendLine(" margin: 20px;");
            body.AppendLine(" padding: 20px;");
            body.AppendLine("   background - color: #f4f4f4;");
            body.AppendLine(" color: #333;");
            body.AppendLine("  }");
            body.AppendLine("  h1 {");
            body.AppendLine("color: #007bff;");
            body.AppendLine(" }");
            body.AppendLine("      table {");
            body.AppendLine("      width: 100 %;");
            body.AppendLine("       border - collapse: collapse;");
            body.AppendLine("    margin - top: 20px;");
            body.AppendLine("    }");
            body.AppendLine("  th, td {");
            body.AppendLine("border: 1px solid #ddd;");
            body.AppendLine("padding: 12px;");
            body.AppendLine("text - align: left;");
            body.AppendLine("}");
            body.AppendLine("th {");
            body.AppendLine("background-color: #007bff;");
            body.AppendLine("color: white;");
            body.AppendLine("}");
            body.AppendLine(".total {");
            body.AppendLine("font - weight: bold;");
            body.AppendLine("font - size: 1.2em;");
            body.AppendLine("}");

            body.AppendLine("    </style>");
            body.AppendLine("</head>");
            body.AppendLine("<body style=\"display: flex; align-items: center; justify-content: center;\">");
            body.AppendLine("    <div style=\"width:800px\">"); // Contenedor para limitar el ancho máximo
            body.AppendLine("        <h1 style=\"text-align: center;\">Resumen de Compra</h1>");

            // Establecer el ancho máximo de la tabla a 800 píxeles
            body.AppendLine("        <table style=\"width: 100%;\">");

            body.AppendLine("        <thead>");
            body.AppendLine("            <tr>");
            body.AppendLine("                <th>Evento</th>");
            body.AppendLine("                <th>Tipo Entrada</th>");
            body.AppendLine("                <th>Fecha</th>");
            body.AppendLine("                <th>Cantidad</th>");
            body.AppendLine("                <th>Precio Unitario</th>");
            body.AppendLine("                <th>Total</th>");

            body.AppendLine("            </tr>");
            body.AppendLine("        </thead>");
            body.AppendLine("        <tbody>");

            foreach (var ordenDetalle in orden)
            {
                body.AppendLine("            <tr>");
                body.AppendLine($"                <td>{ordenDetalle.Evento}</td>");
                body.AppendLine($"                <td>{ordenDetalle.TipoEntrada}</td>");
                body.AppendLine($"                <td>{((DateTime)ordenDetalle.Fecha!).ToShortDateString()}</td>");
                body.AppendLine($"                <td>{ordenDetalle.Cantidad:F2}</td>");
                body.AppendLine($"                <td>S/.{ordenDetalle.PrecioUnitario:F2}</td>");
                body.AppendLine($"                <td>S/.{ordenDetalle.PrecioTotal:F2}</td>");
                body.AppendLine("            </tr>");
            }

            body.AppendLine("            <tr>");
            body.AppendLine($"                <td colspan=5 align=\"right\"> <h3>Total de la Compra:  </h3></td>");
            body.AppendLine($"                <td><h3>  S/.{orden.Sum(x => x.PrecioTotal)} </h3></td>");
            body.AppendLine("            </tr>");

            body.AppendLine("        </tbody>");
            body.AppendLine("    </table>");
            body.AppendLine("    <p>Detalles de la Compra:</p>");
            body.AppendLine($"    <ul>");
            //body.AppendLine($"        <li><strong>Código de Compra:</strong> {o}</li>");
            body.AppendLine($"        <li><strong>Fecha:</strong> {DateTime.Now.ToString("dd/MM/yyyy")}</li>");
            body.AppendLine($"    </ul>");
            body.AppendLine("    <p>¡Gracias por elegirnos!</p>");
            body.AppendLine("    </div>");
            body.AppendLine("</body>");
            body.AppendLine("</html>");

            return body.ToString();
        }

        private string BuildEmailBodyTicketsGenerados(string usuario, int ordenId)
        {
            var body = @"
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Tickets Generados</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        margin: 0;
                        padding: 0;
                        background-color: #f5f5f5;
                    }
                    .container {
                        max-width: 600px;
                        margin: 50px auto;
                        padding: 20px;
                        background-color: #fff;
                        border-radius: 5px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }
                    h1 {
                        text-align: center;
                        color: #333;
                    }
                    p {
                        text-align: center;
                        color: #666;
                    }
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>¡Tickets Generados!</h1>
                    <p>¡Hola " + usuario + @"!</p>
                    <p>Tus Tickets para la orden " + ordenId + @" han sido generados.</p>
                    <p>Ya puedes ingresar a tu panel de usuario para descargarlos.</p>
                    <p>No olvides nominar tus tickets para mayor seguridad.</p>
                    <p>Atentamente,<br>El Equipo de Fabrica de Entradas</p>
                </div>
            </body>
            </html>
        ";

            return body.ToString();
        }
        public async Task SendEmailGenerarTicketsAsync(EmailRequestDto dto, int ordenId)
        {
            var smtpSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = smtpSettings["SmtpServer"];
            var smtpPort = int.Parse(smtpSettings["SmtpPort"]);
            var smtpUsername = smtpSettings["SmtpUsername"];
            var smtpPassword = smtpSettings["SmtpPassword"];
            var senderEmail = smtpSettings["SenderEmail"];

            using (var client = new SmtpClient(smtpServer))
            {
                client.Port = smtpPort;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                var message = new MailMessage(senderEmail, dto.Para, dto.Asunto, BuildEmailBodyTicketsGenerados(dto.Para, ordenId))
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message);
            }
        }
    }
}
