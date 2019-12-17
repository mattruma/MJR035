using Microsoft.Azure.Cosmos;
using System;
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
                new PartitionKey(accountData.AccountId.ToString()));
        }

        public async Task DeleteByIdAsync(
            Guid id)
        {
            await _cosmosContainer.DeleteItemAsync<AccountData>(
                id.ToString(),
                new PartitionKey(id.ToString()));
        }

        public async Task<AccountData> FetchByIdAsync(
            Guid id)
        {
            var accountDataResponse =
                await _cosmosContainer.ReadItemAsync<AccountData>(
                    id.ToString(),
                    new PartitionKey(id.ToString()));

            return accountDataResponse.Resource;
        }

        public async Task UpdateByIdAsync(
            Guid id,
            AccountData accountData)
        {
            var accountDataResponse =
                await _cosmosContainer.ReplaceItemAsync(
                    accountData,
                    id.ToString(),
                    new PartitionKey(id.ToString()));
        }
    }
}