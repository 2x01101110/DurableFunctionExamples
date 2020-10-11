using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using FunctionChainingExample.Orchestrators;
using System;
using FunctionChainingExample.Models;

namespace FunctionChainingExample
{
    public static class VacationBookingInitiator
    {
        [FunctionName("VacationBookingInitiator")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "book-vacation/{id}")]HttpRequest req,
            [DurableClient]IDurableClient durableClient,
            ILogger log,
            Guid id)
        {
            object parameters = new BookingRequest
            {
                ClientId = id,
                PaymentId = Guid.NewGuid(),
                DealId = Guid.NewGuid()
            };

            string instanceId = await durableClient.StartNewAsync(nameof(VacationBookingOrchestrator), input: parameters);

            return durableClient.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
