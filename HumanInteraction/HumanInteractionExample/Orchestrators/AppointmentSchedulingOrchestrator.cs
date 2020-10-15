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
            string appointmentIdentifer = context.GetInput<string>();

            await context.CallActivityAsync(nameof(AppointmentConfirmationActivity), appointmentIdentifer);

            var result = await context.WaitForExternalEvent<dynamic>("CancelAppointment");

            if (result != null)
            {
                log.LogInformation($"{result.From} {result.code}");
                await context.CallActivityAsync(nameof(AppointmentCancelationActivity), null);
            }

            #region Commented Out
            //await context.CallActivityAsync<int>(nameof(AppointmentConfirmationChallengeActivity), null);

            //using CancellationTokenSource source = new CancellationTokenSource();

            //Task timeoutTask = context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(20), source.Token);

            //Task<object> challengeResponseTask = context.WaitForExternalEvent<dynamic>("AppointmentConfirmationEvent");

            //Task completedTask = await Task.WhenAny(challengeResponseTask, timeoutTask);

            //if (completedTask == challengeResponseTask)
            //{
            //    log.LogInformation(JsonConvert.SerializeObject(challengeResponseTask.Result));
            //}
            //else
            //{
            //    log.LogInformation("Timed out");
            //}

            //source.Cancel();

            #endregion
        }


    }
}
