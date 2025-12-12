namespace Production.Grade.WebApi.Application.DTOs
{
    public  class CreateCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public string? CreateMap { get; set; }
    }
}