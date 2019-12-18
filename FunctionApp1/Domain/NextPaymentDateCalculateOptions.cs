using Newtonsoft.Json;
using System;

namespace FunctionApp1.Domain
{
    public class NextPaymentDateCalculateOptions
    {
        [JsonProperty("next_payment_date")]
        public DateTime? NextPaymentDate { get; set; }
    }
}
