using FunctionChainingExample.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace FunctionChainingExample.Activities
{
    public class PaymentActivityCompensate
    {
        [FunctionName(nameof(PaymentActivityCompensate))]
        public static ActivityResult Run([ActivityTrigger] IDurableActivityContext context)
        {
            var booking = context.GetInput<Booking>();

            booking.Payment.InvalidateActionResult("Activity was compensated");

            return booking.Payment;
        }
    }
}
