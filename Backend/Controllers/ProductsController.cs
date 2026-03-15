using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly BeSpokedContext _context;

    public ProductsController(BeSpokedContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var products = await _context.Products
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Manufacturer = p.Manufacturer,
                Style = p.Style,
                PurchasePrice = p.PurchasePrice,
                SalePrice = p.SalePrice,
                QtyOnHand = p.QtyOnHand,
                CommissionPercentage = p.CommissionPercentage
            })
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        return Ok(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Manufacturer = product.Manufacturer,
            Style = product.Style,
            PurchasePrice = product.PurchasePrice,
            SalePrice = product.SalePrice,
            QtyOnHand = product.QtyOnHand,
            CommissionPercentage = product.CommissionPercentage
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        var duplicate = await _context.Products.AnyAsync(p =>
            p.Id != id && p.Name == dto.Name && p.Manufacturer == dto.Manufacturer);
        if (duplicate) return Conflict("A product with this Name and Manufacturer already exists.");

        product.Name = dto.Name;
        product.Manufacturer = dto.Manufacturer;
        product.Style = dto.Style;
        product.PurchasePrice = dto.PurchasePrice;
        product.SalePrice = dto.SalePrice;
        product.QtyOnHand = dto.QtyOnHand;
        product.CommissionPercentage = dto.CommissionPercentage;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
