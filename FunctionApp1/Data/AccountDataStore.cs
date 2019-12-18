using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionApp1.Data
{
    public class AccountDataStore : IAccountDataStore
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _cosmosContainer;

        public AccountDataStore(
            AccountDataStoreOptions accountDataStoreOptions)
        {
            _cosmosClient =
                accountDataStoreOptions.CosmosClient;

            var cosmosDatabase =
               _cosmosClient.GetDatabase(accountDataStoreOptions.DatabaseId);

            _cosmosContainer =
               cosmosDatabase.GetContainer("accounts");
        }

        public async Task AddAsync(
            AccountData accountData)
        {
            await _cosmosContainer.CreateItemAsync(
                accountData,
                new PartitionKey(accountData.AccountNumber.ToString()));
        }

        public async Task DeleteByIdAsync(
            Guid id,
            string accountNumber)
        {
            await _cosmosContainer.DeleteItemAsync<AccountData>(
                id.ToString(),
                new PartitionKey(accountNumber));
        }

        // https://github.com/Azure/azure-cosmos-dotnet-v3/blob/master/Microsoft.Azure.Cosmos.Samples/Usage/Queries/Program.cs

        public async Task<AccountData> FetchByIdAsync(
            Guid id)
        {
            var queryDefinition =
                new QueryDefinition(
                    "SELECT TOP 1 * FROM a WHERE a.object = @object and a.id = @id");

            queryDefinition.WithParameter("@object", "Account");
            queryDefinition.WithParameter("@id", id.ToString());

            var feedIterator =
                _cosmosContainer.GetItemQueryIterator<AccountData>(
                    queryDefinition);

            var accountDataList =
                new List<AccountData>();

            while (feedIterator.HasMoreResults)
            {
                accountDataList.AddRange(
                    await feedIterator.ReadNextAsync());
            };

            if (accountDataList.Count == 0)
            {
                return null;
            }

            return
                accountDataList.First();
        }

        public async Task<AccountData> FetchByAccountNumberAsync(
            string accountNumber)
        {
            var queryDefinition =
                new QueryDefinition(
                    "SELECT TOP 1 * FROM a WHERE a.object = @object");

            queryDefinition.WithParameter("@object", "Account");

            var feedIterator =
                _cosmosContainer.GetItemQueryIterator<AccountData>(
                    queryDefinition,
                    requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(accountNumber) });

            var accountDataList =
                new List<AccountData>();

            while (feedIterator.HasMoreResults)
            {
                accountDataList.AddRange(
                    await feedIterator.ReadNextAsync());
            };

            if (accountDataList.Count == 0)
            {
                return null;
            }

            return
                accountDataList.First();
        }

        public async Task UpdateByIdAsync(
            Guid id,
            string accountNumber,
            AccountData accountData)
        {
            var accountDataResponse =
                await _cosmosContainer.ReplaceItemAsync(
                    accountData,
                    id.ToString(),
                    new PartitionKey(accountNumber));
        }
    }
}