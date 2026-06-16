using FullstackTest.Application.Abstractions;
using FullstackTest.Application.Common;
using FullstackTest.Application.Providers.Dtos;
using FullstackTest.Domain.Entities;

namespace FullstackTest.Application.Providers;

public class ProviderAppService(
    IProviderRepository providerRepository,
    ICurrentUserService currentUserService) : IProviderAppService
{
    public async Task<PagedResult<ProviderDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await providerRepository.GetPagedAsync(
            request.Search,
            request.SortBy,
            request.SortDescending,
            request.NormalizedPage,
            request.NormalizedPageSize,
            request.IncludeInactive,
            cancellationToken);

        return new PagedResult<ProviderDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = request.NormalizedPage,
            PageSize = request.NormalizedPageSize
        };
    }

    public async Task<ProviderDto> GetByIdAsync(Guid id, bool includeInactive = false, CancellationToken cancellationToken = default)
    {
        var provider = await providerRepository.GetByIdAsync(id, includeInactive, cancellationToken);
        if (provider is null)
        {
            throw new NotFoundException("El proveedor no fue encontrado.");
        }

        return MapToDto(provider);
    }

    public async Task<ProviderDto> CreateAsync(CreateProviderRequest request, CancellationToken cancellationToken = default)
    {
        var provider = new Provider(
            request.Nit,
            request.Name,
            request.Website,
            request.Email,
            currentUserService.Email);

        await providerRepository.AddAsync(provider, cancellationToken);
        return MapToDto(provider);
    }

    public async Task<ProviderDto> UpdateAsync(Guid id, UpdateProviderRequest request, CancellationToken cancellationToken = default)
    {
        var provider = await GetActiveProviderAsync(id, cancellationToken);
        provider.Update(request.Nit, request.Name, request.Website, request.Email, currentUserService.Email);
        await providerRepository.UpdateAsync(provider, cancellationToken);
        return MapToDto(provider);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var provider = await providerRepository.GetByIdAsync(id, includeInactive: true, cancellationToken);
        if (provider is null)
        {
            throw new NotFoundException("El proveedor no fue encontrado.");
        }

        if (!provider.IsActive)
        {
            throw new BusinessException("El proveedor ya se encuentra inactivo.");
        }

        provider.Deactivate(currentUserService.Email);
        await providerRepository.UpdateAsync(provider, cancellationToken);
    }

    public async Task ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var provider = await providerRepository.GetByIdAsync(id, includeInactive: true, cancellationToken);
        if (provider is null)
        {
            throw new NotFoundException("El proveedor no fue encontrado.");
        }

        if (provider.IsActive)
        {
            throw new BusinessException("El proveedor ya se encuentra activo.");
        }

        provider.Activate(currentUserService.Email);
        await providerRepository.UpdateAsync(provider, cancellationToken);
    }

    private async Task<Provider> GetActiveProviderAsync(Guid id, CancellationToken cancellationToken)
    {
        var provider = await providerRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (provider is null)
        {
            throw new NotFoundException("El proveedor no fue encontrado.");
        }

        return provider;
    }

    private static ProviderDto MapToDto(Provider provider)
    {
        return new ProviderDto(
            provider.Id,
            provider.Nit,
            provider.Name,
            provider.Website,
            provider.Email,
            provider.IsActive);
    }
}
