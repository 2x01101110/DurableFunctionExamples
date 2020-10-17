using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using MonitoringExample.Orchestrators;
using System.Threading.Tasks;

namespace MonitoringExample
{
    public class MonitoringManager
    {
        [FunctionName(nameof(MonitoringManager))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "monitoring-manager")]HttpRequest req,
            [DurableClient]IDurableClient durableClient)
        {
            var instance = await durableClient.StartNewAsync(nameof(MonitoringOrchestrator));

            return durableClient.CreateCheckStatusResponse(req, instance);
        }
    }
}
