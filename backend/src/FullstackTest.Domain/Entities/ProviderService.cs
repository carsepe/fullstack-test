using FullstackTest.Domain.Common;
using FullstackTest.Domain.Constants;

namespace FullstackTest.Domain.Entities;

public class ProviderService : AuditableEntity
{
    private ProviderService()
    {
    }

    internal ProviderService(Guid providerId, string name, decimal hourlyRateUsd, string createdBy)
    {
        if (providerId == Guid.Empty)
        {
            throw new ArgumentException("El identificador del proveedor es obligatorio.", nameof(providerId));
        }

        ProviderId = providerId;
        Name = RequiredText(name, nameof(name), FieldLengths.Name);
        HourlyRateUsd = ValidHourlyRate(hourlyRateUsd);
        SetCreatedAudit(createdBy);
    }

    public Guid ProviderId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal HourlyRateUsd { get; private set; }

    public void Update(string name, decimal hourlyRateUsd, string updatedBy)
    {
        Name = RequiredText(name, nameof(name), FieldLengths.Name);
        HourlyRateUsd = ValidHourlyRate(hourlyRateUsd);
        SetUpdatedAudit(updatedBy);
    }

    private static decimal ValidHourlyRate(decimal hourlyRateUsd)
    {
        if (hourlyRateUsd <= 0)
        {
            throw new ArgumentException("La tarifa por hora debe ser mayor que cero.", nameof(hourlyRateUsd));
        }

        return hourlyRateUsd;
    }
}
