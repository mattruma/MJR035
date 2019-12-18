using FunctionApp1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FunctionApp1
{
    public static class AccountFetchHttpTriggerV2
    {
        [FunctionName("AccountFetchHttpTriggerV2")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v2/accounts/{accountNumber}")] HttpRequest req,
            [CosmosDB("%AzureCosmosDocumentStoreOptions:DatabaseId%", "accounts",
                ConnectionStringSetting = "AzureCosmosDocumentStoreOptions:ConnectionString",
                SqlQuery = "SELECT TOP 1 * FROM a WHERE a.object = 'Account'",
                PartitionKey = "{accountNumber}")] IEnumerable<AccountData> accountDataList,
            string accountNumber,
            ILogger log)
        {
            log.LogInformation("AccountFetchHttpTriggerV2 function processed a request.");

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return new BadRequestObjectResult("Missing 'account_number' in the uri.");
            }

            if (accountDataList.Count() == 0)
            {
                return new NotFoundObjectResult($"Account '{accountNumber}' cannot be found.");
            }

            return new OkObjectResult(accountDataList.First());
        }
    }
}
