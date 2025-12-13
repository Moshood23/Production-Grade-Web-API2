namespace Production.Grade.WebApi.Application.Interfaces;
using AutoMapper;
using Production.Grade.WebApi.Application.DTOs;
using Production.Grade.WebApi.Application.Validators;
using Production_Grade_Web_API.Application.DTOs;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(Guid userId);
    Task<UserProfileDto> GetCurrentUserAsync(string userId);
    Task<UserProfileDto> UpdateProfileAsync(string userId, UpdateUserProfileDto dto);
}
