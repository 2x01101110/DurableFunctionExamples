using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FunctionChainingExample.Models
{
    public class Booking
    {
        public ActivityResult Hotel { get; set; }
        public ActivityResult Flight { get; set; }
        public ActivityResult Payment { get; set; }

        [JsonConstructor]
        public Booking(ActivityResult hotel, ActivityResult flight, ActivityResult payment)
        {
            this.Hotel = hotel;
            this.Flight = flight;
            this.Payment = payment;
        }

        public Booking()
        {

        }
    }
}
