using FullstackTest.Application.Auth;
using FullstackTest.Application.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FullstackTest.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthAppService authAppService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await authAppService.LoginAsync(request, cancellationToken);
        return Ok(response);
    }
}
