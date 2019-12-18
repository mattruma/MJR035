using Newtonsoft.Json;
using System;

namespace FunctionApp1.Domain
{
    public class RescheduleDateCalculateOptions
    {
        [JsonProperty("next_payment_date")]
        public DateTime? NextPaymentDate { get; set; }

        [JsonProperty("zip_code")]
        public string PostalCode { get; set; }
    }
}
