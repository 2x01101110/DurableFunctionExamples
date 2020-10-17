using DurableTask.Core.Stats;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Threading.Tasks;

namespace StatefulEntityExample.Orchestrators
{
    public class CounterOrchestrator
    {
        [FunctionName(nameof(CounterOrchestrator))]
        public Task Run([OrchestrationTrigger]IDurableOrchestrationContext context)
        {
            //var entity = new EntityId(nameof(Counter), "myCounter");

            return Task.CompletedTask;
        }
    }
}
