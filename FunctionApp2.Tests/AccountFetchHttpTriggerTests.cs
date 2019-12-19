using FluentAssertions;
using FunctionApp2.Data;
using FunctionApp2.Domain;
using FunctionApp2.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FunctionApp2.Tests
{
    public class AccountFetchHttpTriggerTests : BaseTests
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

            var accountFetchHttpTrigger =
                new AccountFetchHttpTrigger(
                    accountDataStore.Object);

            var httpRequest =
                HttpRequestHelper.CreateHttpRequest(
                    "GET",
                    "http://localhost");

            var accountNumber =
                _faker.Random.Number(10000, 99999).ToString();

            // Act

            var actionResult =
                await accountFetchHttpTrigger.Run(
                    httpRequest,
                    accountNumber,
                    _logger);

            // Assert

            accountDataStore.Verify(x => x.FetchByAccountNumberAsync(It.IsAny<string>()));

            actionResult.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult =
                (OkObjectResult)actionResult;

            okObjectResult.Value.Should().BeOfType(typeof(AccountEntity));
        }
    }
}
