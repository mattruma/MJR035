using Newtonsoft.Json;
using System;

namespace ClassLibrary1
{
    public abstract class BaseEntity
    {
        [JsonProperty("id", Order = -4)]
        public Guid Id { get; set; }

        [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}