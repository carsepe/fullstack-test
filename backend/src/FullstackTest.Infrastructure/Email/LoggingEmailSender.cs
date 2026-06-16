using FullstackTest.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FullstackTest.Infrastructure.Email;

public class LoggingEmailSender(IConfiguration configuration, ILogger<LoggingEmailSender> logger) : IEmailSender
{
    public Task SendAsync(string subject, string body, CancellationToken cancellationToken = default)
    {
        var recipient = configuration["EmailSettings:Recipient"] ?? "notifications@tekus.co";
        logger.LogInformation("Correo enviado a {Recipient}. Asunto: {Subject}. Cuerpo: {Body}", recipient, subject, body);
        return Task.CompletedTask;
    }
}
