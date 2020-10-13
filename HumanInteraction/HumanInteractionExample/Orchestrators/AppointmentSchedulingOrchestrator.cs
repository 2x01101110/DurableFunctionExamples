using HumanInteractionExample.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HumanInteractionExample.Orchestrators
{
    public class AppointmentSchedulingOrchestrator
    {
        [FunctionName(nameof(AppointmentSchedulingOrchestrator))]
        public async Task Run([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            int challengeCode = await context.CallActivityAsync<int>(nameof(AppointmentConfirmationChallengeActivity), null);
            //await context.CallActivityAsync<int>(nameof(AppointmentConfirmationChallengeActivity), null);

            using CancellationTokenSource source = new CancellationTokenSource();

            Task timeoutTask = context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(20), source.Token);

            Task<object> challengeResponseTask = context.WaitForExternalEvent<dynamic>("AppointmentConfirmationEvent");

            Task completedTask = await Task.WhenAny(challengeResponseTask, timeoutTask);

            if (completedTask == challengeResponseTask)
            {
                log.LogInformation(JsonConvert.SerializeObject(challengeResponseTask.Result));
            }
            else
            {
                log.LogInformation("Timed out");
            }

            source.Cancel();
        }
    }
}
