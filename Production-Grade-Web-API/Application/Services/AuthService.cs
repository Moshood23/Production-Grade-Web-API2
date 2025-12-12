namespace Production.Grade.WebApi.Application.Services;

using AutoMapper;
using BCrypt.Net;
using Production.Grade.WebApi.Application.Interfaces;
using Production.Grade.WebApi.Domain.Entities;
using Production.Grade.WebApi.Domain.Interfaces;
using Production.Grade.WebApi.Infrastructure.Services;
using Production.Grade.WebApi.Application.DTOs;
using Production.Grade.WebApi.Application.Validators;
using Production_Grade_Web_API.Application.Interfaces;
using Production_Grade_Web_API.Application.DTOs;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AutoMapper.IMapper _mapper;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(IUnitOfWork unitOfWork, AutoMapper.IMapper mapper, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        if (users.Any(u => u.Email == dto.Email || u.Username == dto.Username))
            throw new InvalidOperationException("Email or username already exists");

        var user = new ApplicationUser
        {
            Email = dto.Email,
            Username = dto.Username,
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.HashPassword(dto.Password),
            IsActive = true
        };

        var cart = new Cart
        {
            UserId = user.Id,
            CreatedBy = user.Id.ToString()
        };

        var userResult = await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.Carts.AddAsync(cart);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtTokenService.GenerateToken(userResult);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        return new AuthResponseDto
        {
            UserId = userResult.Id,
            Email = userResult.Email,
            Username = userResult.Username,
            FullName = userResult.FullName,
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        var user = users.FirstOrDefault(u => (u.Email == dto.EmailOrUsername || u.Username == dto.EmailOrUsername) && u.IsActive);

        if (user == null || !BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        user.LastLoginAt = DateTime.UtcNow;
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtTokenService.GenerateToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Username = user.Username,
            FullName = user.FullName,
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
    {
        await Task.CompletedTask;
        throw new NotImplementedException("Refresh token logic will be implemented later");
    }

    public async Task LogoutAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user != null)
        {
            user.LastLoginAt = DateTime.UtcNow;
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<UserProfileDto> GetCurrentUserAsync(string userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(Guid.Parse(userId));
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found");

        return _mapper.Map<UserProfileDto>(user);
    }

    public async Task<UserProfileDto> UpdateProfileAsync(string userId, UpdateUserProfileDto dto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(Guid.Parse(userId));
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found");

        if (!string.IsNullOrEmpty(dto.FullName))
            user.FullName = dto.FullName;

        if (!string.IsNullOrEmpty(dto.PhoneNumber))
            user.PhoneNumber = dto.PhoneNumber;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserProfileDto>(user);
    }
}
