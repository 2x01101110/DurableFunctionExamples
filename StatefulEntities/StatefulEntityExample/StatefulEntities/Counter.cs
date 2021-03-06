﻿using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace StatefulEntityExample.StatefulEntities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Counter
    {
        [JsonProperty("value")]
        public int CurrentValue { get; set; }

        public void Add(int amount) => this.CurrentValue += amount;
        public void Reset() => this.CurrentValue = 0;
        public int Get() => this.CurrentValue;

        [FunctionName(nameof(Counter))]
        public static Task Run([EntityTrigger] IDurableEntityContext context) => context.DispatchAsync<Counter>();
    }
}
