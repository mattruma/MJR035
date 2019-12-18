using Newtonsoft.Json;
using System;

namespace FunctionApp1.Data
{
    public abstract class BaseData
    {
        [JsonProperty("id", Order = -4)]
        public Guid Id { get; set; }

        [JsonProperty("object", Order = -3)]
        public abstract string Object { get; }

        [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }

        protected BaseData()
        {
            this.Id = Guid.NewGuid();
            this.CreatedOn = DateTime.UtcNow;
        }
    }
}