using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using FunctionChainingExample.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FunctionChainingExample.Activities
{
    public class BookFlightActivity
    {
        [FunctionName(nameof(BookFlightActivity))]
        public static ActivityResult Run([ActivityTrigger] IDurableActivityContext context)
        {
            var input = context.GetInput<Dictionary<string, object>>();

            var booking = JsonConvert.DeserializeObject<Booking>(input["Booking"].ToString());

            return ActivityResult.Fulfilled(nameof(BookFlightActivity), new Flight
            {
                FlightId = Guid.NewGuid()
            });
        }
    }
}
