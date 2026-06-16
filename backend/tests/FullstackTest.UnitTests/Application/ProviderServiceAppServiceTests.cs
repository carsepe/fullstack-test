using FullstackTest.Application.Abstractions;
using FullstackTest.Application.Common;
using FullstackTest.Application.ProviderServices;
using FullstackTest.Application.ProviderServices.Dtos;
using FullstackTest.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace FullstackTest.UnitTests.Application;

public class ProviderServiceAppServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldThrowNotFound_WhenProviderDoesNotExist()
    {
        var service = new ProviderServiceAppService(
            new MockProviderServiceRepository(),
            new MockProviderRepository(),
            new MockEmailSender(),
            new TestCurrentUserService(),
            NullLogger<ProviderServiceAppService>.Instance);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.CreateAsync(new CreateProviderServiceRequest(Guid.NewGuid(), "Servicio", 50m)));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateService_WhenProviderExists()
    {
        var provider = new Provider("900111222-3", "Proveedor Test", "https://test.co", "test@test.co", "system");
        var service = new ProviderServiceAppService(
            new MockProviderServiceRepository(),
            new MockProviderRepository
            {
                Provider = provider
            },
            new MockEmailSender(),
            new TestCurrentUserService(),
            NullLogger<ProviderServiceAppService>.Instance);

        var result = await service.CreateAsync(new CreateProviderServiceRequest(provider.Id, "Servicio", 50m));

        Assert.Equal("Servicio", result.Name);
        Assert.Equal(50m, result.HourlyRateUsd);
    }

    private sealed class TestCurrentUserService : ICurrentUserService
    {
        public string Email => "admin@fullstack.test";
    }

    private sealed class MockEmailSender : IEmailSender
    {
        public Task SendAsync(string subject, string body, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class MockProviderRepository : IProviderRepository
    {
        public Provider? Provider { get; init; }

        public Task<Provider?> GetByIdAsync(Guid id, bool includeInactive = false, CancellationToken cancellationToken = default)
        {
            if (Provider is null || Provider.Id != id)
            {
                return Task.FromResult<Provider?>(null);
            }

            if (!includeInactive && !Provider.IsActive)
            {
                return Task.FromResult<Provider?>(null);
            }

            return Task.FromResult<Provider?>(Provider);
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
            return Task.FromResult<(IReadOnlyList<Provider>, int)>(([], 0));
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Provider is not null && Provider.Id == id);
        }

        public Task AddAsync(Provider provider, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task UpdateAsync(Provider provider, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task<int> CountAsync(bool activeOnly = false, CancellationToken cancellationToken = default) => Task.FromResult(0);
    }

    private sealed class MockProviderServiceRepository : IProviderServiceRepository
    {
        public Task<ProviderService?> GetByIdAsync(Guid id, bool includeInactive = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<ProviderService?>(null);
        }

        public Task<(IReadOnlyList<ProviderService> Items, int TotalCount)> GetPagedAsync(
            string? search,
            Guid? providerId,
            string? sortBy,
            bool sortDescending,
            int page,
            int pageSize,
            bool includeInactive = false,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<(IReadOnlyList<ProviderService>, int)>(([], 0));
        }

        public Task AddAsync(ProviderService service, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task UpdateAsync(ProviderService service, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task<int> CountAsync(bool activeOnly = false, CancellationToken cancellationToken = default) => Task.FromResult(0);

        public Task<decimal?> GetAverageHourlyRateAsync(bool activeOnly = true, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<decimal?>(null);
        }
    }
}
