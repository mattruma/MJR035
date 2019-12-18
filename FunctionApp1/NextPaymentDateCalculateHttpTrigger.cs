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
    public static class NextPaymentDateCalculateHttpTrigger
    {
        [FunctionName("NextPaymentDateCalculateHttpTrigger")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "payments/calculate/nextpaymentdate")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("NextPaymentDateCalculateOptions function processed a request.");

            var rescheduleDateCalculateOptions =
                JsonConvert.DeserializeObject<RescheduleDateCalculateOptions>(
                    await new StreamReader(req.Body).ReadToEndAsync());

            if (rescheduleDateCalculateOptions == null)
            {
                return new BadRequestObjectResult("'next_payment_date' is required and must be sent in the body of the request.");
            }

            var nextPaymentDate =
                DateTime.Today.AddDays(2);

            if (rescheduleDateCalculateOptions.NextPaymentDate.HasValue)
            {
                nextPaymentDate =
                    rescheduleDateCalculateOptions.NextPaymentDate.Value.AddDays(2);
            }


            return new OkObjectResult(nextPaymentDate);
        }
    }
}
