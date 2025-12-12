namespace Production.Grade.WebApi.API.Controllers;

using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.Grade.WebApi.API.Common;
using Production.Grade.WebApi.Application.Interfaces;
using Production.Grade.WebApi.Application.DTO;
using Production.Grade.WebApi.Application.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IValidator<CreateCartItemDto> _createItemValidator;
    private readonly IValidator<UpdateCartItemDto> _updateItemValidator;
    private readonly ILogger<CartsController> _logger;

    public CartsController(
        ICartService cartService,
        IValidator<CreateCartItemDto> createItemValidator,
        IValidator<UpdateCartItemDto> updateItemValidator,
        ILogger<CartsController> logger)
    {
        _cartService = cartService;
        _createItemValidator = createItemValidator;
        _updateItemValidator = updateItemValidator;
        _logger = logger;
    }

    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<CartResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _cartService.GetCartDetailsAsync(userId);
            return Ok(ApiResponse<CartResponseDto>.SuccessResponse(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpPost("items")]
    [ProducesResponseType(typeof(ApiResponse<CartItemResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItem([FromBody] CreateCartItemDto dto)
    {
        var validationResult = await _createItemValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse.FailureResponse(errors, StatusCodes.Status400BadRequest));
        }

        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _cartService.AddItemAsync(userId, dto);
            _logger.LogInformation($"Item added to cart: {dto.ProductId}");
            return CreatedAtAction(nameof(GetCart), new { }, ApiResponse<CartItemResponseDto>.SuccessResponse(result, StatusCodes.Status201Created));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpPut("items/{itemId}")]
    [ProducesResponseType(typeof(ApiResponse<CartItemResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem(Guid itemId, [FromBody] UpdateCartItemDto dto)
    {
        var validationResult = await _updateItemValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse.FailureResponse(errors, StatusCodes.Status400BadRequest));
        }

        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _cartService.UpdateItemAsync(userId, itemId, dto);
            _logger.LogInformation($"Cart item updated: {itemId}");
            return Ok(ApiResponse<CartItemResponseDto>.SuccessResponse(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpDelete("items/{itemId}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItem(Guid itemId)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            await _cartService.RemoveItemAsync(userId, itemId);
            _logger.LogInformation($"Item removed from cart: {itemId}");
            return Ok(ApiResponse.SuccessResponse(StatusCodes.Status200OK));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpDelete("clear")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ClearCart()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            await _cartService.ClearCartAsync(userId);
            _logger.LogInformation($"Cart cleared for user: {userId}");
            return Ok(ApiResponse.SuccessResponse(StatusCodes.Status200OK));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error clearing cart: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.FailureResponse(ex.Message, StatusCodes.Status500InternalServerError));
        }
    }
}
