using Dynamitey.DynamicObjects;
using HumanInteractionExample.Activities;
using HumanInteractionExample.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
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
            log = context.CreateReplaySafeLogger(log);

            var appointment = context.GetInput<ClientAppointment>();

            // Create confirmation code for appointment, send it to client, and return code as result
            var confirmationCode = await context
                .CallActivityAsync<ConfirmationCode>(nameof(AppointmentConfirmationNotificationActivity), appointment.PhoneNumber);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var confirmationExpiration = context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(20), cancellationTokenSource.Token);
            var appointmentConfirmation = context.WaitForExternalEvent<ConfirmationCode>("ConfirmAppointment");

            var tasks = new List<Task> { appointmentConfirmation, confirmationExpiration };

            // Give client X ammount of time to confirm appointment
            do
            {
                var result = await Task.WhenAny(tasks);

                if (result == appointmentConfirmation)
                {
                    if (appointmentConfirmation.Result.Code == confirmationCode.Code)
                    {
                        log.LogInformation($"Appointment {appointment.AppointmentId} confirmed");
                        break;
                    }
                }
                if (result == confirmationExpiration)
                {
                    log.LogWarning($"Appointment {appointment.AppointmentId} failed to be confirmed");
                    cancellationTokenSource.Cancel();
                }

            } while (!cancellationTokenSource.IsCancellationRequested);

            if (!cancellationTokenSource.IsCancellationRequested)
            {
                var appointmentCancelation = context.WaitForExternalEvent("CancelAppointment");
                var appointmentReminder = context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(15), cancellationTokenSource.Token);
                // AFter this time expires, we assume that appointment is either currently happening or already happened
                var instanceExpiration = context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(25), cancellationTokenSource.Token);

                tasks = new List<Task> { appointmentCancelation, appointmentReminder, instanceExpiration };

                // Wait for appointment reminder/cancelation task completions
                do
                {
                    var result = await Task.WhenAny(tasks);

                    if (result == appointmentCancelation)
                    {
                        log.LogWarning($"Instance {context.InstanceId} canceling appointment");
                        cancellationTokenSource.Cancel();
                    }
                    else if (result == appointmentReminder)
                    {
                        log.LogInformation("Sending appponintment reminder");
                        tasks.Remove(appointmentReminder);
                    }
                    else if (result == instanceExpiration)
                    {
                        cancellationTokenSource.Cancel();
                    }
                }
                while (!cancellationTokenSource.IsCancellationRequested);
            }
        }
    }
}
