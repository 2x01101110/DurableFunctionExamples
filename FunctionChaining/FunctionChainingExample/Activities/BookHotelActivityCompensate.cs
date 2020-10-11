using FunctionChainingExample.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionChainingExample.Activities
{
    public static class BookHotelActivityCompensate
    {
        [FunctionName(nameof(BookHotelActivityCompensate))]
        public static ActivityResult Run([ActivityTrigger]IDurableActivityContext context, ILogger log)
        {
            var input = context.GetInput<Dictionary<string, object>>();

            var booking = JsonConvert.DeserializeObject<Booking>(input["Booking"].ToString());

            booking.Hotel.InvalidateActionResult("Activity was compensated");

            return booking.Hotel;
        }
    }
}
