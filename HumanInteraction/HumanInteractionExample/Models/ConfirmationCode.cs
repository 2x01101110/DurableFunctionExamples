using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HumanInteractionExample.Models
{
    public class ConfirmationCode
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }
}
