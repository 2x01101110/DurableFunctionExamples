using System;

namespace HumanInteractionExample.Models
{
    public class ClientAppointment
    {
        public Guid AppointmentId { get; set; }
        public Guid ClientId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
