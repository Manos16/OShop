using Microsoft.AspNetCore.Http;
using OShop.Models;

namespace OShop.Services.Product;

public interface IProductService
{
    Task<IEnumerable<MstProductModel>> GetAllAsync();
    Task<MstProductModel?> GetByIdAsync(int id);
    Task<MstProductModel> CreateAsync(MstProductModel product, IFormFile? imageFile);
    Task<bool> UpdateAsync(int id, MstProductModel product, IFormFile? imageFile);
    Task<bool> DeleteAsync(int id);
}