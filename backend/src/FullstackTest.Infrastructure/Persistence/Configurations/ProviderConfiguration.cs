using FullstackTest.Domain.Constants;
using FullstackTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FullstackTest.Infrastructure.Persistence.Configurations;

public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.ToTable("Provider");

        builder.HasKey(provider => provider.Id);

        builder.Property(provider => provider.Nit)
            .HasMaxLength(FieldLengths.Nit)
            .IsRequired();

        builder.HasIndex(provider => provider.Nit)
            .IsUnique();

        builder.Property(provider => provider.Name)
            .HasMaxLength(FieldLengths.Name)
            .IsRequired();

        builder.Property(provider => provider.Website)
            .HasMaxLength(FieldLengths.Website)
            .IsRequired();

        builder.Property(provider => provider.Email)
            .HasMaxLength(FieldLengths.Email)
            .IsRequired();

        builder.Property(provider => provider.CreatedBy)
            .HasMaxLength(FieldLengths.AuditUser)
            .IsRequired();

        builder.Property(provider => provider.UpdatedBy)
            .HasMaxLength(FieldLengths.AuditUser);

        builder.HasMany<ProviderService>("_services")
            .WithOne()
            .HasForeignKey(service => service.ProviderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(provider => provider.Services);
    }
}
