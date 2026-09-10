using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using OShop.Data;
using OShop.Services.Category;
using OShop.Services.Product;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan baris ini untuk mendaftarkan ApplicationDbContext dengan SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var uploadPath = builder.Configuration["FileSettings:UploadPath"] ?? "C:\\OShopStorage\\Uploads";
if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadPath),
    RequestPath = "/uploads" // URL prefix untuk mengakses file
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
