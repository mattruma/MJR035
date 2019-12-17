using Newtonsoft.Json;

namespace ClassLibrary1
{
    public class AccountData : BaseData
    {
        public const string SYSTEMOFRECORD_AUTOSUITE = "AutoSuite";
        public const string SYSTEMOFRECORD_ISERIES = "iSeries";
        public const string SYSTEMOFRECORD_FISERVE = "Fiserve";
        public const string SYSTEMOFRECORD_LEASEMASTER = "LeaseMaster";

        public override string Object => "Account";

        [JsonProperty("account")]
        public string AccountNumber { get; internal set; }

        [JsonProperty("sysofrecord")]
        public string SystemOfRecord { get; set; }

        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }

        public AccountData()
        {
        }

        public AccountData(
            string accountNumber)
        {
            this.AccountNumber = accountNumber;
        }
    }
}
