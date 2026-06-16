using FullstackTest.Domain.Entities;

namespace FullstackTest.UnitTests.Domain;

public class ProviderTests
{
    [Fact]
    public void Constructor_ShouldCreateProvider_WhenDataIsValid()
    {
        var provider = new Provider(
            "900123456-7",
            "Importaciones Tekus S.A.",
            "https://tekus.co",
            "contact@tekus.co",
            "admin@fullstack.test");

        Assert.NotEqual(Guid.Empty, provider.Id);
        Assert.Equal("900123456-7", provider.Nit);
        Assert.Equal("Importaciones Tekus S.A.", provider.Name);
        Assert.True(provider.IsActive);
    }

    [Fact]
    public void Constructor_ShouldRejectInvalidEmail()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Provider(
                "900123456-7",
                "Importaciones Tekus S.A.",
                "https://tekus.co",
                "invalid-email",
                "admin@fullstack.test"));

        Assert.Equal("email", exception.ParamName);
    }

    [Fact]
    public void AddService_ShouldCreateProviderService_WhenDataIsValid()
    {
        var provider = new Provider(
            "900123456-7",
            "Importaciones Tekus S.A.",
            "https://tekus.co",
            "contact@tekus.co",
            "admin@fullstack.test");

        var service = provider.AddService("Content management", 45.50m, "admin@fullstack.test");

        Assert.Equal(provider.Id, service.ProviderId);
        Assert.Equal("Content management", service.Name);
        Assert.Equal(45.50m, service.HourlyRateUsd);
        Assert.Single(provider.Services);
    }

    [Fact]
    public void Deactivate_ShouldSetProviderInactive()
    {
        var provider = new Provider(
            "900123456-7",
            "Importaciones Tekus S.A.",
            "https://tekus.co",
            "contact@tekus.co",
            "admin@fullstack.test");

        provider.Deactivate("admin@fullstack.test");

        Assert.False(provider.IsActive);
    }

    [Fact]
    public void Activate_ShouldSetProviderActive()
    {
        var provider = new Provider(
            "900123456-7",
            "Importaciones Tekus S.A.",
            "https://tekus.co",
            "contact@tekus.co",
            "admin@fullstack.test");

        provider.Deactivate("admin@fullstack.test");
        provider.Activate("admin@fullstack.test");

        Assert.True(provider.IsActive);
    }
}
