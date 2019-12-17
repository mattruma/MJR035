using Newtonsoft.Json;
using System;

namespace ClassLibrary1
{
    public class AccountAddOptions
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("system_of_record")]
        public string SystemOfRecord { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
