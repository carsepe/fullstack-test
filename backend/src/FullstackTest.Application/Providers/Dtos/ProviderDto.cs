namespace FullstackTest.Application.Providers.Dtos;

public record ProviderDto(
    Guid Id,
    string Nit,
    string Name,
    string Website,
    string Email,
    bool IsActive,
    DateTime CreatedAtUtc,
    string CreatedBy,
    DateTime? UpdatedAtUtc,
    string? UpdatedBy);
