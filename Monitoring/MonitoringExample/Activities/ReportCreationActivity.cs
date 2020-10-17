using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Threading.Tasks;

namespace MonitoringExample.Activities
{
    public class ReportCreationActivity
    {
        [FunctionName(nameof(ReportCreationActivity))]
        public async Task Run([ActivityTrigger]IDurableActivityContext context)
        {
            await ReportService.CreateReport();
        }

        /// <summary>
        /// Some sort of a service containing logic to create a report
        /// </summary>
        public static class ReportService
        {
            /// <summary>
            /// Hits the data source/external service and creates a report
            /// </summary>
            /// <returns></returns>
            public static async Task CreateReport()
            {
                await Task.Delay((new Random()).Next(1000, 3000));
            }
        }
    }
}
