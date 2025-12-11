namespace Production.Grade.WebApi.Domain.Entities;
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
}