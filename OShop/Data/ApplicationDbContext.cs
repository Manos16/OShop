using Microsoft.EntityFrameworkCore;
using OShop.Models;

namespace OShop.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<MstCategoryModel> Categories => Set<MstCategoryModel>();
    public DbSet<MstProductModel> Products => Set<MstProductModel>();
    public DbSet<MstUserModel> Users => Set<MstUserModel>();
    public DbSet<MstUserAddressModel> UserAddresses => Set<MstUserAddressModel>();
    public DbSet<MstVoucherModel> Vouchers => Set<MstVoucherModel>();
    public DbSet<TrxOrderModel> Orders => Set<TrxOrderModel>();
    public DbSet<TrxOrderItemModel> OrderItems => Set<TrxOrderItemModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Contoh pengaturan Foreign Key / onDelete behavior jika diperlukan secara eksplisit
        modelBuilder.Entity<TrxOrderItemModel>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TrxOrderItemModel>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict); // Mencegah produk dihapus jika sudah ada di riwayat transaksi
    }
}