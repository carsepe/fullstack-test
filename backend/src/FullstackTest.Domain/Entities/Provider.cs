using System.Net.Mail;
using FullstackTest.Domain.Common;
using FullstackTest.Domain.Constants;

namespace FullstackTest.Domain.Entities;

public class Provider : AuditableEntity
{
    private readonly List<ProviderService> _services = [];

    private Provider()
    {
    }

    public Provider(string nit, string name, string website, string email, string createdBy)
    {
        Nit = RequiredText(nit, nameof(nit), FieldLengths.Nit);
        Name = RequiredText(name, nameof(name), FieldLengths.Name);
        Website = ValidWebsite(website);
        Email = ValidEmail(email);
        SetCreatedAudit(createdBy);
    }

    public string Nit { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Website { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public IReadOnlyCollection<ProviderService> Services => _services.AsReadOnly();

    public void Update(string nit, string name, string website, string email, string updatedBy)
    {
        Nit = RequiredText(nit, nameof(nit), FieldLengths.Nit);
        Name = RequiredText(name, nameof(name), FieldLengths.Name);
        Website = ValidWebsite(website);
        Email = ValidEmail(email);
        SetUpdatedAudit(updatedBy);
    }

    public ProviderService AddService(string name, decimal hourlyRateUsd, string createdBy)
    {
        var service = ProviderService.Create(Id, name, hourlyRateUsd, createdBy);
        _services.Add(service);

        return service;
    }

    private static string ValidEmail(string email)
    {
        var value = RequiredText(email, nameof(email), FieldLengths.Email);

        try
        {
            var address = new MailAddress(value);
            return address.Address;
        }
        catch (FormatException exception)
        {
            throw new ArgumentException("El correo electrónico no tiene un formato válido.", nameof(email), exception);
        }
    }

    private static string ValidWebsite(string website)
    {
        var value = RequiredText(website, nameof(website), FieldLengths.Website);

        if (!value.Contains("://", StringComparison.Ordinal))
        {
            value = $"https://{value}";
        }

        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
            string.IsNullOrWhiteSpace(uri.Host))
        {
            throw new ArgumentException("El sitio web no tiene un formato válido.");
        }

        return value;
    }
}
