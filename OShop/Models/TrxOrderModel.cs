using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OShop.Models.Common;

namespace OShop.Models;

[Table("trx_orders")]
public class TrxOrderModel : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public MstUserModel? User { get; set; }

    public int? VoucherId { get; set; }

    [ForeignKey("VoucherId")]
    public MstVoucherModel? Voucher { get; set; }

    [Required]
    [MaxLength(100)]
    public string OrderCode { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotalAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Paid, Shipped, Cancelled

    // --- Snapshot Alamat Pengiriman ---
    [Required]
    [MaxLength(150)]
    public string ShippingRecipientName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ShippingPhoneNumber { get; set; } = string.Empty;

    [Required]
    public string ShippingAddressLine { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ShippingCity { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string ShippingPostalCode { get; set; } = string.Empty;
    
    public ICollection<TrxOrderItemModel> OrderItems { get; set; } = new List<TrxOrderItemModel>();
}