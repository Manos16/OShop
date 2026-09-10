using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OShop.Models.Common;

namespace OShop.Models;

[Table("trx_order_items")]
public class TrxOrderItemModel : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public TrxOrderModel? Order { get; set; }
 
    [Required]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public MstProductModel? Product { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } // Snapshot harga saat transaksi terjadi
}