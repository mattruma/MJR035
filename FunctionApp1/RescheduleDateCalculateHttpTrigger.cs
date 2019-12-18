using FunctionApp1.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public static class RescheduleDateCalculateHttpTrigger
    {
        [FunctionName("RescheduleDateCalculateHttpTrigger")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "payments/calculate/rescheduledate")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("RescheduleDateCalculateHttpTrigger function processed a request.");

            var rescheduleDateCalculateOptions =
                JsonConvert.DeserializeObject<RescheduleDateCalculateOptions>(
                    await new StreamReader(req.Body).ReadToEndAsync());

            if (rescheduleDateCalculateOptions == null)
            {
                return new BadRequestObjectResult("'next_payment_date' and 'zip_code' are required and must be sent in the body of the request.");
            }

            var nextPaymentDate =
                DateTime.Today.AddDays(2);

            if (rescheduleDateCalculateOptions.NextPaymentDate.HasValue
                && !string.IsNullOrWhiteSpace(rescheduleDateCalculateOptions.PostalCode))
            {
                switch (rescheduleDateCalculateOptions.PostalCode)
                {
                    case "85026":
                        nextPaymentDate =
                            rescheduleDateCalculateOptions.NextPaymentDate.Value.AddDays(2);
                        break;
                    case "75014":
                        nextPaymentDate =
                            rescheduleDateCalculateOptions.NextPaymentDate.Value.AddDays(3);
                        break;
                    case "75063":
                        nextPaymentDate =
                            rescheduleDateCalculateOptions.NextPaymentDate.Value.AddDays(4);
                        break;
                    case "76014":
                        nextPaymentDate =
                            rescheduleDateCalculateOptions.NextPaymentDate.Value.AddDays(5);
                        break;
                    default:
                        break;
                }
            }

            return new OkObjectResult(nextPaymentDate);
        }
    }
}
