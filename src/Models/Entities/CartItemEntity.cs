using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciandt.Retail.MCP.Models.Entities;

[Table("CartItems")]
public class CartItemEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int CartId { get; set; }

    [Required]
    [MaxLength(50)]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public int Quantity { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(CartId))]
    public virtual CartEntity Cart { get; set; } = null!;

    [ForeignKey(nameof(ProductId))]
    public virtual ProductEntity? Product { get; set; }
}