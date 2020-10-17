using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using MonitoringExample.Activities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringExample.Orchestrators
{
    public class MonitoringOrchestrator
    {
        [FunctionName(nameof(MonitoringOrchestrator))]
        public async Task Run([OrchestrationTrigger]IDurableOrchestrationContext context, ILogger log)
        {
            log = context.CreateReplaySafeLogger(log);

            // Real life scenario would be to create a new report every day at midnight
            // DateTime endTime = context.CurrentUtcDateTime.AddDays(1).Date;

            DateTime reportCreationTime = context.CurrentUtcDateTime.AddSeconds(15);
            var reportCreationTimer = context.CreateTimer(reportCreationTime, CancellationToken.None);

            var reportingCancelationRequest = context.WaitForExternalEvent("CancelReportingTask");

            var completedTask = await Task.WhenAny(reportCreationTimer, reportingCancelationRequest);

            if (completedTask == reportCreationTimer)
            {
                await context.CallActivityAsync(nameof(ReportCreationActivity), null);

                log.LogInformation("Starting a new instance cycle");
                // Eternal orchestration until canceled
                context.ContinueAsNew(null);
            }
            else
            {
                log.LogInformation("Reporting task cancelation was requested");
            }
        }
    }
}
