using MeetingGenerator.Config;
using MeetingGenerator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MeetingGenerator
{
    internal class ReportService
    {
        private readonly SyncConfigUser _configuration;
        private readonly IReadOnlyCollection<string> _availableWorkIssuesFilter;

        public ReportService(SyncConfigUser configuration, IReadOnlyCollection<string> availableWorkIssuesFilter)
        {
            _configuration = configuration;
            _availableWorkIssuesFilter = availableWorkIssuesFilter;
        }

        public string CreateReport(IEnumerable<TimeEntry> timeEntries, IEnumerable<WorkIssue> availableWorkIssues)
        {
            var previousDateTime = timeEntries
                .Where(x => x.Project.Id == _configuration.RedmineProjectId)
                .Max(x => x.SpentOn);
            var formattedTimeEntries = timeEntries
                .Where(x => x.Project.Id == _configuration.RedmineProjectId)
                .Where(x => x.SpentOn == previousDateTime)
                .Select(x => $"#{x.Issue.Id}: {x.Comments}");
            var formattedAvailableWorkIssues = availableWorkIssues
                .Where(x => _availableWorkIssuesFilter.Contains(x.Status.Name))
                .Where(x => x.Project.Id == _configuration.RedmineProjectId)
                .Select(x => $"#{x.Id}: {x.Subject}");

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("```");
            stringBuilder.AppendLine($"*{_configuration.ReporterName}*");
            stringBuilder.AppendLine("* *Сделано*:");
            stringBuilder.AppendJoin(Environment.NewLine, formattedTimeEntries);
            if (formattedTimeEntries.Any())
            { 
                stringBuilder.Append(Environment.NewLine);
            } 
            stringBuilder.AppendLine("* *Планируется сделать*:");
            stringBuilder.AppendJoin(Environment.NewLine, formattedAvailableWorkIssues);
            if (formattedAvailableWorkIssues.Any())
            {
                stringBuilder.Append(Environment.NewLine);
            }
            else
            {
                stringBuilder.Append("Взять согласоно приоритету после митинга.");
                stringBuilder.Append(Environment.NewLine);
            }
            stringBuilder.AppendLine("* *Проблемы*: нет");
            stringBuilder.AppendLine("```");

            return stringBuilder.ToString();
        }

        public async Task SaveReportAsync(string report)
        {
            Console.WriteLine("Try to send meeting to Mattermost");
            Console.WriteLine(report);

            var body = new { channel_id = _configuration.MattermostChannelId, message = report };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, _configuration.MattermostUrl);
            httpRequest.Headers.Add("Authorization", $"Bearer {_configuration.MattermostAccessToken}");
            httpRequest.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(httpRequest);

            Console.WriteLine("Meeting has been sent to Mattermost");
        }
    }
}
