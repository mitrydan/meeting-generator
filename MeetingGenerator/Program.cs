using MeetingGenerator.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MeetingGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var appConfig = configuration.GetSection("Sync").Get<SyncConfig>();

            foreach (var userConfig in appConfig.Users)
            {
                try
                {
                    var redmineProxy = new RedmineProxy(userConfig);
                    var reportService = new ReportService(userConfig, appConfig.AvailableWorkIssuesFilter);
                    var meetingGenerator = new MeetingGenerator(redmineProxy, reportService);
                    await meetingGenerator.GenerateAsync();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
