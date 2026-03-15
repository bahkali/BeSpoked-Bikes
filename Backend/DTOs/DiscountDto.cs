namespace backend.DTOs;

public record DiscountDto
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public DateTime BeginDate { get; init; }
    public DateTime EndDate { get; init; }
    public decimal DiscountPercentage { get; init; }
}
