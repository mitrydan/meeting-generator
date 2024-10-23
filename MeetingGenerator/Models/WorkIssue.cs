namespace MeetingGenerator.Models
{
    public class WorkIssue
    {
        public long Id { get; set; }

        public Project Project { get; set; }

        public string Subject { get; set; }

        public Status Status { get; set; }
    }
}
