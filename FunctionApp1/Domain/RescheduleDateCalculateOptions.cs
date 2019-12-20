using System;

namespace FunctionApp1.Domain
{
    public class RescheduleDateCalculateOptions
    {
        public DateTime? NextPaymentDate { get; set; }

        public string PostalCode { get; set; }
    }
}
