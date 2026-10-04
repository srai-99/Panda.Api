using Panda.Api.Models.Requests.Appointment;
using System.ComponentModel.DataAnnotations;

namespace Panda.Api.Validators.Appointment
{
    public static class AppointmentCreateValidator
    {
        public static void EnsureValid(AppointmentCreateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Patient))
                throw new ValidationException("Patient NHS number is required.");

            if (string.IsNullOrWhiteSpace(model.Status))
                throw new ValidationException("Status is required.");

            if (string.IsNullOrWhiteSpace(model.Duration))
                throw new ValidationException("Duration is required.");

            if (string.IsNullOrWhiteSpace(model.Clinician))
                throw new ValidationException("Clinician is required.");

            if (string.IsNullOrWhiteSpace(model.Department))
                throw new ValidationException("Department is required.");

            if (string.IsNullOrWhiteSpace(model.Postcode))
                throw new ValidationException("Postcode is required.");
        }
    }
}