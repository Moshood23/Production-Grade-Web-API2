namespace Production.Grade.WebApi.API.Controllers;

using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.Grade.WebApi.API.Common;
using Production.Grade.WebApi.Application.DTOs.Category;
using Production.Grade.WebApi.Application.Interfaces;
using Production.Grade.WebApi.Application.DTOs;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IValidator<CreateCategoryDto> _createValidator;
    private readonly IValidator<UpdateCategoryDto> _updateValidator;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(
        ICategoryService categoryService,
        IValidator<CreateCategoryDto> createValidator,
        IValidator<UpdateCategoryDto> updateValidator,
        ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CategoryResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
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
            var result = await _categoryService.CreateAsync(userId, dto);
            _logger.LogInformation($"Category created: {result.Name}");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<CategoryResponseDto>.SuccessResponse(result, StatusCodes.Status201Created));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating category: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.FailureResponse(ex.Message, StatusCodes.Status500InternalServerError));
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _categoryService.GetByIdAsync(id, userId);
            return Ok(ApiResponse<CategoryResponseDto>.SuccessResponse(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated", StatusCodes.Status401Unauthorized));

        try
        {
            var result = await _categoryService.GetAllAsync(userId);
            return Ok(ApiResponse<IEnumerable<CategoryResponseDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting categories: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.FailureResponse(ex.Message, StatusCodes.Status500InternalServerError));
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDto dto)
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
            var result = await _categoryService.UpdateAsync(id, userId, dto);
            _logger.LogInformation($"Category updated: {id}");
            return Ok(ApiResponse<CategoryResponseDto>.SuccessResponse(result));
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
            await _categoryService.DeleteAsync(id, userId);
            _logger.LogInformation($"Category deleted: {id}");
            return Ok(ApiResponse.SuccessResponse(StatusCodes.Status200OK));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.FailureResponse(ex.Message, StatusCodes.Status404NotFound));
        }
    }
}
