using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dynamitey;
using Dynamitey.DynamicObjects;
using FunctionChainingExample.Activities;
using FunctionChainingExample.Models;
using ImpromptuInterface.Optimization;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace FunctionChainingExample.Orchestrators
{
    public class VacationBookingOrchestrator
    {
        [FunctionName(nameof(VacationBookingOrchestrator))]
        public async Task<Booking> Run([OrchestrationTrigger]IDurableOrchestrationContext context, ILogger log)
        {
            Booking booking = new Booking();
            int successfullSteps = 0;

            var bookingActivities = new List<string>
            {
                nameof(BookHotelActivity),
                nameof(BookFlightActivity)
            };

            foreach (var activity in bookingActivities)
            {
                try
                {
                    var result = await ActivityFactory(context, activity, booking);

                    if (!result.CompletedSuccessfuly) break;

                    UpdateBooking(booking, result, result.ActivityName);
                    successfullSteps++;
                }
                catch (Exception)
                {
                    break;
                }
            }

            if (successfullSteps != bookingActivities.Count && successfullSteps - 1 > 0)
            {
                bookingActivities = bookingActivities.GetRange(0, successfullSteps - 1);
                bookingActivities.Reverse();

                foreach (var activity in bookingActivities)
                {
                    await ActivityFactory(context, $"{nameof(activity)}Compensate", booking);
                }

                booking = null;
            }

            return booking;
        }

        private void UpdateBooking(Booking booking, ActivityResult activityResult, string activityName)
        {
            switch (activityName)
            {
                case nameof(BookHotelActivity):
                    booking.Hotel = activityResult;
                    break;
                case nameof(BookFlightActivity):
                    booking.Flight = activityResult;
                    break;
            }
        }

        private Task<ActivityResult> ActivityFactory(IDurableOrchestrationContext context, string activityName, Booking booking)
        {
            return context.CallActivityAsync<ActivityResult>(activityName, booking);
        }
    }
}
