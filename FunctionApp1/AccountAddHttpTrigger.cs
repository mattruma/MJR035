using ClassLibrary1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class AccountAddHttpTrigger
    {
        private IAccountEntityService _accountService;

        public AccountAddHttpTrigger(
            IAccountEntityService accountService)
        {
            _accountService = accountService;
        }

        [FunctionName("AccountAddHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] AccountEntityAddOptions accountAddOptions,
            ILogger log)
        {
            log.LogInformation("AccountAddHttpTrigger function processed a request.");

            _accountService.AddAsync(
                accountAddOptions);

            return new StatusCodeResult((int)HttpStatusCode.Created);
        }
    }
}
