using FullstackTest.Domain.Constants;
using FullstackTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FullstackTest.Infrastructure.Persistence.Configurations;

public class ProviderServiceConfiguration : IEntityTypeConfiguration<ProviderService>
{
    public void Configure(EntityTypeBuilder<ProviderService> builder)
    {
        builder.ToTable("ProviderService");

        builder.HasKey(service => service.Id);

        builder.Property(service => service.Name)
            .HasMaxLength(FieldLengths.Name)
            .IsRequired();

        builder.Property(service => service.HourlyRateUsd)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(service => service.CreatedBy)
            .HasMaxLength(FieldLengths.AuditUser)
            .IsRequired();

        builder.Property(service => service.UpdatedBy)
            .HasMaxLength(FieldLengths.AuditUser);

        builder.HasIndex(service => service.ProviderId);
    }
}
