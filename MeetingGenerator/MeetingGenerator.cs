using System;
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
            await _redmineProxy.GetWorkItemsInfo()
                .ContinueWith(t => _reportService.CreateReport(t.Result.timeEntries, t.Result.workIssues))
                .ContinueWith(t => _reportService.SaveReportAsync(t.Result))
                .ContinueWith(CheckErrors);
        }

        private void CheckErrors(Task task)
        {
            if (task.IsFaulted)
            {
                throw new ApplicationException(task.Exception.Message);
            }
        }
    }
}
