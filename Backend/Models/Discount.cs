using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Discount
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public DateTime BeginDate { get; set; }

    public DateTime EndDate { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal DiscountPercentage { get; set; }

    public Product Product { get; set; } = null!;
}
