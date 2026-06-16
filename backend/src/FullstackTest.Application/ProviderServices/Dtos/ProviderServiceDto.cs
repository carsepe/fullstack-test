namespace FullstackTest.Application.ProviderServices.Dtos;

public record ProviderServiceDto(
    Guid Id,
    Guid ProviderId,
    string Name,
    decimal HourlyRateUsd,
    bool IsActive);
