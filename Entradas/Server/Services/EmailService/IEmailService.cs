namespace Entradas.Server.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailRegistroUsuarioAsync(EmailRequestDto dto);
        Task SendEmailRegistroPedidoAsync(int ordenId, int usuarioId);
        Task SendEmailGenerarTicketsAsync(EmailRequestDto dto,int ordenId);
    }
}

