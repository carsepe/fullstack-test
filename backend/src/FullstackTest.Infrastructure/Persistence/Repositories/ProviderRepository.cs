using FullstackTest.Application.Abstractions;
using FullstackTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FullstackTest.Infrastructure.Persistence.Repositories;

public class ProviderRepository(AppDbContext context) : IProviderRepository
{
    public async Task<Provider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Providers
            .FirstOrDefaultAsync(provider => provider.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Provider> Items, int TotalCount)> GetPagedAsync(
        string? search,
        string? sortBy,
        bool sortDescending,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = context.Providers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(provider =>
                provider.Nit.Contains(term) ||
                provider.Name.Contains(term) ||
                provider.Email.Contains(term) ||
                provider.Website.Contains(term));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = ApplySorting(query, sortBy, sortDescending);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(Provider provider, CancellationToken cancellationToken = default)
    {
        await context.Providers.AddAsync(provider, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Provider provider, CancellationToken cancellationToken = default)
    {
        context.Providers.Update(provider);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await context.Providers.CountAsync(cancellationToken);
    }

    private static IQueryable<Provider> ApplySorting(IQueryable<Provider> query, string? sortBy, bool sortDescending)
    {
        return (sortBy?.ToLowerInvariant()) switch
        {
            "nit" => sortDescending ? query.OrderByDescending(provider => provider.Nit) : query.OrderBy(provider => provider.Nit),
            "email" => sortDescending ? query.OrderByDescending(provider => provider.Email) : query.OrderBy(provider => provider.Email),
            "website" => sortDescending ? query.OrderByDescending(provider => provider.Website) : query.OrderBy(provider => provider.Website),
            "createdatutc" => sortDescending ? query.OrderByDescending(provider => provider.CreatedAtUtc) : query.OrderBy(provider => provider.CreatedAtUtc),
            _ => sortDescending ? query.OrderByDescending(provider => provider.Name) : query.OrderBy(provider => provider.Name),
        };
    }
}
