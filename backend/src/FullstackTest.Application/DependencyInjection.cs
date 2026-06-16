using FullstackTest.Application.Auth;
using FullstackTest.Application.Dashboard;
using FullstackTest.Application.ProviderServices;
using FullstackTest.Application.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace FullstackTest.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProviderAppService, ProviderAppService>();
        services.AddScoped<IProviderServiceAppService, ProviderServiceAppService>();
        services.AddScoped<IDashboardAppService, DashboardAppService>();

        return services;
    }
}
