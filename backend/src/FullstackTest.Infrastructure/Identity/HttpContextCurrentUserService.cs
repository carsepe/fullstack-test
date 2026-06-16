using System.Security.Claims;
using FullstackTest.Application.Common;
using Microsoft.AspNetCore.Http;

namespace FullstackTest.Infrastructure.Identity;

public class HttpContextCurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string Email
    {
        get
        {
            var email = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            return string.IsNullOrWhiteSpace(email) ? "system" : email;
        }
    }
}
