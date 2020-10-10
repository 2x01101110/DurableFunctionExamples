using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FunctionChainingExample.Models
{
    public class Booking
    {
        public ActivityResult Hotel { get; set; }
        public ActivityResult Flight { get; set; }

        public IEnumerable<bool> SuccessfullActivities()
        {
            yield return this.Hotel.CompletedSuccessfuly;
            yield return this.Flight.CompletedSuccessfuly;
        }
    }
}
