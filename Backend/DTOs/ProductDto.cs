namespace backend.DTOs;

public record ProductDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Manufacturer { get; init; } = string.Empty;
    public string Style { get; init; } = string.Empty;
    public decimal PurchasePrice { get; init; }
    public decimal SalePrice { get; init; }
    public int QtyOnHand { get; init; }
    public decimal CommissionPercentage { get; init; }
}

public record UpdateProductDto
{
    public string Name { get; init; } = string.Empty;
    public string Manufacturer { get; init; } = string.Empty;
    public string Style { get; init; } = string.Empty;
    public decimal PurchasePrice { get; init; }
    public decimal SalePrice { get; init; }
    public int QtyOnHand { get; init; }
    public decimal CommissionPercentage { get; init; }
}
