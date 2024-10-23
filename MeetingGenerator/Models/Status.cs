using System.Text.Json.Serialization;

namespace MeetingGenerator.Models
{
    public class Status
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [JsonPropertyName("is_closed")]
        public bool IsClosed { get; set; }
    }
}
