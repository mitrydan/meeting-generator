using MeetingGenerator.Config;
using MeetingGenerator.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MeetingGenerator
{
    internal class RedmineProxy
    {
        private readonly SyncConfigUser _configuration;

        public RedmineProxy(SyncConfigUser configuration)
        {
            _configuration = configuration;
        }

        public async Task<(IEnumerable<TimeEntry> timeEntries, IEnumerable<WorkIssue> workIssues)> GetWorkItemsInfo()
        {
            var getTimeEntriesTask = GetTimeEntries();
            var getAvailableWorkIssuesTask = GetAvailableWorkIssues();
            await Task.WhenAll(getTimeEntriesTask, getAvailableWorkIssuesTask);

            return (getTimeEntriesTask.Result, getAvailableWorkIssuesTask.Result);
        }

        private async Task<IEnumerable<TimeEntry>> GetTimeEntries()
        {
            var request = $"{_configuration.RedmineUrl}/time_entries.json?user_id=me&key={_configuration.RedmineApiKey}";
            var response = await GetAsync(request);
            
            return response["time_entries"].ToObject<IEnumerable<TimeEntry>>();
        }

        private async Task<IEnumerable<WorkIssue>> GetAvailableWorkIssues()
        {
            var request = $"{_configuration.RedmineUrl}/issues.json?assigned_to_id=me&key={_configuration.RedmineApiKey}";
            var response = await GetAsync(request);
            
            return response["issues"].ToObject<IEnumerable<WorkIssue>>();
        }

        private async Task<JObject> GetAsync(string request)
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            
            return JObject.Parse(responseString);
        }
    }
}
