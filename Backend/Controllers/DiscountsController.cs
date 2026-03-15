using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountsController : ControllerBase
{
    private readonly BeSpokedContext _context;

    public DiscountsController(BeSpokedContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<DiscountDto>>> GetAll()
    {
        var discounts = await _context.Discounts
            .Include(d => d.Product)
            .OrderBy(d => d.BeginDate)
            .Select(d => new DiscountDto
            {
                Id = d.Id,
                ProductId = d.ProductId,
                ProductName = d.Product.Name,
                BeginDate = d.BeginDate,
                EndDate = d.EndDate,
                DiscountPercentage = d.DiscountPercentage
            })
            .ToListAsync();

        return Ok(discounts);
    }
}
