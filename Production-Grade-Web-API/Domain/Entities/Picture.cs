namespace Production.Grade.WebApi.Domain.Entities;
public class Picture : BaseEntity
{
    public Guid ProductId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public int OrderID { get; set; }
    public Product? Product { get; set; }
    public object? AddedAt { get; internal set; }
}