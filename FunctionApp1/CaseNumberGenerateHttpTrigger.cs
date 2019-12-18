using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;

namespace FunctionApp1
{
    public class CaseNumberGenerateHttpTrigger
    {
        private static readonly Random _random = new Random();

        [FunctionName("CaseNumberGenerateHttpTrigger")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "cases/number/generate")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("CaseNumberGenerateHttpTrigger function processed a request.");

            var caseNumberGenerated =
                _random.Next(10000, 999999);

            return new OkObjectResult(caseNumberGenerated);
        }
    }
}
