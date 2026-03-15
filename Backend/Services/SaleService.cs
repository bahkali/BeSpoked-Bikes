using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Services;

public interface ISaleService
{
    Task<(Sale? sale, string? error)> CreateSaleAsync(int productId, int salespersonId, int customerId, DateTime salesDate);
}

public class SaleService : ISaleService
{
    private readonly BeSpokedContext _context;

    public SaleService(BeSpokedContext context)
    {
        _context = context;
    }

    public async Task<(Sale? sale, string? error)> CreateSaleAsync(
        int productId, int salespersonId, int customerId, DateTime salesDate)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) return (null, "Product not found");
        if (product.QtyOnHand <= 0) return (null, "Product is out of stock");

        var salesperson = await _context.Salespersons.FindAsync(salespersonId);
        if (salesperson == null) return (null, "Salesperson not found");
        if (salesperson.TerminationDate.HasValue && salesperson.TerminationDate.Value < salesDate)
            return (null, "Salesperson was terminated before the sale date");

        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null) return (null, "Customer not found");

        var discount = await _context.Discounts
            .Where(d => d.ProductId == productId && d.BeginDate <= salesDate && d.EndDate >= salesDate)
            .OrderByDescending(d => (double)d.DiscountPercentage)
            .FirstOrDefaultAsync();

        var price = product.SalePrice;
        if (discount != null)
            price = price * (1 - discount.DiscountPercentage / 100);

        var commission = price * (product.CommissionPercentage / 100);

        var sale = new Sale
        {
            ProductId = productId,
            SalespersonId = salespersonId,
            CustomerId = customerId,
            SalesDate = salesDate,
            Price = Math.Round(price, 2),
            SalespersonCommission = Math.Round(commission, 2)
        };

        product.QtyOnHand -= 1;

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        return (sale, null);
    }
}
