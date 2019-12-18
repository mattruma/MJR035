using Newtonsoft.Json;

namespace FunctionApp1.Domain
{
    public class AccountEntityAddOptions
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("system_of_record")]
        public string SystemOfRecord { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
