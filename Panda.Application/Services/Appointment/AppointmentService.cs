using System.ComponentModel.DataAnnotations;
using Panda.Domain.Enums;
using Panda.Domain.Utilities;

namespace Panda.Application.Services.Appointment;

public class AppointmentService : IAppointmentService
{
    public void ApplyBusinessRules(Domain.Entities.Appointment appt)
    {
        if (appt.Status is AppointmentStatus.Cancelled or AppointmentStatus.Missed)
            return;

        var minutes = DurationParser.ParseMinutes(appt.Duration);
        var endTime = appt.Time.AddMinutes(minutes);

        if (endTime < DateTimeOffset.UtcNow &&
            appt.Status == AppointmentStatus.Active)
        {
            appt.Status = AppointmentStatus.Missed;
        }
    }

    public void Cancel(Domain.Entities.Appointment appt)
    {
        switch (appt.Status)
        {
            case AppointmentStatus.Cancelled:
                throw new ValidationException("Appointment is already cancelled.");

            case AppointmentStatus.Missed:
                throw new ValidationException("Missed appointments cannot be cancelled.");

            case AppointmentStatus.Attended:
                throw new ValidationException("Attended appointments cannot be cancelled.");
        }

        appt.Status = AppointmentStatus.Cancelled;
    }
}