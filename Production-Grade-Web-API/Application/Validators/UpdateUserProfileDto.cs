namespace Production.Grade.WebApi.Application.Validators
{
    public class UpdateUserProfileDto
    {
        internal string? PhoneNumber;

        public string? FullName { get; internal set; }
        public string? Email { get; internal set; }

    }
}
