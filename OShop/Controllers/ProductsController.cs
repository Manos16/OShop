using Microsoft.AspNetCore.Mvc;
using OShop.Models;
using OShop.Services.Product;

namespace OShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(new { success = true, data = products });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound(new { success = false, message = "Produk tidak ditemukan." });

        return Ok(new { success = true, data = product });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] MstProductModel productModel, IFormFile? imageFile)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, message = "Data tidak valid.", errors = ModelState });

        var createdProduct = await _productService.CreateAsync(productModel, imageFile);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, new { success = true, data = createdProduct });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] MstProductModel productModel, IFormFile? imageFile)
    {
        var success = await _productService.UpdateAsync(id, productModel, imageFile);
        if (!success)
            return NotFound(new { success = false, message = "Produk tidak ditemukan." });

        return Ok(new { success = true, message = "Produk berhasil diperbarui." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _productService.DeleteAsync(id);
        if (!success)
            return NotFound(new { success = false, message = "Produk tidak ditemukan." });

        return Ok(new { success = true, message = "Produk berhasil dihapus." });
    }
}