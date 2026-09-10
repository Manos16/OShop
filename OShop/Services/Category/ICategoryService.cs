using OShop.Models;

namespace OShop.Services.Category;

public interface ICategoryService
{
    Task<IEnumerable<MstCategoryModel>> GetAllAsync();
    Task<MstCategoryModel?> GetByIdAsync(int id);
    Task<MstCategoryModel> CreateAsync(MstCategoryModel category);
    Task<bool> UpdateAsync(int id, MstCategoryModel category);
    Task<bool> DeleteAsync(int id);
}