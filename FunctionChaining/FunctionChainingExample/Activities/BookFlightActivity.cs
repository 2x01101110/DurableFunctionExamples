using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using FunctionChainingExample.Models;

namespace FunctionChainingExample.Activities
{
    public class BookFlightActivity
    {
        [FunctionName(nameof(BookFlightActivity))]
        public static ActivityResult Run([ActivityTrigger] IDurableActivityContext context)
        {
            var booking = context.GetInput<Booking>();

            return ActivityResult.Fulfilled(nameof(BookFlightActivity), new Flight
            {
                FlightId = Guid.NewGuid()
            });
        }
    }
}
