using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using FunctionChainingExample.Models;
using System;
using Dynamitey;

namespace FunctionChainingExample.Activities
{
    public class BookHotelActivity
    {
        [FunctionName(nameof(BookHotelActivity))]
        public static ActivityResult Run([ActivityTrigger]IDurableActivityContext context)
        {
            return ActivityResult.Fulfilled(nameof(BookHotelActivity), new Hotel
            {
                BookingId = Guid.NewGuid()
            });
        }
    }
}
