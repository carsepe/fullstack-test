using FullstackTest.Application.Abstractions;
using FullstackTest.Application.Common;
using FullstackTest.Application.Providers;
using FullstackTest.Application.Providers.Dtos;
using FullstackTest.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace FullstackTest.UnitTests.Application;

public class ProviderAppServiceTests
{
    private readonly MockProviderRepository _repository = new();
    private readonly ProviderAppService _service;

    public ProviderAppServiceTests()
    {
        _service = new ProviderAppService(_repository, new TestCurrentUserService());
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistProvider_WhenDataIsValid()
    {
        var result = await _service.CreateAsync(
            new CreateProviderRequest("900111222-3", "Proveedor Test", "https://test.co", "test@test.co"));

        Assert.Equal("900111222-3", result.Nit);
        Assert.True(result.IsActive);
        Assert.Single(_repository.Providers);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFound_WhenProviderDoesNotExist()
    {
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.UpdateAsync(
                Guid.NewGuid(),
                new UpdateProviderRequest("900111222-3", "Proveedor Test", "https://test.co", "test@test.co")));
    }

    [Fact]
    public async Task DeactivateAsync_ShouldSetProviderInactive()
    {
        var provider = new Provider("900111222-3", "Proveedor Test", "https://test.co", "test@test.co", "system");
        _repository.Providers.Add(provider);

        await _service.DeactivateAsync(provider.Id);

        Assert.False(_repository.Providers[0].IsActive);
    }

    private sealed class TestCurrentUserService : ICurrentUserService
    {
        public string Email => "admin@fullstack.test";
    }

    private sealed class MockProviderRepository : IProviderRepository
    {
        public List<Provider> Providers { get; } = [];

        public Task<Provider?> GetByIdAsync(Guid id, bool includeInactive = false, CancellationToken cancellationToken = default)
        {
            var provider = Providers.FirstOrDefault(item => item.Id == id);
            if (provider is null || (!includeInactive && !provider.IsActive))
            {
                return Task.FromResult<Provider?>(null);
            }

            return Task.FromResult<Provider?>(provider);
        }

        public Task<(IReadOnlyList<Provider> Items, int TotalCount)> GetPagedAsync(
            string? search,
            string? sortBy,
            bool sortDescending,
            int page,
            int pageSize,
            bool includeInactive = false,
            CancellationToken cancellationToken = default)
        {
            var items = Providers.Where(provider => includeInactive || provider.IsActive).ToList();
            return Task.FromResult<(IReadOnlyList<Provider>, int)>((items, items.Count));
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Providers.Any(provider => provider.Id == id));
        }

        public Task AddAsync(Provider provider, CancellationToken cancellationToken = default)
        {
            Providers.Add(provider);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Provider provider, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<int> CountAsync(bool activeOnly = false, CancellationToken cancellationToken = default)
        {
            var count = activeOnly ? Providers.Count(provider => provider.IsActive) : Providers.Count;
            return Task.FromResult(count);
        }
    }
}
