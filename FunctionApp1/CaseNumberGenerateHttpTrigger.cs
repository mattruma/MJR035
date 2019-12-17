using ClassLibrary1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class CaseNumberGenerateHttpTrigger
    {
        private ICaseNumberGenerate _caseNumberGenerate;

        public CaseNumberGenerateHttpTrigger(
            ICaseNumberGenerate caseNumberGenerate)
        {
            _caseNumberGenerate = caseNumberGenerate;
        }

        [FunctionName("CaseNumberGenerateHttpTrigger")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("CaseNumberGenerateHttpTrigger function processed a request.");

            var caseNumberGenerated =
                _caseNumberGenerate.Generate();

            return new OkObjectResult(caseNumberGenerated);
        }
    }
}
