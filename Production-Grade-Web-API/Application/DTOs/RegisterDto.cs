namespace Production_Grade_Web_API.Application.DTOs
{
    public class RegisterDto
    {
        public string? Username { get; internal set; }
        public string? Password { get; internal set; }
        public string? FullName { get; internal set; }
        public string? Email { get; internal set; }
        public string? PhoneNumber { get; internal set; }
    }
}