using FluentAssertions;
using FunctionApp1.Domain;
using FunctionApp1.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace FunctionApp1.Tests
{
    public class NextPaymentDateCalculateHttpTriggerTests : BaseTests
    {
        [Fact]
        public async void When_Trigger()
        {
            // Arrange

            var nextPaymentDate =
                _faker.Date.Soon();

            var nextPaymentDateCalculateOptions =
                new NextPaymentDateCalculateOptions
                {
                    NextPaymentDate = nextPaymentDate
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
                    nextPaymentDateCalculateOptions);

            // Act

            var actionResult =
                await NextPaymentDateCalculateHttpTrigger.Run(
                    httpRequest,
                    _logger);

            // Assert

            actionResult.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult =
                (OkObjectResult)actionResult;

            okObjectResult.Value.Should().BeOfType(typeof(DateTime));
        }

        [Fact]
        public async void When_Trigger_And_NextPaymentDateIsNull()
        {
            // Arrange

            var nextPaymentDateCalculateOptions =
                new NextPaymentDateCalculateOptions
                {
                    NextPaymentDate = null
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
                    nextPaymentDateCalculateOptions);

            // Act

            var actionResult =
                await NextPaymentDateCalculateHttpTrigger.Run(
                    httpRequest,
                    _logger);

            // Assert;
            var okObjectResult =
                (OkObjectResult)actionResult;

            var nextPaymentDate =
                (DateTime)okObjectResult.Value;

            nextPaymentDate.Date.Should().Be(DateTime.UtcNow.Date.AddDays(2));
        }

        [Fact]
        public async void When_Trigger_And_NextPaymentDateIsNotNull()
        {
            // Arrange

            var nextPaymentDateCalculateOptions =
                new NextPaymentDateCalculateOptions
                {
                    NextPaymentDate = DateTime.UtcNow.AddDays(2)
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
                    nextPaymentDateCalculateOptions);

            // Act

            var actionResult =
                await NextPaymentDateCalculateHttpTrigger.Run(
                    httpRequest,
                    _logger);

            // Assert;
            var okObjectResult =
                (OkObjectResult)actionResult;

            var nextPaymentDate =
                (DateTime)okObjectResult.Value;

            var expectedNextPaymentDate =
                nextPaymentDateCalculateOptions.NextPaymentDate.Value.Date.AddDays(2);

            nextPaymentDate.Date.Should().Be(expectedNextPaymentDate);
        }
    }
}
