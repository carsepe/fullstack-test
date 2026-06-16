using FullstackTest.Application.Auth.Dtos;

namespace FullstackTest.Application.Auth;

public interface IAuthAppService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
