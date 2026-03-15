using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalespersonsController : ControllerBase
{
    private readonly BeSpokedContext _context;

    public SalespersonsController(BeSpokedContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<SalespersonDto>>> GetAll()
    {
        var salespersons = await _context.Salespersons
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .Select(s => new SalespersonDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Address = s.Address,
                Phone = s.Phone,
                StartDate = s.StartDate,
                TerminationDate = s.TerminationDate,
                Manager = s.Manager
            })
            .ToListAsync();

        return Ok(salespersons);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SalespersonDto>> GetById(int id)
    {
        var s = await _context.Salespersons.FindAsync(id);
        if (s == null) return NotFound();

        return Ok(new SalespersonDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Address = s.Address,
            Phone = s.Phone,
            StartDate = s.StartDate,
            TerminationDate = s.TerminationDate,
            Manager = s.Manager
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateSalespersonDto dto)
    {
        var salesperson = await _context.Salespersons.FindAsync(id);
        if (salesperson == null) return NotFound();

        var duplicate = await _context.Salespersons.AnyAsync(s =>
            s.Id != id && s.FirstName == dto.FirstName && s.LastName == dto.LastName && s.Phone == dto.Phone);
        if (duplicate) return Conflict("A salesperson with this name and phone already exists.");

        salesperson.FirstName = dto.FirstName;
        salesperson.LastName = dto.LastName;
        salesperson.Address = dto.Address;
        salesperson.Phone = dto.Phone;
        salesperson.StartDate = dto.StartDate;
        salesperson.TerminationDate = dto.TerminationDate;
        salesperson.Manager = dto.Manager;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
