namespace MeetingGenerator.Config
{
    internal class SyncConfigUser
    {
        public string RedmineUrl { get; set; }

        public string RedmineApiKey { get; set; }

        public int RedmineProjectId { get; set; }

        public string ReporterName { get; set; }

        public string MattermostUrl { get; set; }

        public int MattermostChannelId { get; set; }

        public string MattermostAccessToken { get; set; }
    }
}
