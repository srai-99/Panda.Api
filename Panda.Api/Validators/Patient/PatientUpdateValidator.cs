using Panda.Api.Models.Requests.Patient;
using System.ComponentModel.DataAnnotations;

namespace Panda.Api.Validators.Patient
{
    public static class PatientUpdateValidator
    {
        public static void EnsureValid(PatientUpdateModel model)
        {
            if (model.Name is not null &&
                string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ValidationException("Name cannot be empty.");
            }

            if (model.Postcode is not null &&
                string.IsNullOrWhiteSpace(model.Postcode))
            {
                throw new ValidationException("Postcode cannot be empty.");
            }
        }
    }
}