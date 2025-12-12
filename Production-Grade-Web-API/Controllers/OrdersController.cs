
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.Grade.WebApi.API.Common;
using Production.Grade.WebApi.Application.Interfaces;
using Production.Grade.WebApi.Application.DTO;

namespace Production.Grade.WebApi.API.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IValidator<CreateOrderDto> _createValidator;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(
        IOrderService orderService,
        IValidator<CreateOrderDto> createValidator,
        ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _createValidator = createValidator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<OrderResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
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
            var result = await _orderService.CreateOrderAsync(userId, dto);
            _logger.LogInformation($"Order created: {result.OrderNumber}");
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, ApiResponse<OrderResponseDto>.SuccessResponse(result, StatusCodes.Status201Created));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status409Conflict));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating order: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.FailureResponse(ex.Message, StatusCodes.Status500InternalServerError));
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<OrderResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _orderService.GetOrderByIdAsync(id, userId);
            return Ok(ApiResponse<OrderResponseDto>.SuccessResponse(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrderListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _orderService.GetUserOrdersAsync(userId, pageNumber, pageSize);
            return Ok(ApiResponse<IEnumerable<OrderListDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting orders: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.FailureResponse(ex.Message, StatusCodes.Status500InternalServerError));
        }
    }

    [HttpPut("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<OrderResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _orderService.CancelOrderAsync(id, userId);
            _logger.LogInformation($"Order cancelled: {id}");
            return Ok(ApiResponse<OrderResponseDto>.SuccessResponse(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status409Conflict));
        }
    }

    [HttpPost("checkout")]
    [ProducesResponseType(typeof(ApiResponse<OrderResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CheckoutCart()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _orderService.CheckoutCartAsync(userId);
            _logger.LogInformation($"Cart checked out: {result.OrderNumber}");
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, ApiResponse<OrderResponseDto>.SuccessResponse(result, StatusCodes.Status201Created));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status400BadRequest));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking out cart: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.FailureResponse(ex.Message, StatusCodes.Status500InternalServerError));
        }
    }
}
