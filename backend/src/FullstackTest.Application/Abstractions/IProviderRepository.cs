using FullstackTest.Domain.Entities;

namespace FullstackTest.Application.Abstractions;

public interface IProviderRepository
{
    Task<Provider?> GetByIdAsync(Guid id, bool includeInactive = false, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Provider> Items, int TotalCount)> GetPagedAsync(
        string? search,
        string? sortBy,
        bool sortDescending,
        int page,
        int pageSize,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Provider provider, CancellationToken cancellationToken = default);

    Task UpdateAsync(Provider provider, CancellationToken cancellationToken = default);

    Task<int> CountAsync(bool activeOnly = false, CancellationToken cancellationToken = default);
}
