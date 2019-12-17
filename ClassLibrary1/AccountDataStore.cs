using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary1
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

        public async Task<AccountData> FetchByIdAsync(
            Guid id,
            string accountNumber)
        {
            var accountDataResponse =
                await _cosmosContainer.ReadItemAsync<AccountData>(
                    id.ToString(),
                    new PartitionKey(accountNumber));

            return accountDataResponse.Resource;
        }

        public async Task<AccountData> FetchByAccountNumberAsync(
            string accountNumber)
        {
            var queryDefinition =
                new QueryDefinition(
                    "SELECT TOP 1 * FROM t1 WHERE t1.object = 'Account'");

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