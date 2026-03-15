namespace backend.DTOs;

public record SalespersonDto
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime? TerminationDate { get; init; }
    public string Manager { get; init; } = string.Empty;
}

public record UpdateSalespersonDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime? TerminationDate { get; init; }
    public string Manager { get; init; } = string.Empty;
}
