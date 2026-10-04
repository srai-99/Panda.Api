namespace Panda.Domain.Entities;

public class Patient
{
    public string NhsNumber { get; init; } = default!;
    public string Name { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public string Postcode { get; set; } = default!;
}