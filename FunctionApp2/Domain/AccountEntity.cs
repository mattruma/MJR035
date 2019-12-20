using System;

namespace FunctionApp2.Domain
{
    public class AccountEntity
    {
        public Guid Id { get; set; }

        public string AccountNumber { get; set; }

        public string SystemOfRecord { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
