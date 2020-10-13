using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunctionChainingExample.Activities;
using FunctionChainingExample.Models;
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
            var input = context.GetInput<BookingRequest>();

            Booking booking = new Booking();
            int successfullSteps = 0;

            var bookingActivities = new List<Activity>
            {
                new Activity 
                { 
                    Name = nameof(BookHotelActivity),
                    Input = new Dictionary<string, object>
                    {
                        { "ClientId", input.ClientId },
                        { "DealId", input.DealId }
                    }
                },
                new Activity
                {
                    Name = nameof(BookFlightActivity),
                    Input = new Dictionary<string, object>
                    {
                        { "DealId", input.DealId }
                    }
                },
                new Activity
                {
                    Name =  nameof(PaymentActivity),
                    Input = new Dictionary<string, object>
                    {
                        { "ClientId", input.ClientId }
                    }
                }
            };

            foreach (var activity in bookingActivities)
            {
                try
                {
                    activity.Input.Add("Booking", booking);

                    var result = await ActivityFactory(context, activity.Name, activity.Input);

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
                bookingActivities = bookingActivities.GetRange(0, successfullSteps);
                bookingActivities.Reverse();

                foreach (var activity in bookingActivities)
                {
                    var result = await ActivityFactory(context, $"{activity.Name}Compensate", activity.Input);

                    UpdateBooking(booking, result, result.ActivityName);
                }
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
                case nameof(PaymentActivity):
                    booking.Payment = activityResult;
                    break;
                default:
                    throw new Exception($"Activity {activityName} was not found and activity result could not be saved");
            }
        }

        private Task<ActivityResult> ActivityFactory(
            IDurableOrchestrationContext context, 
            string activityName,
            Dictionary<string, object> input)
        {
            return context.CallActivityAsync<ActivityResult>(activityName, input);
        }
    }
}
