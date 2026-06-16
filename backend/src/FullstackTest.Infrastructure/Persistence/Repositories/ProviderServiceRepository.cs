using FullstackTest.Application.Abstractions;
using FullstackTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FullstackTest.Infrastructure.Persistence.Repositories;

public class ProviderServiceRepository(AppDbContext context) : IProviderServiceRepository
{
    public async Task<ProviderService?> GetByIdAsync(Guid id, bool includeInactive = false, CancellationToken cancellationToken = default)
    {
        var query = context.ProviderServices.AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(service => service.IsActive);
        }

        return await query.FirstOrDefaultAsync(service => service.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<ProviderService> Items, int TotalCount)> GetPagedAsync(
        string? search,
        Guid? providerId,
        string? sortBy,
        bool sortDescending,
        int page,
        int pageSize,
        bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var query = context.ProviderServices.AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(service => service.IsActive);
        }

        if (providerId.HasValue)
        {
            query = query.Where(service => service.ProviderId == providerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(service => service.Name.Contains(term));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = ApplySorting(query, sortBy, sortDescending);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(ProviderService service, CancellationToken cancellationToken = default)
    {
        await context.ProviderServices.AddAsync(service, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ProviderService service, CancellationToken cancellationToken = default)
    {
        context.ProviderServices.Update(service);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(bool activeOnly = false, CancellationToken cancellationToken = default)
    {
        var query = context.ProviderServices.AsQueryable();

        if (activeOnly)
        {
            query = query.Where(service => service.IsActive);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<decimal?> GetAverageHourlyRateAsync(bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var query = context.ProviderServices.AsQueryable();

        if (activeOnly)
        {
            query = query.Where(service => service.IsActive);
        }

        if (!await query.AnyAsync(cancellationToken))
        {
            return null;
        }

        return await query.AverageAsync(service => service.HourlyRateUsd, cancellationToken);
    }

    private static IQueryable<ProviderService> ApplySorting(IQueryable<ProviderService> query, string? sortBy, bool sortDescending)
    {
        return (sortBy?.ToLowerInvariant()) switch
        {
            "hourlyrateusd" => sortDescending
                ? query.OrderByDescending(service => service.HourlyRateUsd)
                : query.OrderBy(service => service.HourlyRateUsd),
            "providerid" => sortDescending
                ? query.OrderByDescending(service => service.ProviderId)
                : query.OrderBy(service => service.ProviderId),
            "createdatutc" => sortDescending
                ? query.OrderByDescending(service => service.CreatedAtUtc)
                : query.OrderBy(service => service.CreatedAtUtc),
            _ => sortDescending
                ? query.OrderByDescending(service => service.Name)
                : query.OrderBy(service => service.Name),
        };
    }
}
