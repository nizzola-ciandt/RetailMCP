using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciandt.Retail.MCP.Models.Entities;

[Table("Products")]
public class ProductEntity
{
    [Key]
    [MaxLength(50)]
    public string ProductId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Brand { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountedPrice { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public double AverageRating { get; set; }

    public int ReviewCount { get; set; }

    public bool InStock { get; set; }

    [MaxLength(500)]
    public string? AvailableColors { get; set; } // JSON serializado

    [MaxLength(500)]
    public string? AvailableSizes { get; set; } // JSON serializado

    public bool HasPromotion { get; set; }

    [MaxLength(100)]
    public string? PromoLabel { get; set; }

    public bool IsBestSeller { get; set; }

    public bool IsNew { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
}