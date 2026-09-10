using Microsoft.EntityFrameworkCore;
using OShop.Data;
using OShop.Models;

namespace OShop.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MstCategoryModel>> GetAllAsync()
    {
        // Hanya mengambil kategori yang aktif (IsActive = true)
        return await _context.Categories
            .Where(c => c.IsActive)
            .ToListAsync();
    }

    public async Task<MstCategoryModel?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<MstCategoryModel> CreateAsync(MstCategoryModel category)
    {
        category.CreatedAt = DateTime.UtcNow;
        category.IsActive = true;
        
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<bool> UpdateAsync(int id, MstCategoryModel categoryDto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        category.Name = categoryDto.Name;
        category.Slug = categoryDto.Slug;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        // Menerapkan Soft Delete alih-alih Hard Delete
        category.IsActive = false;
        category.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}