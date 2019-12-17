using FluentAssertions;
using Microsoft.Azure.Cosmos;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ClassLibrary1.Tests
{
    public class AccountDataStoreTests : BaseTests
    {
        [Fact]
        public async Task When_AddAsync()
        {
            // Arrange

            var accountData =
                new AccountData
                {
                    AccountNumber =
                        _faker.Random.Number(10000, 99999).ToString(),
                    SystemOfRecord =
                        _faker.PickRandom(
                            AccountData.SYSTEMOFRECORD_AUTOSUITE,
                            AccountData.SYSTEMOFRECORD_FISERVE,
                            AccountData.SYSTEMOFRECORD_ISERIES,
                            AccountData.SYSTEMOFRECORD_LEASEMASTER),
                    PhoneNumber =
                        _faker.Person.Phone,
                };

            var accountDataStoreOptions =
                new AccountDataStoreOptions(
                    _configuration["AzureCosmosDocumentStoreOptions:DatabaseId"],
                    _cosmosClient);

            var accountDataStore =
                new AccountDataStore(
                    accountDataStoreOptions);

            // Act

            await accountDataStore.AddAsync(
                accountData);

            // Assert

            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    accountDataStoreOptions.DatabaseId);

            var cosmosContainer =
               cosmosDatabase.GetContainer(
                   "accounts");

            var accountDataResponse =
                await cosmosContainer.ReadItemAsync<AccountData>(
                    accountData.Id.ToString(),
                        new PartitionKey(accountData.AccountId.ToString()));

            accountDataResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_DeleteByIdAsync()
        {
            // Arrange

            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    _configuration["AzureCosmosDocumentStoreOptions:DatabaseId"]);

            var cosmosContainer =
               cosmosDatabase.GetContainer("accounts");

            var accountData =
                new AccountData
                {
                    AccountNumber =
                        _faker.Random.Number(10000, 99999).ToString(),
                    SystemOfRecord =
                        _faker.PickRandom(
                            AccountData.SYSTEMOFRECORD_AUTOSUITE,
                            AccountData.SYSTEMOFRECORD_FISERVE,
                            AccountData.SYSTEMOFRECORD_ISERIES,
                            AccountData.SYSTEMOFRECORD_LEASEMASTER),
                    PhoneNumber =
                        _faker.Person.Phone,
                };

            await cosmosContainer.CreateItemAsync(
                accountData,
                new PartitionKey(accountData.AccountId.ToString()));

            var accountDataStoreOptions =
                new AccountDataStoreOptions(
                    _configuration["AzureCosmosDocumentStoreOptions:DatabaseId"],
                    _cosmosClient);

            var accountDataStore =
                new AccountDataStore(
                    accountDataStoreOptions);

            // Act

            await accountDataStore.DeleteByIdAsync(
                accountData.Id);

            // Assert

            Func<Task> action = async () =>
                await cosmosContainer.ReadItemAsync<AccountData>(
                    accountData.Id.ToString(),
                        new PartitionKey(accountData.AccountId.ToString()));

            action.Should().Throw<CosmosException>();
        }

        [Fact]
        public async Task When_FetchByIdAsync()
        {
            // Arrange

            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    _configuration["AzureCosmosDocumentStoreOptions:DatabaseId"]);

            var cosmosContainer =
               cosmosDatabase.GetContainer("accounts");

            var accountData =
                new AccountData
                {
                    AccountNumber =
                        _faker.Random.Number(10000, 99999).ToString(),
                    SystemOfRecord =
                        _faker.PickRandom(
                            AccountData.SYSTEMOFRECORD_AUTOSUITE,
                            AccountData.SYSTEMOFRECORD_FISERVE,
                            AccountData.SYSTEMOFRECORD_ISERIES,
                            AccountData.SYSTEMOFRECORD_LEASEMASTER),
                    PhoneNumber =
                        _faker.Person.Phone,
                };

            await cosmosContainer.CreateItemAsync(
                accountData,
                new PartitionKey(accountData.AccountId.ToString()));

            var accountDataStoreOptions =
                new AccountDataStoreOptions(
                    _configuration["AzureCosmosDocumentStoreOptions:DatabaseId"],
                    _cosmosClient);

            var accountDataStore =
                new AccountDataStore(
                    accountDataStoreOptions);

            // Act

            var accountDataFetched = await
                accountDataStore.FetchByIdAsync(
                    accountData.Id);

            // Assert

            accountDataFetched.Should().NotBeNull();
            accountDataFetched.AccountId.Should().Be(accountData.AccountId);
            accountDataFetched.AccountNumber.Should().Be(accountData.AccountNumber);
            accountDataFetched.Id.Should().Be(accountData.Id);
            accountDataFetched.PhoneNumber.Should().Be(accountData.PhoneNumber);
            accountDataFetched.SystemOfRecord.Should().Be(accountData.SystemOfRecord);
        }

        [Fact]
        public async Task When_UpdateByIdAsync()
        {
            // Arrange

            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    _configuration["AzureCosmosDocumentStoreOptions:DatabaseId"]);

            var cosmosContainer =
               cosmosDatabase.GetContainer("accounts");

            var accountData =
                new AccountData
                {
                    AccountNumber =
                        _faker.Random.Number(10000, 99999).ToString(),
                    SystemOfRecord =
                        _faker.PickRandom(
                            AccountData.SYSTEMOFRECORD_AUTOSUITE,
                            AccountData.SYSTEMOFRECORD_FISERVE,
                            AccountData.SYSTEMOFRECORD_ISERIES,
                            AccountData.SYSTEMOFRECORD_LEASEMASTER),
                    PhoneNumber =
                        _faker.Person.Phone,
                };

            await cosmosContainer.CreateItemAsync(
                accountData,
                new PartitionKey(accountData.AccountId.ToString()));

            var accountDataStoreOptions =
                new AccountDataStoreOptions(
                    _configuration["AzureCosmosDocumentStoreOptions:DatabaseId"],
                    _cosmosClient);

            var accountDataStore =
                new AccountDataStore(
                    accountDataStoreOptions);

            // Act

            accountData.AccountNumber =
                _faker.Random.Number(10000, 99999).ToString();
            accountData.SystemOfRecord =
                _faker.PickRandom(
                    AccountData.SYSTEMOFRECORD_AUTOSUITE,
                    AccountData.SYSTEMOFRECORD_FISERVE,
                    AccountData.SYSTEMOFRECORD_ISERIES,
                    AccountData.SYSTEMOFRECORD_LEASEMASTER);
            accountData.AccountNumber =
                _faker.Random.Number(10000, 99999).ToString();

            await accountDataStore.UpdateByIdAsync(
                accountData.Id,
                accountData);

            // Assert

            var accountDataResponse =
                await cosmosContainer.ReadItemAsync<AccountData>(
                    accountData.Id.ToString(),
                        new PartitionKey(accountData.AccountId.ToString()));

            accountDataResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var accountDataFetched =
                accountDataResponse.Resource;

            accountDataFetched.Should().NotBeNull();
            accountDataFetched.AccountId.Should().Be(accountData.AccountId);
            accountDataFetched.AccountNumber.Should().Be(accountData.AccountNumber);
            accountDataFetched.Id.Should().Be(accountData.Id);
            accountDataFetched.PhoneNumber.Should().Be(accountData.PhoneNumber);
            accountDataFetched.SystemOfRecord.Should().Be(accountData.SystemOfRecord);
        }
    }
}
