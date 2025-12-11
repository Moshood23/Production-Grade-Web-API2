namespace Production.Grade.WebApi.Application.Mappings
{
    public  class CreateCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

    }
}