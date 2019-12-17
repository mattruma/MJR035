using Newtonsoft.Json;

namespace ClassLibrary1
{
    public class AccountEntityUpdateOptions
    {
        [JsonProperty("system_of_record")]
        public string SystemOfRecord { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
