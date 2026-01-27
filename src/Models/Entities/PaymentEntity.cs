using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciandt.Retail.MCP.Models.Entities;

[Table("Payments")]
public class PaymentEntity
{
    [Key]
    [MaxLength(50)]
    public string PaymentId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(50)]
    public int OrderId { get; set; }

    [Required]
    [MaxLength(50)]
    public string PaymentMethodId { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public int Installments { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal InstallmentValue { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Processing, Approved, Declined, Refunded

    [MaxLength(500)]
    public string? PaymentUrl { get; set; }

    [MaxLength(100)]
    public string? TransactionId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ProcessedAt { get; set; }

    // Navigation properties
    [ForeignKey("Id")]
    public virtual OrderEntity Order { get; set; } = null!;
}