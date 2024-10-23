using System.Collections.Generic;

namespace MeetingGenerator.Config
{
    internal class SyncConfig
    {
        public IReadOnlyCollection<SyncConfigUser> Users { get; set; }

        public IReadOnlyCollection<string> AvailableWorkIssuesFilter { get; set; }
    }
}
