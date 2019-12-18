using FunctionApp1.Data;
using FunctionApp1.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class AccountUpdateHttpTrigger
    {
        private IAccountDataStore _accountDataStore;

        public AccountUpdateHttpTrigger(
            IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        [FunctionName("AccountUpdateHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "accounts/{accountNumber}")] HttpRequest req,
            string accountNumber,
            ILogger log)
        {
            log.LogInformation("AccountUpdateHttpTrigger function processed a request.");

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return new BadRequestObjectResult("Missing 'account_number' in the uri.");
            }

            var accountEntityUpdateOptions =
                JsonConvert.DeserializeObject<AccountEntityUpdateOptions>(
                    await new StreamReader(req.Body).ReadToEndAsync());

            if (accountEntityUpdateOptions == null)
            {
                return new BadRequestObjectResult("Missing request body.");
            }

            var accountData =
                await _accountDataStore.FetchByAccountNumberAsync(
                    accountNumber);

            var okToUpdate = false;

            // Can add multiple conditions here to look for changes, or create a helper method,
            // doing this will minimize RU usage with Cosmsos

            if (accountData.PhoneNumber != accountEntityUpdateOptions.PhoneNumber)
            {
                accountData.PhoneNumber =
                    accountEntityUpdateOptions.PhoneNumber;
                okToUpdate = true;
            }

            if (okToUpdate)
            {
                await _accountDataStore.UpdateByIdAsync(
                    accountData.Id,
                    accountData.AccountNumber,
                    accountData);
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
