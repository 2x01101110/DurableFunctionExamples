using System;

namespace FunctionChainingExample.Models
{
    public class BookingRequest
    {
        public Guid ClientId { get; set; }
        public Guid DealId { get; set; }
        public Guid PaymentId { get; set; }
    }
}
