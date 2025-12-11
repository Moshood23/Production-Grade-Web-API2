namespace Production.Grade.WebApi.Application.Services
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int CategoryID { get; set; }
        public object Name { get; internal set; }
    }
}