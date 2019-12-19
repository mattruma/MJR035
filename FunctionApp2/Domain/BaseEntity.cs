using Newtonsoft.Json;
using System;

namespace FunctionApp2.Domain
{
    public abstract class BaseEntity
    {
        [JsonProperty("id", Order = -3)]
        public Guid Id { get; set; }

        [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}