using Microsoft.Azure.Cosmos;

namespace FunctionApp1.Data
{
    public class AccountDataStoreOptions
    {
        public string DatabaseId { get; set; }
        public CosmosClient CosmosClient { get; set; }

        public AccountDataStoreOptions(
            string databaseId,
            CosmosClient cosmosClient)
        {
            this.DatabaseId = databaseId;
            this.CosmosClient = cosmosClient;
        }
    }
}
