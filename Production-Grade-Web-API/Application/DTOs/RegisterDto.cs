namespace Production.Grade.WebApi.Application.DTOs
{
    public class RegisterDto
    {
        public string? Username { get; internal set; }
        public string? Password { get; internal set; }
        public string? FullName { get; internal set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get;  set; }
        public object? ConfirmPassword { get; internal set; }
    }
}