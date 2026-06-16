using FullstackTest.Domain.Constants;

namespace FullstackTest.Domain.Common;

public abstract class AuditableEntity : Entity
{
    public bool IsActive { get; protected set; } = true;
    public DateTime CreatedAtUtc { get; protected set; }
    public string CreatedBy { get; protected set; } = string.Empty;
    public DateTime? UpdatedAtUtc { get; protected set; }
    public string? UpdatedBy { get; protected set; }

    protected void SetCreatedAudit(string createdBy)
    {
        CreatedAtUtc = DateTime.UtcNow;
        CreatedBy = RequiredText(createdBy, nameof(createdBy), FieldLengths.AuditUser);
    }

    protected void SetUpdatedAudit(string updatedBy)
    {
        UpdatedAtUtc = DateTime.UtcNow;
        UpdatedBy = RequiredText(updatedBy, nameof(updatedBy), FieldLengths.AuditUser);
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetUpdatedAudit(updatedBy);
    }

    protected static string RequiredText(string value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{fieldName} es obligatorio.", fieldName);
        }

        var trimmedValue = value.Trim();

        if (trimmedValue.Length > maxLength)
        {
            throw new ArgumentException($"{fieldName} no puede superar {maxLength} caracteres.", fieldName);
        }

        return trimmedValue;
    }
}
