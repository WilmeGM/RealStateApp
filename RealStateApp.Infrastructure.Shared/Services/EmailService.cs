using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Options;
using RealStateApp.Core.Application.Dtos.Email;
using RealStateApp.Core.Domain.Settings;
using RealStateApp.Core.Application.Interfaces.Services;

namespace RealStateApp.Infrastructure.Shared.Services
{
    public class EmailService(IOptions<MailSettings> mailSettings) : IEmailService
    {
        // Configuración de correo electrónico obtenida desde IOptions
        private readonly MailSettings _mailSettings = mailSettings.Value;

        // Método asincrónico para enviar correos
        public async Task SendAsync(EmailRequest request)
        {
            // Crea un nuevo mensaje de correo electrónico
            var email = new MimeMessage();

            // Configura el remitente del correo (desde MailSettings)
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.EmailFrom));

            // Configura el destinatario del correo (desde EmailRequest)
            email.To.Add(MailboxAddress.Parse(request.To));

            // Configura el asunto del correo
            email.Subject = request.Subject;

            // Define el cuerpo del correo en formato HTML
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            // Usa un cliente SMTP para enviar el correo
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            // Desactiva la validación del certificado del servidor (esto debe ser revisado para producción)
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            // Conecta al servidor SMTP usando el host y puerto especificados en MailSettings
            await smtp.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);

            // Autentica el cliente SMTP con el usuario y contraseña especificados en MailSettings
            await smtp.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);

            // Envía el correo electrónico
            await smtp.SendAsync(email);

            // Desconecta el cliente SMTP (cierra la conexión)
            await smtp.DisconnectAsync(true);
        }
    }
}
