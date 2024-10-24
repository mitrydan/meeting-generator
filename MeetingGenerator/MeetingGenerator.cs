using System.Threading.Tasks;

namespace MeetingGenerator
{
    internal class MeetingGenerator
    {
        private readonly RedmineProxy _redmineProxy;
        private readonly ReportService _reportService;

        public MeetingGenerator(RedmineProxy redmineProxy, ReportService reportService)
        {
            _redmineProxy = redmineProxy;
            _reportService = reportService;
        }

        public async Task GenerateAsync()
        {
            var workItems = await _redmineProxy.GetWorkItemsInfo();
            var report = _reportService.CreateReport(workItems.timeEntries, workItems.workIssues);
            await _reportService.SaveReportAsync(report);
        }
    }
}
