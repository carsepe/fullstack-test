-- Initial seed data for FullstackTestDb
-- Requires schema.sql to be applied first.

SET NOCOUNT ON;

IF NOT EXISTS (SELECT 1 FROM [Provider])
BEGIN
    DECLARE @Now datetime2 = SYSUTCDATETIME();

    INSERT INTO [Provider] ([Id], [Nit], [Name], [Website], [Email], [IsActive], [CreatedAtUtc], [CreatedBy])
    VALUES
        ('11111111-1111-1111-1111-111111111101', '900123456-7', 'Importaciones Tekus S.A.', 'https://tekus.co', 'contact@tekus.co', 1, @Now, 'system'),
        ('11111111-1111-1111-1111-111111111102', '800456789-1', 'Soluciones Andinas Ltda.', 'https://solucionesandinas.com', 'info@solucionesandinas.com', 1, @Now, 'system'),
        ('11111111-1111-1111-1111-111111111103', '900987654-3', 'Nova Digital S.A.S.', 'https://novadigital.io', 'hola@novadigital.io', 1, @Now, 'system'),
        ('11111111-1111-1111-1111-111111111104', '901222333-4', 'CloudBridge Colombia', 'https://cloudbridge.co', 'soporte@cloudbridge.co', 1, @Now, 'system'),
        ('11111111-1111-1111-1111-111111111105', '800555121-2', 'DataPulse Group', 'https://datapulse.com', 'contacto@datapulse.com', 1, @Now, 'system'),
        ('11111111-1111-1111-1111-111111111106', '900444555-6', 'Ingeniería Vectorial', 'https://vectorial.dev', 'admin@vectorial.dev', 1, @Now, 'system'),
        ('11111111-1111-1111-1111-111111111107', '901777888-9', 'Pixel Norte S.A.', 'https://pixelnorte.com', 'ventas@pixelnorte.com', 1, @Now, 'system'),
        ('11111111-1111-1111-1111-111111111108', '800333222-1', 'Altiplano Software', 'https://altiplano.tech', 'hello@altiplano.tech', 1, @Now, 'system'),
        ('11111111-1111-1111-1111-111111111109', '900666777-8', 'RioTech Partners', 'https://riotech.co', 'partners@riotech.co', 1, @Now, 'system'),
        ('11111111-1111-1111-1111-111111111110', '901111000-5', 'Cumbre Analytics', 'https://cumbreanalytics.com', 'data@cumbreanalytics.com', 1, @Now, 'system');

    INSERT INTO [ProviderService] ([Id], [ProviderId], [Name], [HourlyRateUsd], [IsActive], [CreatedAtUtc], [CreatedBy])
    VALUES
        ('22222222-2222-2222-2222-222222222201', '11111111-1111-1111-1111-111111111101', 'Descarga espacial de contenidos', 85.00, 1, @Now, 'system'),
        ('22222222-2222-2222-2222-222222222202', '11111111-1111-1111-1111-111111111101', 'Desaparición forzada de bytes', 120.50, 1, @Now, 'system'),
        ('22222222-2222-2222-2222-222222222203', '11111111-1111-1111-1111-111111111102', 'Integración de APIs REST', 65.00, 1, @Now, 'system'),
        ('22222222-2222-2222-2222-222222222204', '11111111-1111-1111-1111-111111111102', 'Migración de datos legacy', 95.00, 1, @Now, 'system'),
        ('22222222-2222-2222-2222-222222222205', '11111111-1111-1111-1111-111111111103', 'Diseño de interfaces web', 55.00, 1, @Now, 'system'),
        ('22222222-2222-2222-2222-222222222206', '11111111-1111-1111-1111-111111111104', 'Administración de Azure', 110.00, 1, @Now, 'system'),
        ('22222222-2222-2222-2222-222222222207', '11111111-1111-1111-1111-111111111105', 'Automatización de pipelines CI/CD', 98.00, 1, @Now, 'system'),
        ('22222222-2222-2222-2222-222222222208', '11111111-1111-1111-1111-111111111106', 'Consultoría de arquitectura limpia', 150.00, 1, @Now, 'system'),
        ('22222222-2222-2222-2222-222222222209', '11111111-1111-1111-1111-111111111107', 'Desarrollo de microfrontends', 78.00, 1, @Now, 'system'),
        ('22222222-2222-2222-2222-222222222210', '11111111-1111-1111-1111-111111111108', 'Auditoría de seguridad OWASP', 135.00, 1, @Now, 'system'),
        ('22222222-2222-2222-2222-222222222211', '11111111-1111-1111-1111-111111111109', 'Implementación de observabilidad', 88.00, 1, @Now, 'system'),
        ('22222222-2222-2222-2222-222222222212', '11111111-1111-1111-1111-111111111110', 'Modelado de datos analíticos', 102.00, 1, @Now, 'system');
END;
