using System.Text.Json.Serialization;

namespace Panda.Api.Models.Requests.Appointment
{
    public class AppointmentCreateModel
    {
        [JsonPropertyName("patient")]
        public string Patient { get; set; } = null!;

        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;

        [JsonPropertyName("time")]
        public DateTimeOffset Time { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; } = null!;

        [JsonPropertyName("clinician")]
        public string Clinician { get; set; } = null!;

        [JsonPropertyName("department")]
        public string Department { get; set; } = null!;

        [JsonPropertyName("postcode")]
        public string Postcode { get; set; } = null!;
    }
}