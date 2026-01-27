using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciandt.Retail.MCP.Models.Entities;

[Table("Orders")]
public class OrderEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; }

    [MaxLength(50)]
    public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending; // Pending, Processing, Shipped, Delivered, Cancelled

    [MaxLength(100)]
    public string? TrackingNumber { get; set; }

    [MaxLength(50)]
    public string? ShippingMethod { get; set; }

    [MaxLength(20)]
    public string? ShippingZipCode { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ShippedAt { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    // Navigation properties
    [ForeignKey(nameof(CustomerId))]
    public virtual CustomerEntity Customer { get; set; } = null!;

    public virtual ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
    public virtual ICollection<PaymentEntity> Payments { get; set; } = new List<PaymentEntity>();
}