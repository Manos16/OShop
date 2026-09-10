using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OShop.Data;
using OShop.Models;

namespace OShop.Services.Product;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly string _uploadPath;

    public ProductService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        // Mengambil path folder eksternal dari appsettings.json
        _uploadPath = configuration["FileSettings:UploadPath"] ?? "C:\\OShopStorage\\Uploads";
    }

    public async Task<IEnumerable<MstProductModel>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<MstProductModel?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
    }

    public async Task<MstProductModel> CreateAsync(MstProductModel product, IFormFile? imageFile)
    {
        // Tangani upload gambar jika ada file yang dikirim
        if (imageFile != null && imageFile.Length > 0)
        {
            product.ImageUrl = await SaveImageFileAsync(imageFile);
        }

        product.CreatedAt = DateTime.UtcNow;
        product.IsActive = true;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> UpdateAsync(int id, MstProductModel productDto, IFormFile? imageFile)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null || !product.IsActive) return false;

        product.Name = productDto.Name;
        product.Slug = productDto.Slug;
        product.Description = productDto.Description;
        product.Price = productDto.Price;
        product.Stock = productDto.Stock;
        product.CategoryId = productDto.CategoryId;
        product.UpdatedAt = DateTime.UtcNow;

        // Jika ada file gambar baru yang di-upload, timpa/perbarui ImageUrl
        if (imageFile != null && imageFile.Length > 0)
        {
            // Opsional: Hapus file lama di folder eksternal jika diperlukan
            product.ImageUrl = await SaveImageFileAsync(imageFile);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null || !product.IsActive) return false;

        // Soft Delete
        product.IsActive = false;
        product.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    // Helper privat untuk menyimpan file fisik ke folder eksternal
    private async Task<string> SaveImageFileAsync(IFormFile file)
    {
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var fullPath = Path.Combine(_uploadPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/{fileName}";
    }
}