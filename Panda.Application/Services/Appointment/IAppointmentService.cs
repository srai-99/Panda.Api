namespace Panda.Application.Services.Appointment
{
    public interface IAppointmentService
    {
        void ApplyBusinessRules(Domain.Entities.Appointment appt);

        void Cancel(Domain.Entities.Appointment appt);
    }
}