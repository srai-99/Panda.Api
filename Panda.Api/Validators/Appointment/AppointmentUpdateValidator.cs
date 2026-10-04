using Panda.Api.Models.Requests.Appointment;
using System.ComponentModel.DataAnnotations;

namespace Panda.Api.Validators.Appointment
{
    public static class AppointmentUpdateValidator
    {
        public static void EnsureValid(AppointmentUpdateModel model)
        {
            if (model.Status is not null &&
                string.IsNullOrWhiteSpace(model.Status))
            {
                throw new ValidationException("Status cannot be empty.");
            }

            if (model.Time.HasValue &&
                model.Time.Value == default)
            {
                throw new ValidationException("Start time is invalid.");
            }

            if (model.Duration is not null &&
                string.IsNullOrWhiteSpace(model.Duration))
            {
                throw new ValidationException("Duration cannot be empty.");
            }

            if (model.Clinician is not null &&
                string.IsNullOrWhiteSpace(model.Clinician))
            {
                throw new ValidationException("Clinician cannot be empty.");
            }

            if (model.Department is not null &&
                string.IsNullOrWhiteSpace(model.Department))
            {
                throw new ValidationException("Department cannot be empty.");
            }

            if (model.Postcode is not null &&
                string.IsNullOrWhiteSpace(model.Postcode))
            {
                throw new ValidationException("Postcode cannot be empty.");
            }
        }
    }
}