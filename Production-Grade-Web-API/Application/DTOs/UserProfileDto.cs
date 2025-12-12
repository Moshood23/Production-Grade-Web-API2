namespace Production.Grade.WebApi.Application.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserProfileDto() { }
    }
}