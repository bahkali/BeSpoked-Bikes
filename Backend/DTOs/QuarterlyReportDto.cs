namespace backend.DTOs;

public record CommissionReportDto
{
    public int SalespersonId { get; init; }
    public string SalespersonName { get; init; } = string.Empty;
    public int TotalSalesCount { get; init; }
    public decimal TotalSalesAmount { get; init; }
    public decimal TotalCommission { get; init; }
}

public record QuarterlyReportDto
{
    public int Year { get; init; }
    public int Quarter { get; init; }
    public List<CommissionReportDto> Salespersons { get; init; } = new();
}
