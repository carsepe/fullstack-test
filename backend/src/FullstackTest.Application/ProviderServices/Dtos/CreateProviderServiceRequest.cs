namespace FullstackTest.Application.ProviderServices.Dtos;

public record CreateProviderServiceRequest(Guid ProviderId, string Name, decimal HourlyRateUsd);
