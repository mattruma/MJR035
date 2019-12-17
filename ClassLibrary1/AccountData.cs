using Newtonsoft.Json;
using System;

namespace ClassLibrary1
{
    public class AccountData : BaseData
    {
        public const string SYSTEMOFRECORD_AUTOSUITE = "AutoSuite";
        public const string SYSTEMOFRECORD_ISERIES = "iSeries";
        public const string SYSTEMOFRECORD_FISERVE = "Fiserve";
        public const string SYSTEMOFRECORD_LEASEMASTER = "LeaseMaster";

        public override string Object => "Account";

        [JsonProperty("account_id")]
        public Guid AccountId => this.Id;

        [JsonProperty("account")]
        public string AccountNumber { get; set; }

        [JsonProperty("sysofrecord")]
        public string SystemOfRecord { get; set; }

        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
    }
}
