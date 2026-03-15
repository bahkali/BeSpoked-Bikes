namespace backend.DTOs;

public record SaleDto
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int SalespersonId { get; init; }
    public string SalespersonName { get; init; } = string.Empty;
    public int CustomerId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public DateTime SalesDate { get; init; }
    public decimal Price { get; init; }
    public decimal SalespersonCommission { get; init; }
}

public record CreateSaleDto
{
    public int ProductId { get; init; }
    public int SalespersonId { get; init; }
    public int CustomerId { get; init; }
    public DateTime SalesDate { get; init; }
}
