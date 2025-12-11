namespace Production.Grade.WebApi.API.Controllers;

using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.Grade.WebApi.API.Common;
using Production.Grade.WebApi.Application.Interfaces;
using Production.Grade.WebApi.Application.Mappings;
using Production_Grade_Web_API.Application.DTOs;
using Production_Grade_Web_API.Application.Validators;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var validationResult = await _registerValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse.FailureResponse(errors, StatusCodes.Status400BadRequest));
        }

        try
        {
            var result = await _authService.RegisterAsync(dto);
            _logger.LogInformation($"User registered: {dto.Email}");
            return CreatedAtAction(nameof(GetCurrentUser), new { }, ApiResponse<AuthResponseDto>.SuccessResponse(result, StatusCodes.Status201Created));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var validationResult = await _loginValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return Unauthorized(ApiResponse.FailureResponse(errors, StatusCodes.Status401Unauthorized));
        }

        try
        {
            var result = await _authService.LoginAsync(dto);
            _logger.LogInformation($"User logged in: {dto.EmailOrUsername}");
            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status401Unauthorized));
        }
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _authService.GetCurrentUserAsync(userId);
            return Ok(ApiResponse<UserProfileDto>.SuccessResponse(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpPut("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _authService.UpdateProfileAsync(userId, dto);
            return Ok(ApiResponse<UserProfileDto>.SuccessResponse(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        await _authService.LogoutAsync(Guid.Parse(userId));
        _logger.LogInformation($"User logged out: {userId}");
        return Ok(ApiResponse.SuccessResponse(StatusCodes.Status200OK));
    }
}
