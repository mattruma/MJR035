using FunctionApp1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class AccountDeleteHttpTrigger
    {
        private IAccountDataStore _accountDataStore;

        public AccountDeleteHttpTrigger(
            IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        [FunctionName("AccountDeleteHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "accounts/{accountNumber}")] HttpRequest req,
            string accountNumber,
            ILogger log)
        {
            log.LogInformation("AccountDeleteHttpTrigger function processed a request.");


            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return new BadRequestObjectResult("Missing 'account_number' in the uri.");
            }


            var accountData =
                await _accountDataStore.FetchByAccountNumberAsync(
                    accountNumber);

            if (accountData == null)
            {
                return new NotFoundObjectResult($"Account '{accountNumber}' cannot be found.");
            }

            await _accountDataStore.DeleteByIdAsync(
                accountData.Id,
                accountData.AccountNumber);

            return new NoContentResult();
        }
    }
}
