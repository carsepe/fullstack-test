using FullstackTest.Application.Abstractions;
using FullstackTest.Application.Common;
using FullstackTest.Application.ProviderServices.Dtos;
using FullstackTest.Domain.Entities;

namespace FullstackTest.Application.ProviderServices;

public interface IProviderServiceAppService
{
    Task<PagedResult<ProviderServiceDto>> GetPagedAsync(
        PagedRequest request,
        Guid? providerId = null,
        CancellationToken cancellationToken = default);

    Task<ProviderServiceDto> GetByIdAsync(Guid id, bool includeInactive = false, CancellationToken cancellationToken = default);

    Task<ProviderServiceDto> CreateAsync(CreateProviderServiceRequest request, CancellationToken cancellationToken = default);

    Task<ProviderServiceDto> UpdateAsync(Guid id, UpdateProviderServiceRequest request, CancellationToken cancellationToken = default);

    Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default);

    Task ActivateAsync(Guid id, CancellationToken cancellationToken = default);
}
