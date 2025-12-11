namespace Production.Grade.WebApi.API.Controllers;

using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.Grade.WebApi.API.Common;
using Production.Grade.WebApi.Application.Interfaces;
using Production_Grade_Web_API.Application.DTO;
using Production_Grade_Web_API.Application.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IValidator<CreateProductDto> _createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductService productService,
        IValidator<CreateProductDto> createValidator,
        IValidator<UpdateProductDto> updateValidator,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ProductResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
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
            var result = await _productService.CreateAsync(userId, dto);
            _logger.LogInformation($"Product created: {result.SKU}");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ProductResponseDto>.SuccessResponse(result, StatusCodes.Status201Created));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating product: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.FailureResponse(ex.Message, StatusCodes.Status500InternalServerError));
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProductResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _productService.GetByIdAsync(id, userId);
            return Ok(ApiResponse<ProductResponseDto>.SuccessResponse(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpGet("sku/{sku}")]
    [ProducesResponseType(typeof(ApiResponse<ProductResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySku(string sku)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _productService.GetBySkuAsync(sku, userId);
            return Ok(ApiResponse<ProductResponseDto>.SuccessResponse(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _productService.GetAllAsync(userId, pageNumber, pageSize);
            return Ok(ApiResponse<IEnumerable<ProductListDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting products: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.FailureResponse(ex.Message, StatusCodes.Status500InternalServerError));
        }
    }

    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductListDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCategory(Guid categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _productService.GetByCategoryAsync(categoryId, userId, pageNumber, pageSize);
            return Ok(ApiResponse<IEnumerable<ProductListDto>>.SuccessResponse(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProductResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
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
            var result = await _productService.UpdateAsync(id, userId, dto);
            _logger.LogInformation($"Product updated: {id}");
            return Ok(ApiResponse<ProductResponseDto>.SuccessResponse(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            await _productService.DeleteAsync(id, userId);
            _logger.LogInformation($"Product deleted: {id}");
            return Ok(ApiResponse.SuccessResponse(StatusCodes.Status200OK));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }
}
