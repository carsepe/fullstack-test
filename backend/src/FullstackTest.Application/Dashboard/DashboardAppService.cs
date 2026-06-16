using FullstackTest.Application.Abstractions;
using FullstackTest.Application.Dashboard.Dtos;

namespace FullstackTest.Application.Dashboard;

public class DashboardAppService(
    IProviderRepository providerRepository,
    IProviderServiceRepository providerServiceRepository) : IDashboardAppService
{
    public async Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var totalProviders = await providerRepository.CountAsync(activeOnly: true, cancellationToken);
        var totalServices = await providerServiceRepository.CountAsync(activeOnly: true, cancellationToken);
        var averageRate = await providerServiceRepository.GetAverageHourlyRateAsync(activeOnly: true, cancellationToken);

        return new DashboardSummaryDto(totalProviders, totalServices, averageRate);
    }
}
