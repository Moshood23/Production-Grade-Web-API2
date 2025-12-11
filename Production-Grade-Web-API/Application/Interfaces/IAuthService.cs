namespace Production.Grade.WebApi.Application.Interfaces;

using Production.Grade.WebApi.Application.Mappings;
using Production_Grade_Web_API.Application.DTOs;
using Production_Grade_Web_API.Application.Validators;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(Guid userId);
    Task<UserProfileDto> GetCurrentUserAsync(string userId);
    Task<UserProfileDto> UpdateProfileAsync(string userId, UpdateUserProfileDto dto);
}
