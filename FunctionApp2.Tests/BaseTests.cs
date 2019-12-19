using Bogus;
using FunctionApp2.Tests.Helpers;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace FunctionApp2.Tests
{
    public abstract class BaseTests : IAsyncLifetime
    {
        protected readonly IConfiguration _configuration;
        protected readonly CosmosClient _cosmosClient;
        protected readonly ILogger _logger = LoggerHelper.CreateLogger(LoggerTypes.List);
        protected readonly Faker _faker;

        protected BaseTests()
        {
            // NOTE: Make sure to set these files to copy to output directory

            _configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .AddJsonFile("appsettings.Development.json")
                 .Build();

            _cosmosClient = new CosmosClientBuilder(
                    _configuration["AzureCosmosDocumentStoreOptions:ConnectionString"])
                    .WithConnectionModeDirect()
                    .Build();

            _faker = new Bogus.Faker();
        }
        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            await _cosmosClient.CreateDatabaseIfNotExistsAsync(
                _configuration["AzureCosmosDocumentStoreOptions:DatabaseId"],
                throughput: 1000);

            var database =
                _cosmosClient.GetDatabase(
                    _configuration["AzureCosmosDocumentStoreOptions:DatabaseId"]);

            await database.CreateContainerIfNotExistsAsync(
                new ContainerProperties
                {
                    Id = "accounts",
                    PartitionKeyPath = "/account"
                });
        }
    }
}
