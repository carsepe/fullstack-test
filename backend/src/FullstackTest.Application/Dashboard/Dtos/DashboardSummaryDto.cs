namespace FullstackTest.Application.Dashboard.Dtos;

public record DashboardSummaryDto(
    int TotalProviders,
    int TotalProviderServices,
    decimal? AverageHourlyRateUsd);
