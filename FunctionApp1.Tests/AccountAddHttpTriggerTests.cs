using FluentAssertions;
using FunctionApp1.Data;
using FunctionApp1.Domain;
using FunctionApp1.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FunctionApp1.Tests
{
    public class AccountAddHttpTriggerTests : BaseTests
    {
        [Fact]
        public async void When_Trigger()
        {
            // Arrange

            var accountDataStore =
                new Mock<IAccountDataStore>();

            var accountAddHttpTrigger =
                new AccountAddHttpTrigger(
                    accountDataStore.Object);

            var accountEntityAddOptions =
                new AccountEntityAddOptions
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

            var httpRequestHeaders =
                new HeaderDictionary
                {
                    { "Content-Type", "application/json" }
                };

            var httpRequest =
                HttpRequestHelper.CreateHttpRequest(
                    "POST",
                    "http://localhost",
                    httpRequestHeaders,
                    accountEntityAddOptions);

            // Act

            var actionResult =
                await accountAddHttpTrigger.Run(
                    httpRequest,
                    _logger);

            // Assert

            actionResult.Should().BeOfType(typeof(CreatedResult));

            var createdResult =
                (CreatedResult)actionResult;

            createdResult.Value.Should().BeOfType(typeof(AccountEntity));
        }
    }
}
