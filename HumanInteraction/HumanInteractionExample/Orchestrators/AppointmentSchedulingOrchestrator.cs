using Dynamitey.DynamicObjects;
using HumanInteractionExample.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HumanInteractionExample.Orchestrators
{
    public class AppointmentSchedulingOrchestrator
    {
        [FunctionName(nameof(AppointmentSchedulingOrchestrator))]
        public async Task Run([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            using CancellationTokenSource source = new CancellationTokenSource();

            //var task1 = await context.WaitForExternalEvent("ApproveAppointment", new System.TimeSpan(0, 0, 5), default(string), source.Token);

            //if (task1 == default)
            //{
            //    source.Cancel();
            //}

            if (!source.IsCancellationRequested)
            {
                var cancelAppointment = context.WaitForExternalEvent("CancelAppointment", new System.TimeSpan(0, 0, 10), default(string), source.Token);
                var remindAppointment = context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(15), source.Token);

                var tasks = new List<Task> { cancelAppointment, remindAppointment };

                do
                {
                    var result = await Task.WhenAny(tasks);

                    if (result == cancelAppointment)
                    {
                        log.LogInformation("Canceling appponintment");
                        source.Cancel();
                    }
                    else if (result == remindAppointment)
                    {
                        log.LogInformation("Sending appponintment reminder");
                        tasks.Remove(remindAppointment);
                    }
                } 
                while (source.IsCancellationRequested != true);
            }


            //string appointmentIdentifer = context.GetInput<string>();

            //await context.CallActivityAsync(nameof(AppointmentConfirmationActivity), appointmentIdentifer);

            //var result = await context.WaitForExternalEvent<dynamic>("CancelAppointment");

            //if (result != null)
            //{
            //    log.LogInformation($"{result.From} {result.code}");
            //    await context.CallActivityAsync(nameof(AppointmentCancelationActivity), null);
            //}

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
