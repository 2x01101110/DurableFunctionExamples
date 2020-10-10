using FunctionChainingExample.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunctionChainingExample.Activities
{
    public static class BookHotelActivityCompensate
    {
        [FunctionName(nameof(BookHotelActivityCompensate))]
        public static Task Run([ActivityTrigger]IDurableActivityContext activityContext, ILogger log)
        {
            var hotel = activityContext.GetInput<Hotel>();

            log.LogInformation($"Instace {activityContext.InstanceId} executing {nameof(BookHotelActivityCompensate)} for hotel {hotel.BookingId}");

            return Task.CompletedTask;
        }
    }
}
