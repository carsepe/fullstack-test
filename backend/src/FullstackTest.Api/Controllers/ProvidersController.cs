using FullstackTest.Application.Common;
using FullstackTest.Application.Providers;
using FullstackTest.Application.Providers.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FullstackTest.Api.Controllers;

[ApiController]
[Route("api/providers")]
[Authorize]
public class ProvidersController(IProviderAppService providerAppService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ProviderDto>>> GetPaged(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await providerAppService.GetPagedAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProviderDto>> GetById(
        Guid id,
        [FromQuery] bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var provider = await providerAppService.GetByIdAsync(id, includeInactive, cancellationToken);
        return Ok(provider);
    }

    [HttpPost]
    public async Task<ActionResult<ProviderDto>> Create(
        [FromBody] CreateProviderRequest request,
        CancellationToken cancellationToken)
    {
        var provider = await providerAppService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = provider.Id }, provider);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProviderDto>> Update(
        Guid id,
        [FromBody] UpdateProviderRequest request,
        CancellationToken cancellationToken)
    {
        var provider = await providerAppService.UpdateAsync(id, request, cancellationToken);
        return Ok(provider);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        await providerAppService.DeactivateAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        await providerAppService.ActivateAsync(id, cancellationToken);
        return NoContent();
    }
}
