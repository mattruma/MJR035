using FunctionApp1.Data;
using FunctionApp1.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class AccountFetchHttpTrigger
    {
        private IAccountDataStore _accountDataStore;

        public AccountFetchHttpTrigger(
            IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        [FunctionName("AccountFetchHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "accounts/{accountNumber}")] HttpRequest req,
            string accountNumber,
            ILogger log)
        {
            log.LogInformation("AccountFetchHttpTrigger function processed a request.");


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

            var accountEntity =
                new AccountEntity
                {
                    Id = accountData.Id,
                    CreatedOn = accountData.CreatedOn,
                    AccountNumber = accountData.AccountNumber,
                    SystemOfRecord = accountData.SystemOfRecord,
                    PhoneNumber = accountData.PhoneNumber
                };

            return new OkObjectResult(accountEntity);
        }
    }
}
