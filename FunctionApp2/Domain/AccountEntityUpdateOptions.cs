using Newtonsoft.Json;

namespace FunctionApp2.Domain
{
    public class AccountEntityUpdateOptions
    {
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
