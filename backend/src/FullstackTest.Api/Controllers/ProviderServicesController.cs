using FullstackTest.Application.Common;
using FullstackTest.Application.ProviderServices;
using FullstackTest.Application.ProviderServices.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FullstackTest.Api.Controllers;

[ApiController]
[Route("api/provider-services")]
[Authorize]
public class ProviderServicesController(IProviderServiceAppService providerServiceAppService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ProviderServiceDto>>> GetPaged(
        [FromQuery] PagedRequest request,
        [FromQuery] Guid? providerId,
        CancellationToken cancellationToken)
    {
        var result = await providerServiceAppService.GetPagedAsync(request, providerId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProviderServiceDto>> GetById(
        Guid id,
        [FromQuery] bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var service = await providerServiceAppService.GetByIdAsync(id, includeInactive, cancellationToken);
        return Ok(service);
    }

    [HttpPost]
    public async Task<ActionResult<ProviderServiceDto>> Create(
        [FromBody] CreateProviderServiceRequest request,
        CancellationToken cancellationToken)
    {
        var service = await providerServiceAppService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProviderServiceDto>> Update(
        Guid id,
        [FromBody] UpdateProviderServiceRequest request,
        CancellationToken cancellationToken)
    {
        var service = await providerServiceAppService.UpdateAsync(id, request, cancellationToken);
        return Ok(service);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        await providerServiceAppService.DeactivateAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        await providerServiceAppService.ActivateAsync(id, cancellationToken);
        return NoContent();
    }
}
