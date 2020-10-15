using HumanInteractionExample.Orchestrators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
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
            Random random = new Random();
            string appointmentIdentifer = random.Next(1000, 9999).ToString();
            string instanceIdentifer = $"+37124426622-{appointmentIdentifer}";

            string instanceId = await durableClient.StartNewAsync(
                nameof(AppointmentSchedulingOrchestrator),
                instanceIdentifer, 
                appointmentIdentifer);

            return durableClient.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
