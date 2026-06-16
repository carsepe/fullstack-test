using FullstackTest.Domain.Entities;

namespace FullstackTest.Application.Abstractions;

public interface IProviderRepository
{
    Task<Provider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Provider> Items, int TotalCount)> GetPagedAsync(
        string? search,
        string? sortBy,
        bool sortDescending,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task AddAsync(Provider provider, CancellationToken cancellationToken = default);

    Task UpdateAsync(Provider provider, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
