using FullstackTest.Application;
using FullstackTest.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace FullstackTest.UnitTests.Architecture;

public class DependencyInjectionTests
{
    [Fact]
    public void AddProjectServices_ShouldRegisterWithoutErrors()
    {
        var services = new ServiceCollection();

        services.AddApplication();
        services.AddInfrastructure();
        using var provider = services.BuildServiceProvider();

        Assert.NotNull(provider);
    }
}
