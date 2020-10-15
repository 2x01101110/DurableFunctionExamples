using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Twilio.Rest.Api.V2010.Account;

namespace HumanInteractionExample.Activities
{
    public class AppointmentCancelationActivity
    {
        [FunctionName(nameof(AppointmentCancelationActivity))]
        public void Run(
            [TwilioSms(
                AccountSidSetting = "AccountSidSetting",
                AuthTokenSetting = "AuthTokenSetting")]
            out CreateMessageOptions message,
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log)
        {
            string appointmentIdentifer = context.GetInput<string>();

            message = new CreateMessageOptions(new Twilio.Types.PhoneNumber("+"));
            message.From = new Twilio.Types.PhoneNumber("+");
            message.Body = $"Appointment {appointmentIdentifer} has been canceled!";
        }
    }
}
