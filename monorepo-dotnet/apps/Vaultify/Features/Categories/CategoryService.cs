using Core.Contexts;
using Core.Exceptions;
using DAL.Entities;
using DAL.Repositories;
using Vaultify.Features.Categories.Dtos.Requests;
using Vaultify.Features.Categories.Dtos.Response;

namespace Vaultify.Features.Categories;

public class CategoryService(
    CategoryRepository categoryRepository,
    UserContext userContext,
    ILogger<CategoryService> logger)
{
    private readonly CategoryRepository _categoryRepository = categoryRepository;
    private readonly UserContext _userContext = userContext;
    private readonly ILogger<CategoryService> _logger = logger;

    public async Task<List<CategoryResponseDto>> ListCategories()
    {
        var userId = _userContext.GetUserIdOrThrow();
        return await _categoryRepository.ListByUserId(userId, c => new CategoryResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Color = c.Color,
            Audit = new DAL.Dtos.AuditableDto(c.CreatedAt, c.CreatedBy, c.UpdatedAt, c.UpdatedBy)
        });
    }

    public async Task<CategoryResponseDto> CreateCategory(CategoryRequestCreateDto payload)
    {
        var userId = _userContext.GetUserIdOrThrow();

        if (await _categoryRepository.ExistsByNameAndUserId(payload.Name, userId))
            throw new ConflictException($"A category named '{payload.Name}' already exists");

        var category = payload.ToCategoryEntity(userId);
        await _categoryRepository.AddAsync(category);

        _logger.LogInformation("[CreateCategory] Created category {CategoryId} for user {UserId}", category.Id, userId);

        return category.ToCategoryResponseDto();
    }

    public async Task<CategoryResponseDto> UpdateCategory(Guid categoryId, CategoryRequestUpdateDto payload)
    {
        var userId = _userContext.GetUserIdOrThrow();
        var category = await GetOwnedCategory(categoryId, userId);

        if (!string.IsNullOrWhiteSpace(payload.Name) && !payload.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)
            && await _categoryRepository.ExistsByNameAndUserId(payload.Name, userId, categoryId))
            throw new ConflictException($"A category named '{payload.Name}' already exists");

        if (!string.IsNullOrWhiteSpace(payload.Name)) category.Name = payload.Name;
        if (!string.IsNullOrWhiteSpace(payload.Color)) category.Color = payload.Color;

        await _categoryRepository.UpdateAsync(category);

        return category.ToCategoryResponseDto();
    }

    public async Task DeleteCategory(Guid categoryId)
    {
        var userId = _userContext.GetUserIdOrThrow();
        var category = await GetOwnedCategory(categoryId, userId);

        if (await _categoryRepository.HasActiveSecrets(categoryId))
            throw new ConflictException("Cannot delete a category that still has secrets assigned to it");

        category.Active = false;
        await _categoryRepository.UpdateAsync(category);

        _logger.LogInformation("[DeleteCategory] Soft-deleted category {CategoryId} for user {UserId}", categoryId, userId);
    }

    public async Task<Guid?> ResolveCategoryId(Guid? categoryId, Guid userId)
    {
        if (categoryId is not { } id) return null;

        if (!await _categoryRepository.ExistsByIdAndUserId(id, userId))
            throw new InvalidDataException("Category not found");

        return id;
    }

    private async Task<Category> GetOwnedCategory(Guid categoryId, Guid userId)
    {
        return await _categoryRepository.GetByIdAndUserId(categoryId, userId)
            ?? throw new KeyNotFoundException("Category not found");
    }
}
