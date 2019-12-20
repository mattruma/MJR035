using Newtonsoft.Json;
using System;

namespace FunctionApp2.Data
{
    public class AccountData
    {
        public const string SYSTEMOFRECORD_AUTOSUITE = "AutoSuite";
        public const string SYSTEMOFRECORD_ISERIES = "iSeries";
        public const string SYSTEMOFRECORD_FISERVE = "Fiserve";
        public const string SYSTEMOFRECORD_LEASEMASTER = "LeaseMaster";

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("object")]
        public string Object => "Account";

        [JsonProperty("account")]
        public string AccountNumber { get; internal set; }

        [JsonProperty("sysofrecord")]
        public string SystemOfRecord { get; set; }

        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }

        [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }

        public AccountData()
        {
            this.Id = Guid.NewGuid();
            this.CreatedOn = DateTime.UtcNow;
        }

        public AccountData(
            string accountNumber) : this()
        {
            this.AccountNumber = accountNumber;
        }
    }
}
