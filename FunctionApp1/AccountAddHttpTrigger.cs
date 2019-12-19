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
    // https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-http-endpoint
    // https://marcelzehner.ch/2019/02/05/issues-with-creating-new-cosmos-db-documents-with-logic-app/
    // https://docs.microsoft.com/en-us/azure/logic-apps/manage-logic-apps-with-visual-studio
    // https://marketplace.visualstudio.com/items?itemName=VinaySinghMSFT.AzureLogicAppsToolsForVS2019

    public class AccountAddHttpTrigger
    {
        private IAccountDataStore _accountDataStore;

        public AccountAddHttpTrigger(
            IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        [FunctionName("AccountAddHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "accounts")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("AccountAddHttpTrigger function processed a request.");

            var accountEntityAddOptions =
                JsonConvert.DeserializeObject<AccountEntityAddOptions>(
                    await new StreamReader(req.Body).ReadToEndAsync());

            if (accountEntityAddOptions == null)
            {
                return new BadRequestObjectResult("Missing request body.");
            }

            if (string.IsNullOrWhiteSpace(accountEntityAddOptions.AccountNumber))
            {
                return new BadRequestObjectResult("'account_number' is required.");
            }

            if (string.IsNullOrWhiteSpace(accountEntityAddOptions.SystemOfRecord))
            {
                return new BadRequestObjectResult("'system_of_record' is required.");
            }

            var accountData =
                new AccountData
                {
                    AccountNumber = accountEntityAddOptions.AccountNumber,
                    SystemOfRecord = accountEntityAddOptions.SystemOfRecord,
                    PhoneNumber = accountEntityAddOptions.PhoneNumber
                };

            await _accountDataStore.AddAsync(
                accountData);

            var accountEntity =
                new AccountEntity
                {
                    Id = accountData.Id,
                    CreatedOn = accountData.CreatedOn,
                    AccountNumber = accountData.AccountNumber,
                    SystemOfRecord = accountData.SystemOfRecord,
                    PhoneNumber = accountData.PhoneNumber
                };

            return new CreatedResult($"/accounts/{accountEntity.Id}", accountEntity);
        }
    }
}
