using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Salesperson
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime? TerminationDate { get; set; }

    [Required, MaxLength(100)]
    public string Manager { get; set; } = string.Empty;

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
