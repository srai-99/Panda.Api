using Panda.Api.Models.Requests.Patient;
using System.ComponentModel.DataAnnotations;

namespace Panda.Api.Validators.Patient
{
    public static class PatientRequestValidator
    {
        public static void EnsureValid(PatientCreateModel model)
        {
            if (model is null)
                throw new ValidationException("Request body is required.");

            if (string.IsNullOrWhiteSpace(model.NhsNumber))
                throw new ValidationException("NHS number is required.");

            if (model.NhsNumber.Length > 10)
                throw new ValidationException("NHS number cannot exceed 10 characters.");

            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ValidationException("Name is required.");

            if (model.Name.Length > 200)
                throw new ValidationException("Name cannot exceed 200 characters.");

            if (string.IsNullOrWhiteSpace(model.Postcode))
                throw new ValidationException("Postcode is required.");

            if (model.Postcode.Length > 10)
                throw new ValidationException("Postcode cannot exceed 10 characters.");
        }
    }
}