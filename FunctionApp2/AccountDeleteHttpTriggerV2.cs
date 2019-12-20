using FunctionApp2.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionApp2
{
    public static class AccountDeleteHttpTriggerV2
    {
        [FunctionName("AccountDeleteHttpTriggerV2")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "v2/accounts/{accountNumber}")] HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "AzureCosmosDocumentStoreOptions:ConnectionString")] DocumentClient documentClient,
            string accountNumber,
            ILogger log)
        {
            log.LogInformation("AccountDeleteHttpTriggerV2 function processed a request.");

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return new BadRequestObjectResult("Missing 'account_number' in the uri.");
            }

            var collectionUri =
                UriFactory.CreateDocumentCollectionUri(
                    Environment.GetEnvironmentVariable("AzureCosmosDocumentStoreOptions:DatabaseId"), "accounts");

            var accountData =
                documentClient
                    .CreateDocumentQuery<AccountData>(collectionUri, new FeedOptions { PartitionKey = new PartitionKey(accountNumber.ToString()) })
                    .Where(a => a.Object == "Account")
                    .AsEnumerable().FirstOrDefault();

            if (accountData == null)
            {
                return new NotFoundObjectResult($"Account '{accountNumber}' cannot be found.");
            }

            var documentUri =
                UriFactory.CreateDocumentUri(
                    Environment.GetEnvironmentVariable("AzureCosmosDocumentStoreOptions:DatabaseId"),
                    "accounts",
                    accountData.Id.ToString());

            await documentClient.DeleteDocumentAsync(
                documentUri, new RequestOptions { PartitionKey = new PartitionKey(accountNumber.ToString()) });

            return new NoContentResult();
        }
    }
}
