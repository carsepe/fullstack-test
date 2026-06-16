using FullstackTest.Application.Abstractions;
using FullstackTest.Application.Auth;
using FullstackTest.Application.Common;
using FullstackTest.Infrastructure.Email;
using FullstackTest.Infrastructure.Identity;
using FullstackTest.Infrastructure.Persistence;
using FullstackTest.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FullstackTest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProviderRepository, ProviderRepository>();
        services.AddScoped<IProviderServiceRepository, ProviderServiceRepository>();
        services.AddScoped<ICurrentUserService, HttpContextCurrentUserService>();
        services.AddScoped<IEmailSender, LoggingEmailSender>();
        services.AddScoped<IAuthAppService, AuthAppService>();
        services.AddSingleton<JwtTokenService>();

        return services;
    }
}
