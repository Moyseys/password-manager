using Microsoft.AspNetCore.Mvc;
using Vaultify.Features.Categories.Dtos.Requests;
using Vaultify.Features.Categories.Dtos.Response;

namespace Vaultify.Features.Categories;

[ApiController]
[Route("api/v1/categories")]
public class CategoryController(CategoryService categoryService) : ControllerBase
{
    private readonly CategoryService _categoryService = categoryService;

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponseDto>>> List()
    {
        return Ok(await _categoryService.ListCategories());
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> Create([FromBody] CategoryRequestCreateDto payload)
    {
        return Ok(await _categoryService.CreateCategory(payload));
    }

    [HttpPatch("{categoryId}")]
    public async Task<ActionResult<CategoryResponseDto>> Update(Guid categoryId, [FromBody] CategoryRequestUpdateDto payload)
    {
        return Ok(await _categoryService.UpdateCategory(categoryId, payload));
    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> Delete(Guid categoryId)
    {
        await _categoryService.DeleteCategory(categoryId);
        return NoContent();
    }
}
