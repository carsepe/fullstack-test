IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260616151342_InitialCreate'
)
BEGIN
    CREATE TABLE [Provider] (
        [Id] uniqueidentifier NOT NULL,
        [Nit] nvarchar(30) NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [Website] nvarchar(250) NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [CreatedBy] nvarchar(100) NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        [UpdatedBy] nvarchar(100) NULL,
        CONSTRAINT [PK_Provider] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260616151342_InitialCreate'
)
BEGIN
    CREATE TABLE [ProviderService] (
        [Id] uniqueidentifier NOT NULL,
        [ProviderId] uniqueidentifier NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [HourlyRateUsd] decimal(18,2) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [CreatedBy] nvarchar(100) NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        [UpdatedBy] nvarchar(100) NULL,
        CONSTRAINT [PK_ProviderService] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProviderService_Provider_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Provider] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260616151342_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Provider_Nit] ON [Provider] ([Nit]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260616151342_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProviderService_ProviderId] ON [ProviderService] ([ProviderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260616151342_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260616151342_InitialCreate', N'10.0.7');
END;

COMMIT;
GO

