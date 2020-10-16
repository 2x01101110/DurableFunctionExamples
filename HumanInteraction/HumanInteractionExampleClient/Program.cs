using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HumanInteractionExampleClient
{
    class Program
    {
        private static HttpClient _httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            var instance = await CreateAppointment();

            Console.WriteLine(instance.Id);

            await Task.Delay(15000);

            await ConfirmAppointment(instance);
        }

        public static async Task<Instance> CreateAppointment()
        {
            var response = await _httpClient.PostAsync("http://localhost:7071/api/appointment-manager", null);
            var contentString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Instance>(contentString);
        }

        public static async Task ConfirmAppointment(Instance instance)
        {
            var eventUrl = instance.SendEventPostUri.Replace("{eventName}", "ConfirmAppointment");
            await _httpClient.PostAsync(eventUrl, new StringContent(JsonConvert.SerializeObject(new
            {
                code = 1337
            }), Encoding.UTF8, "application/json"));
        }
    }

    public class Instance
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("statusQueryGetUri")]
        public string StatusQueryGetUri { get; set; }

        [JsonPropertyName("sendEventPostUri")]
        public string SendEventPostUri { get; set; }
    }
}
