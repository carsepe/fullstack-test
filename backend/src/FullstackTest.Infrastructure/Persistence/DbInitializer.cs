using FullstackTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FullstackTest.Infrastructure.Persistence;

public static class DbInitializer
{
    private const string SeedUser = "system";

    public static async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken = default)
    {
        if (await context.Providers.AnyAsync(cancellationToken))
        {
            return;
        }

        var providers = new List<Provider>
        {
            CreateProvider("900123456-7", "Importaciones Tekus S.A.", "https://tekus.co", "contact@tekus.co"),
            CreateProvider("800456789-1", "Soluciones Andinas Ltda.", "https://solucionesandinas.com", "info@solucionesandinas.com"),
            CreateProvider("900987654-3", "Nova Digital S.A.S.", "https://novadigital.io", "hola@novadigital.io"),
            CreateProvider("901222333-4", "CloudBridge Colombia", "https://cloudbridge.co", "soporte@cloudbridge.co"),
            CreateProvider("800555121-2", "DataPulse Group", "https://datapulse.com", "contacto@datapulse.com"),
            CreateProvider("900444555-6", "Ingeniería Vectorial", "https://vectorial.dev", "admin@vectorial.dev"),
            CreateProvider("901777888-9", "Pixel Norte S.A.", "https://pixelnorte.com", "ventas@pixelnorte.com"),
            CreateProvider("800333222-1", "Altiplano Software", "https://altiplano.tech", "hello@altiplano.tech"),
            CreateProvider("900666777-8", "RioTech Partners", "https://riotech.co", "partners@riotech.co"),
            CreateProvider("901111000-5", "Cumbre Analytics", "https://cumbreanalytics.com", "data@cumbreanalytics.com"),
        };

        providers[0].AddService("Descarga espacial de contenidos", 85.00m, SeedUser);
        providers[0].AddService("Desaparición forzada de bytes", 120.50m, SeedUser);
        providers[1].AddService("Integración de APIs REST", 65.00m, SeedUser);
        providers[1].AddService("Migración de datos legacy", 95.00m, SeedUser);
        providers[2].AddService("Diseño de interfaces web", 55.00m, SeedUser);
        providers[3].AddService("Administración de Azure", 110.00m, SeedUser);
        providers[4].AddService("Automatización de pipelines CI/CD", 98.00m, SeedUser);
        providers[5].AddService("Consultoría de arquitectura limpia", 150.00m, SeedUser);
        providers[6].AddService("Desarrollo de microfrontends", 78.00m, SeedUser);
        providers[7].AddService("Auditoría de seguridad OWASP", 135.00m, SeedUser);
        providers[8].AddService("Implementación de observabilidad", 88.00m, SeedUser);
        providers[9].AddService("Modelado de datos analíticos", 102.00m, SeedUser);

        await context.Providers.AddRangeAsync(providers, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static Provider CreateProvider(string nit, string name, string website, string email)
    {
        return new Provider(nit, name, website, email, SeedUser);
    }
}
