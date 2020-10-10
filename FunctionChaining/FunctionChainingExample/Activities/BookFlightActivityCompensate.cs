using FunctionChainingExample.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunctionChainingExample.Activities
{
    //public static class BookFlightActivityCompensate
    //{
    //    [FunctionName(nameof(BookFlightActivityCompensate))]
    //    public static Task Run([ActivityTrigger]IDurableActivityContext activityContext, ILogger log)
    //    {
    //        var flight = activityContext.GetInput<Flight>();

    //        log.LogInformation($"Instace {activityContext.InstanceId} executing {nameof(BookFlightActivityCompensate)} for flight {flight.FlightId}");

    //        return Task.CompletedTask;
    //    }
    //}
}
