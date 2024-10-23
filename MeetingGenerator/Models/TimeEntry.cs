using Newtonsoft.Json;
using System;

namespace MeetingGenerator.Models
{
    public class TimeEntry
    {
        public long Id { get; set; }

        public Project Project { get; set; }

        public Issue Issue { get; set; }

        public int Hours { get; set; }

        public string Comments { get; set; }

        [JsonProperty("spent_on")]
        public DateTime SpentOn { get; set; }
    }
}
