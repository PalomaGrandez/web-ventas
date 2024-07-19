using System.ComponentModel.DataAnnotations;

namespace Entradas.Shared.DTO.OrdenDto
{
    public class OrdenTicketActualizarDto
    {
        public int OrdenTicketId { get; set; }
        public int OrdenId { get; set; }
        public string Evento { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime? Fecha { get; set; }

        [Required(ErrorMessage = "El campo nombres es requerido.")]
        [StringLength(100,MinimumLength =3,ErrorMessage ="El nombre debe ser de 3 caracteres como minimo")]
        public string Nombres { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 3, ErrorMessage = "El apellido paterno debe ser de 3 caracteres como minimo")]
        [Required(ErrorMessage = "El campo apellido paterno es requerido.")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 3, ErrorMessage = "El apellido materno debe ser de 3 caracteres como minimo")]
        [Required(ErrorMessage = "El campo apellido materno es requerido.")]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo tipo de documento es requerido.")]
        public string TipoDocumento { get; set; } = string.Empty;

        [StringLength(20, MinimumLength = 8, ErrorMessage = "El numero de documento debe ser de 8 caracteres como minimo")]
        [Required(ErrorMessage = "El campo numero de documento es requerido.")]
        public string NumeroDocumento { get; set; } = string.Empty;
        public bool FlagNominado { get; set; } = false;
    }
}
