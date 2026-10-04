using System.Text.Json.Serialization;

namespace Panda.Api.Models.Requests.Appointment
{
    public class AppointmentUpdateModel
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("time")]
        public DateTimeOffset? Time { get; set; }

        [JsonPropertyName("duration")]
        public string? Duration { get; set; }

        [JsonPropertyName("clinician")]
        public string? Clinician { get; set; }

        [JsonPropertyName("department")]
        public string? Department { get; set; } 

        [JsonPropertyName("postcode")]
        public string? Postcode { get; set; }
    }
}