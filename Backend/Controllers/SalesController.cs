using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly BeSpokedContext _context;
    private readonly ISaleService _saleService;

    public SalesController(BeSpokedContext context, ISaleService saleService)
    {
        _context = context;
        _saleService = saleService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SaleDto>>> GetAll(
        [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var query = _context.Sales
            .Include(s => s.Product)
            .Include(s => s.Salesperson)
            .Include(s => s.Customer)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(s => s.SalesDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(s => s.SalesDate <= endDate.Value);

        var sales = await query
            .OrderByDescending(s => s.SalesDate)
            .Select(s => new SaleDto
            {
                Id = s.Id,
                ProductId = s.ProductId,
                ProductName = s.Product.Name,
                SalespersonId = s.SalespersonId,
                SalespersonName = s.Salesperson.FirstName + " " + s.Salesperson.LastName,
                CustomerId = s.CustomerId,
                CustomerName = s.Customer.FirstName + " " + s.Customer.LastName,
                SalesDate = s.SalesDate,
                Price = s.Price,
                SalespersonCommission = s.SalespersonCommission
            })
            .ToListAsync();

        return Ok(sales);
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> Create(CreateSaleDto dto)
    {
        var (sale, error) = await _saleService.CreateSaleAsync(
            dto.ProductId, dto.SalespersonId, dto.CustomerId, dto.SalesDate);

        if (error != null) return BadRequest(error);

        await _context.Entry(sale!).Reference(s => s.Product).LoadAsync();
        await _context.Entry(sale!).Reference(s => s.Salesperson).LoadAsync();
        await _context.Entry(sale!).Reference(s => s.Customer).LoadAsync();

        var result = new SaleDto
        {
            Id = sale!.Id,
            ProductId = sale.ProductId,
            ProductName = sale.Product.Name,
            SalespersonId = sale.SalespersonId,
            SalespersonName = sale.Salesperson.FirstName + " " + sale.Salesperson.LastName,
            CustomerId = sale.CustomerId,
            CustomerName = sale.Customer.FirstName + " " + sale.Customer.LastName,
            SalesDate = sale.SalesDate,
            Price = sale.Price,
            SalespersonCommission = sale.SalespersonCommission
        };

        return CreatedAtAction(nameof(GetAll), new { id = sale.Id }, result);
    }

    [HttpGet("quarterly-report")]
    public async Task<ActionResult<QuarterlyReportDto>> QuarterlyReport(
        [FromQuery] int year, [FromQuery] int quarter)
    {
        if (quarter < 1 || quarter > 4)
            return BadRequest("Quarter must be between 1 and 4.");

        var startMonth = (quarter - 1) * 3 + 1;
        var startDate = new DateTime(year, startMonth, 1);
        var endDate = startDate.AddMonths(3).AddDays(-1);

        var salespersons = await _context.Sales
            .Where(s => s.SalesDate >= startDate && s.SalesDate <= endDate)
            .GroupBy(s => new { s.SalespersonId, s.Salesperson.FirstName, s.Salesperson.LastName })
            .Select(g => new CommissionReportDto
            {
                SalespersonId = g.Key.SalespersonId,
                SalespersonName = g.Key.FirstName + " " + g.Key.LastName,
                TotalSalesCount = g.Count(),
                TotalSalesAmount = g.Sum(s => s.Price),
                TotalCommission = g.Sum(s => s.SalespersonCommission)
            })
            .OrderByDescending(r => r.TotalCommission)
            .ToListAsync();

        return Ok(new QuarterlyReportDto
        {
            Year = year,
            Quarter = quarter,
            Salespersons = salespersons
        });
    }
}
