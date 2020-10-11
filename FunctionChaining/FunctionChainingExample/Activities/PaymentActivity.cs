using FunctionChainingExample.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Collections.Generic;

namespace FunctionChainingExample.Activities
{
    public class PaymentActivity
    {
        [FunctionName(nameof(PaymentActivity))]
        public static ActivityResult Run([ActivityTrigger] IDurableActivityContext context)
        {
            var input = context.GetInput<Dictionary<string, object>>();

            if (Guid.Parse(input["ClientId"].ToString()) == Guid.Parse("5a308090-1ea9-4f3f-bfb6-892adfe51c02"))
            {
                throw new Exception("Test exception");
            }

            return ActivityResult.Fulfilled(nameof(PaymentActivity), new Payment
            {
                PaymentId = Guid.NewGuid()
            });
        }
    }
}
