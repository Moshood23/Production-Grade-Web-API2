namespace Production_Grade_Web_API.Application.DTOs
{
    public class AuthResponseDto
    {
        public Guid UserId { get; internal set; }
        public string? Email { get; internal set; }
        public string? Username { get; internal set; }
        public string? FullName { get; internal set; }
        public string? Token { get; internal set; }
        public string? RefreshToken { get; internal set; }
        public DateTime ExpiresAt { get; internal set; }
    }
}