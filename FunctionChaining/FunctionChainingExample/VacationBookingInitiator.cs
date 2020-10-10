using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using FunctionChainingExample.Orchestrators;

namespace FunctionChainingExample
{
    public static class VacationBookingInitiator
    {
        [FunctionName("VacationBookingInitiator")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req,
            [DurableClient]IDurableClient durableClient,
            ILogger log)
        {
            string instanceId = await durableClient.StartNewAsync(nameof(VacationBookingOrchestrator));

            return durableClient.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
