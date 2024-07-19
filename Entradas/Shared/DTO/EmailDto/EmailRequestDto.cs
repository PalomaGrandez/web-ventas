using System.ComponentModel.DataAnnotations;

namespace Entradas.Shared.DTO.EmailDto
{
    public class EmailRequestDto
    {
        [EmailAddress]
        public string Para { get; set; } = string.Empty;
        public string Asunto { get; set; } = string.Empty;
        public string Cuerpo { get; set; } = string.Empty;
    }
}
