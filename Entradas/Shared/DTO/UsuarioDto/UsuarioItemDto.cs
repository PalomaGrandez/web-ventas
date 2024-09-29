namespace Entradas.Shared.DTO.UsuarioDto
{
    public class UsuarioItemDto
    {
        public int UsuarioId { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string TipoDocuemnto { get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }
    }
}
