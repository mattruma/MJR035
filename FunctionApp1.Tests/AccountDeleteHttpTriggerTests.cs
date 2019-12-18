using FluentAssertions;
using FunctionApp1.Data;
using FunctionApp1.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace FunctionApp1.Tests
{
    public class AccountDeleteHttpTriggerTests : BaseTests
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
                .Setup(x => x.DeleteByIdAsync(It.IsAny<Guid>(), It.IsAny<string>()));

            var accountDeleteHttpTrigger =
                new AccountDeleteHttpTrigger(
                    accountDataStore.Object);

            var httpRequest =
                HttpRequestHelper.CreateHttpRequest(
                    "DELETE",
                    "http://localhost");

            var accountNumber =
                _faker.Random.Number(10000, 99999).ToString();

            // Act

            var actionResult =
                await accountDeleteHttpTrigger.Run(
                    httpRequest,
                    accountNumber,
                    _logger);

            // Assert

            accountDataStore.Verify(x => x.FetchByAccountNumberAsync(It.IsAny<string>()));
            accountDataStore.Verify(x => x.DeleteByIdAsync(It.IsAny<Guid>(), It.IsAny<string>()));

            actionResult.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
