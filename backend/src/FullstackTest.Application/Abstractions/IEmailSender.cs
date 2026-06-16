namespace FullstackTest.Application.Abstractions;

public interface IEmailSender
{
    Task SendAsync(string subject, string body, CancellationToken cancellationToken = default);
}
