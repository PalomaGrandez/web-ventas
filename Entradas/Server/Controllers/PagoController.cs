using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Threading.Tasks;

namespace Entradas.Server.Controllers
{
    [Route("api/pagos")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        [HttpPost("procesar")]
        public async Task<IActionResult> ProcesarPago([FromBody] TokenRequest request)
        {
            // Log para verificar que el token se recibe correctamente
            Console.WriteLine($"Token recibido: {request.TokenId}");
        
            // Crear una instancia del cliente RestSharp
            var client = new RestClient("https://api.culqi.com/v2/charges");

            // Crear una solicitud POST
            var restRequest = new RestRequest();
            restRequest.AddHeader("Authorization", "Bearer sk_test_47b1d0312b63a309"); // Usa tu llave secreta de prueba
            restRequest.AddHeader("Content-Type", "application/json");

            // Configurar el método HTTP como POST
            restRequest.Method = Method.Post;

            // Crear el payload usando solo el token recibido
            var payload = new
            {
                amount = request.Amount, // Monto en céntimos, ajusta según sea necesario
                currency_code = "PEN",
                source_id = request.TokenId, // El token recibido del frontend
                capture = true,
                description = "Pago realizado con Yape", // Descripción del pago
                email = "soporte@fabrica.pe",
                    
            };

            // Agregar el cuerpo de la solicitud
            restRequest.AddJsonBody(payload);

            // Enviar la solicitud y obtener la respuesta
            RestResponse response = await client.ExecuteAsync(restRequest);

            // Log para verificar el contenido de la respuesta
            Console.WriteLine($"Respuesta de la API: {response.Content}");

            if (response.IsSuccessful)
            {
                return Ok(new { message = "Pago procesado exitosamente", data = response.Content });
            }
            else
            {
                return BadRequest(new { message = "Error al procesar el pago", error = response.Content });
            }
        }
    }

    public class TokenRequest
    {
        public string TokenId { get; set; }

        public int Amount { get; set; }
    }
}