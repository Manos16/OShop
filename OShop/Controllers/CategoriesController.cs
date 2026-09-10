using Microsoft.AspNetCore.Mvc;
using OShop.Models;
using OShop.Services.Category;

namespace OShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(new { success = true, data = categories });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound(new { success = false, message = "Kategori tidak ditemukan." });

        return Ok(new { success = true, data = category });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MstCategoryModel category)
    {
        var created = await _categoryService.CreateAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { success = true, data = created });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MstCategoryModel category)
    {
        var success = await _categoryService.UpdateAsync(id, category);
        if (!success)
            return NotFound(new { success = false, message = "Kategori tidak ditemukan." });

        return Ok(new { success = true, message = "Kategori berhasil diperbarui." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _categoryService.DeleteAsync(id);
        if (!success)
            return NotFound(new { success = false, message = "Kategori tidak ditemukan." });

        return Ok(new { success = true, message = "Kategori berhasil dihapus." });
    }
}