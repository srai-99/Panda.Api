using System.Text.Json.Serialization;

namespace Panda.Api.Models.Requests.Patient
{
    public class PatientCreateModel
    {
        [JsonPropertyName("nhs_number")]
        public string NhsNumber { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("date_of_birth")]
        public DateOnly DateOfBirth { get; set; }

        [JsonPropertyName("postcode")]
        public string Postcode { get; set; } = null!;
    }
}