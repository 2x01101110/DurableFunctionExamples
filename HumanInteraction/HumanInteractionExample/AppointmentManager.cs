using HumanInteractionExample.Models;
using HumanInteractionExample.Orchestrators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HumanInteractionExample
{
    public class AppointmentManager
    {
        [FunctionName(nameof(AppointmentManager))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route ="appointment-manager")]HttpRequest req,
            [DurableClient]IDurableClient durableClient,
            ILogger log)
        {
            var clientAppointment = new ClientAppointment
            {
                ClientId = Guid.NewGuid(),
                AppointmentId = Guid.NewGuid(),
                PhoneNumber = "+00011112222"
            };

            string instanceId = await durableClient.StartNewAsync(nameof(AppointmentSchedulingOrchestrator), clientAppointment);

            return durableClient.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
