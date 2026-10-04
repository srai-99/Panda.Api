using Panda.Domain.Enums;

namespace Panda.Domain.Entities;

public class Appointment
{
    public Guid Id { get; set; }

    public string PatientNhsNumber { get; set; } = null!;

    public AppointmentStatus Status { get; set; }

    public DateTimeOffset Time { get; set; }

    public string Duration { get; set; } = null!;

    public string Clinician { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string Postcode { get; set; } = null!;
}