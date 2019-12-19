using FluentAssertions;
using FunctionApp2.Data;
using FunctionApp2.Domain;
using FunctionApp2.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace FunctionApp2.Tests
{
    public class AccountUpdateHttpTriggerTests : BaseTests
    {
        [Fact]
        public async void When_Trigger()
        {
            // Arrange

            var accountDataStore =
                new Mock<IAccountDataStore>();

            accountDataStore
                .Setup(x => x.FetchByAccountNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(new AccountData());

            accountDataStore
                .Setup(x => x.UpdateByIdAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<AccountData>()));

            var accountUpdateHttpTrigger =
                new AccountUpdateHttpTrigger(
                    accountDataStore.Object);

            var accountEntityUpdateOptions =
                new AccountEntityUpdateOptions
                {
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
                    "PUT",
                    "http://localhost",
                    httpRequestHeaders,
                    accountEntityUpdateOptions);

            var accountNumber =
                _faker.Random.Number(10000, 99999).ToString();

            // Act

            var actionResult =
                await accountUpdateHttpTrigger.Run(
                    httpRequest,
                    accountNumber,
                    _logger);

            // Assert

            accountDataStore.Verify(x => x.FetchByAccountNumberAsync(It.IsAny<string>()));
            accountDataStore.Verify(x => x.UpdateByIdAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<AccountData>()));

            actionResult.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}
