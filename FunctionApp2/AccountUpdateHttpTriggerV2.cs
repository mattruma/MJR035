using FunctionApp2.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionApp2
{
    public static class AccountUpdateHttpTriggerV2
    {
        [FunctionName("AccountUpdateHttpTriggerV2")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "v2/accounts/{accountNumber}")] HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "AzureCosmosDocumentStoreOptions:ConnectionString")] DocumentClient documentClient,
            string accountNumber,
            ILogger log)
        {
            log.LogInformation("AccountUpdateHttpTriggerV2 function processed a request.");

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

            var collectionUri =
                UriFactory.CreateDocumentCollectionUri(
                    Environment.GetEnvironmentVariable("AzureCosmosDocumentStoreOptions:DatabaseId"), "accounts");

            var document =
                documentClient
                    .CreateDocumentQuery<dynamic>(collectionUri, "SELECT TOP 1 * FROM a WHERE a.object = 'Account'", new FeedOptions { PartitionKey = new PartitionKey(accountNumber.ToString()) })
                    .AsEnumerable().FirstOrDefault();

            if (document == null)
            {
                return new NotFoundObjectResult($"Account '{accountNumber}' cannot be found.");
            }

            var okToUpdate = false;

            // Can add multiple conditions here to look for changes, or create a helper method,
            // doing this will minimize RU usage with Cosmsos

            if (document.phone != accountEntityUpdateOptions.PhoneNumber)
            {
                document.phone =
                    accountEntityUpdateOptions.PhoneNumber;
                okToUpdate = true;
            }

            if (okToUpdate)
            {
                await documentClient.UpsertDocumentAsync(
                    collectionUri, document, new RequestOptions { PartitionKey = new PartitionKey(accountNumber.ToString()) });
            }

            var accountEntity =
                new AccountEntity
                {
                    Id = new Guid(document.id.ToString()),
                    CreatedOn = Convert.ToDateTime(document.created_on),
                    AccountNumber = document.account.ToString(),
                    SystemOfRecord = document.sysofrecord.ToString(),
                    PhoneNumber = document.phone.ToString()
                };

            return new OkObjectResult(accountEntity);
        }
    }
}
