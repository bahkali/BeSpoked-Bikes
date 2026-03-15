using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Sale
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int SalespersonId { get; set; }

    public int CustomerId { get; set; }

    public DateTime SalesDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal SalespersonCommission { get; set; }

    public Product Product { get; set; } = null!;
    public Salesperson Salesperson { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}
