using Newtonsoft.Json;

namespace FunctionApp1.Domain
{
    public class AccountEntityUpdateOptions
    {
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
