using FullstackTest.Application.Dashboard.Dtos;

namespace FullstackTest.Application.Dashboard;

public interface IDashboardAppService
{
    Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken cancellationToken = default);
}
