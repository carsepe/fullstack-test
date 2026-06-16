using System.Security.Claims;
using FullstackTest.Application.Auth;
using FullstackTest.Application.Auth.Dtos;
using FullstackTest.Application.Common;
using Microsoft.Extensions.Configuration;

namespace FullstackTest.Infrastructure.Identity;

public class AuthAppService(IConfiguration configuration, JwtTokenService jwtTokenService) : IAuthAppService
{
    public Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var defaultEmail = configuration["Auth:DefaultUser:Email"];
        var defaultPassword = configuration["Auth:DefaultUser:Password"];

        if (!string.Equals(request.Email, defaultEmail, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(request.Password, defaultPassword, StringComparison.Ordinal))
        {
            throw new BusinessException("Credenciales inválidas.");
        }

        var token = jwtTokenService.GenerateToken(defaultEmail!);
        return Task.FromResult(new LoginResponse(token, defaultEmail!));
    }
}
