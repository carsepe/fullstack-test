using FullstackTest.Application.Abstractions;
using FullstackTest.Application.Common;
using FullstackTest.Application.ProviderServices.Dtos;
using FullstackTest.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FullstackTest.Application.ProviderServices;

public class ProviderServiceAppService(
    IProviderServiceRepository providerServiceRepository,
    IProviderRepository providerRepository,
    IEmailSender emailSender,
    ICurrentUserService currentUserService,
    ILogger<ProviderServiceAppService> logger) : IProviderServiceAppService
{
    public async Task<PagedResult<ProviderServiceDto>> GetPagedAsync(
        PagedRequest request,
        Guid? providerId = null,
        CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await providerServiceRepository.GetPagedAsync(
            request.Search,
            providerId,
            request.SortBy,
            request.SortDescending,
            request.NormalizedPage,
            request.NormalizedPageSize,
            request.IncludeInactive,
            cancellationToken);

        return new PagedResult<ProviderServiceDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = request.NormalizedPage,
            PageSize = request.NormalizedPageSize
        };
    }

    public async Task<ProviderServiceDto> GetByIdAsync(Guid id, bool includeInactive = false, CancellationToken cancellationToken = default)
    {
        var service = await providerServiceRepository.GetByIdAsync(id, includeInactive, cancellationToken);
        if (service is null)
        {
            throw new NotFoundException("El servicio no fue encontrado.");
        }

        return MapToDto(service);
    }

    public async Task<ProviderServiceDto> CreateAsync(CreateProviderServiceRequest request, CancellationToken cancellationToken = default)
    {
        var provider = await providerRepository.GetByIdAsync(request.ProviderId, cancellationToken: cancellationToken);
        if (provider is null)
        {
            throw new NotFoundException("El proveedor no fue encontrado.");
        }

        if (!provider.IsActive)
        {
            throw new BusinessException("No se puede agregar un servicio a un proveedor inactivo.");
        }

        var service = ProviderService.Create(
            request.ProviderId,
            request.Name,
            request.HourlyRateUsd,
            currentUserService.Email);

        await providerServiceRepository.AddAsync(service, cancellationToken);

        try
        {
            await emailSender.SendAsync(
                "Nuevo servicio habilitado",
                $"El proveedor {provider.Name} habilitó el servicio {service.Name}.",
                cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "No se pudo enviar el correo de notificación del servicio {ServiceId}.", service.Id);
        }

        return MapToDto(service);
    }

    public async Task<ProviderServiceDto> UpdateAsync(Guid id, UpdateProviderServiceRequest request, CancellationToken cancellationToken = default)
    {
        var service = await GetActiveServiceAsync(id, cancellationToken);
        service.Update(request.Name, request.HourlyRateUsd, currentUserService.Email);
        await providerServiceRepository.UpdateAsync(service, cancellationToken);
        return MapToDto(service);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var service = await providerServiceRepository.GetByIdAsync(id, includeInactive: true, cancellationToken);
        if (service is null)
        {
            throw new NotFoundException("El servicio no fue encontrado.");
        }

        if (!service.IsActive)
        {
            throw new BusinessException("El servicio ya se encuentra inactivo.");
        }

        service.Deactivate(currentUserService.Email);
        await providerServiceRepository.UpdateAsync(service, cancellationToken);
    }

    public async Task ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var service = await providerServiceRepository.GetByIdAsync(id, includeInactive: true, cancellationToken);
        if (service is null)
        {
            throw new NotFoundException("El servicio no fue encontrado.");
        }

        if (service.IsActive)
        {
            throw new BusinessException("El servicio ya se encuentra activo.");
        }

        var provider = await providerRepository.GetByIdAsync(service.ProviderId, cancellationToken: cancellationToken);
        if (provider is null || !provider.IsActive)
        {
            throw new BusinessException("No se puede activar un servicio cuyo proveedor está inactivo.");
        }

        service.Activate(currentUserService.Email);
        await providerServiceRepository.UpdateAsync(service, cancellationToken);
    }

    private async Task<ProviderService> GetActiveServiceAsync(Guid id, CancellationToken cancellationToken)
    {
        var service = await providerServiceRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (service is null)
        {
            throw new NotFoundException("El servicio no fue encontrado.");
        }

        return service;
    }

    private static ProviderServiceDto MapToDto(ProviderService service)
    {
        return new ProviderServiceDto(
            service.Id,
            service.ProviderId,
            service.Name,
            service.HourlyRateUsd,
            service.IsActive);
    }
}
