using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Twilio.Rest.Api.V2010.Account;

namespace HumanInteractionExample.Activities
{
    public class AppointmentConfirmationChallengeActivity
    {
        [FunctionName(nameof(AppointmentConfirmationChallengeActivity))]
        public int Run(
            [TwilioSms(
                AccountSidSetting = "TwilioAccountSid", 
                AuthTokenSetting = "AuthTokenSetting")]
            out CreateMessageOptions message,
            [ActivityTrigger]IDurableActivityContext context, 
            ILogger log)
        {
            int challengeCode = 123456;

            message = new CreateMessageOptions(new Twilio.Types.PhoneNumber("+"));
            message.From = new Twilio.Types.PhoneNumber("+12157068968");
            message.Body = $"Test {challengeCode}";

            return challengeCode;
        }
    }
}
