using FullstackTest.Application;
using FullstackTest.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FullstackTest.UnitTests.Architecture;

public class DependencyInjectionTests
{
    [Fact]
    public void AddProjectServices_ShouldRegisterWithoutErrors()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] =
                    "Server=(localdb)\\mssqllocaldb;Database=FullstackTest_Test;Trusted_Connection=True;TrustServerCertificate=True"
            })
            .Build();

        services.AddApplication();
        services.AddInfrastructure(configuration);
        using var provider = services.BuildServiceProvider();

        Assert.NotNull(provider);
    }
}
