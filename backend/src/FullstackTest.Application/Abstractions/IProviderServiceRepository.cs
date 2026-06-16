using FullstackTest.Domain.Entities;

namespace FullstackTest.Application.Abstractions;

public interface IProviderServiceRepository
{
    Task<ProviderService?> GetByIdAsync(Guid id, bool includeInactive = false, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<ProviderService> Items, int TotalCount)> GetPagedAsync(
        string? search,
        Guid? providerId,
        string? sortBy,
        bool sortDescending,
        int page,
        int pageSize,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);

    Task AddAsync(ProviderService service, CancellationToken cancellationToken = default);

    Task UpdateAsync(ProviderService service, CancellationToken cancellationToken = default);

    Task<int> CountAsync(bool activeOnly = false, CancellationToken cancellationToken = default);

    Task<decimal?> GetAverageHourlyRateAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
}
