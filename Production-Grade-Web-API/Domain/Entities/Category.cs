namespace Production.Grade.WebApi.Domain.Entities;
public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid UserId { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ApplicationUser? User { get; set; }
}