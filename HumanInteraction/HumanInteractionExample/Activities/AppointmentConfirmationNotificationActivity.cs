using HumanInteractionExample.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;

namespace HumanInteractionExample.Activities
{
    public class AppointmentConfirmationNotificationActivity
    {
        [FunctionName(nameof(AppointmentConfirmationNotificationActivity))]
        public ConfirmationCode Run(
            //[TwilioSms(AccountSidSetting = "AccountSidSetting", AuthTokenSetting = "AuthTokenSetting")]out CreateMessageOptions message,
            [ActivityTrigger]IDurableActivityContext context,
            ILogger logger)
        {
            string phoneNumber = context.GetInput<string>();

            var random = new Random();
            //var appointmentConfirmationCode = random.Next(1000, 9999);
            var appointmentConfirmationCode = 1337;

            logger.LogInformation($"{context.InstanceId} sending appointment confirmation code {appointmentConfirmationCode} to {phoneNumber}");

            /*message = new CreateMessageOptions(new Twilio.Types.PhoneNumber(phoneNumber));
            message.From = new Twilio.Types.PhoneNumber("");
            message.Body = $"To confirm scheduled appointment reply to this message with {appointmentConfirmationCode} code.";*/

            return new ConfirmationCode
            { 
                Code = appointmentConfirmationCode
            };
        }
    }
}
