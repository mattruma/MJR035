using FunctionApp2.Data;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FunctionApp2.Startup))]
namespace FunctionApp2
{
    public class Startup : FunctionsStartup
    {
        // https://dotnetcoretutorials.com/2017/03/25/net-core-dependency-injection-lifetimes-explained/

        // Transient Lifetime
        // If in doubt, make it transient. Adding a transient service means that each time the service is requested, a new instance is created.

        // Singleton Lifetime
        // A singleton is an instance that will last the entire lifetime of the application.

        // Scoped Lifetime
        // Scoped lifetime actually means that within a created “scope” objects will be the same instance.

        public override void Configure(
            IFunctionsHostBuilder builder)
        {
            var cosmosClient =
                new CosmosClientBuilder(
                       Environment.GetEnvironmentVariable("AzureCosmosDocumentStoreOptions:ConnectionString"))
                   .WithConnectionModeDirect()
                   .Build();

            builder.Services.AddSingleton(
                new AccountDataStoreOptions(Environment.GetEnvironmentVariable("AzureCosmosDocumentStoreOptions:DatabaseId"),
                cosmosClient));

            builder.Services.AddSingleton<IAccountDataStore, AccountDataStore>();
        }
    }
}
