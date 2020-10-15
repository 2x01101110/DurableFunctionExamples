using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using Twilio.Rest.Api.V2010.Account;

namespace HumanInteractionExample.Activities
{
    public class AppointmentConfirmationActivity
    {
        [FunctionName(nameof(AppointmentConfirmationActivity))]
        public void Run(
            [TwilioSms(
                AccountSidSetting = "AccountSidSetting", 
                AuthTokenSetting = "AuthTokenSetting")]
            out CreateMessageOptions message,
            [ActivityTrigger]IDurableActivityContext context, 
            ILogger log)
        {
            string appointmentIdentifer = context.GetInput<string>();

            message = new CreateMessageOptions(new Twilio.Types.PhoneNumber("+"));
            message.From = new Twilio.Types.PhoneNumber("+");
            message.Body = $"Appointment scheduled. To cancel send {appointmentIdentifer} code as reply.";
        }
    }
}
