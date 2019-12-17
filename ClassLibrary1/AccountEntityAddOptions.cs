using Newtonsoft.Json;

namespace ClassLibrary1
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
