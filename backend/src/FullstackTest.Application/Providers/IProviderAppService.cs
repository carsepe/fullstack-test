using FullstackTest.Application.Common;
using FullstackTest.Application.Providers.Dtos;

namespace FullstackTest.Application.Providers;

public interface IProviderAppService
{
    Task<PagedResult<ProviderDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);

    Task<ProviderDto> GetByIdAsync(Guid id, bool includeInactive = false, CancellationToken cancellationToken = default);

    Task<ProviderDto> CreateAsync(CreateProviderRequest request, CancellationToken cancellationToken = default);

    Task<ProviderDto> UpdateAsync(Guid id, UpdateProviderRequest request, CancellationToken cancellationToken = default);

    Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default);

    Task ActivateAsync(Guid id, CancellationToken cancellationToken = default);
}
