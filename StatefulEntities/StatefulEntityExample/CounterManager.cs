using DurableTask.Core.Stats;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace StatefulEntityExample
{
    public class CounterManager
    {
        [FunctionName(nameof(CounterManager))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "counter-manager")] HttpRequest req,
            [DurableClient]IDurableEntityClient client)
        {
            var entityId = new EntityId(nameof(Counter), "myCounter5");

            await client.SignalEntityAsync(entityId, "Add", 100);

            var state = await client.ReadEntityStateAsync<JObject>(entityId);

            return new OkObjectResult(state.EntityState);
        }
    }
}
