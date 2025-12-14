namespace Production.Grade.WebApi.Application.DTOs
{
    public class LoginDto
    {
        internal string? Email;
        internal string? Password;
        internal string? Username;
        internal string? PasswordHash;

        public object EmailOrUsername { get; internal set; }
    }
}