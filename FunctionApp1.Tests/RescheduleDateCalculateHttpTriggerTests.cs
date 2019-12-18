using FluentAssertions;
using FunctionApp1.Domain;
using FunctionApp1.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace FunctionApp1.Tests
{
    public class RescheduleDateCalculateHttpTriggerTests : BaseTests
    {
        [Fact]
        public async void When_Trigger()
        {
            // Arrange

            var rescheduleDateCalculateOptions =
                new RescheduleDateCalculateOptions();

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
                    rescheduleDateCalculateOptions);

            // Act

            var actionResult =
                await RescheduleDateCalculateHttpTrigger.Run(
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

            var rescheduleDateCalculateOptions =
                new RescheduleDateCalculateOptions
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
                    rescheduleDateCalculateOptions);

            // Act

            var actionResult =
                await RescheduleDateCalculateHttpTrigger.Run(
                    httpRequest,
                    _logger);

            // Assert

            var okObjectResult =
                (OkObjectResult)actionResult;

            var nextPaymentDate =
                (DateTime)okObjectResult.Value;

            var expectedNextPaymentDate =
                DateTime.UtcNow.Date.AddDays(2);

            nextPaymentDate.Date.Should().Be(expectedNextPaymentDate);
        }

        [Theory]
        [ClassData(typeof(RescheduleDateCalculateHttpTriggerTestsData))]
        public async void When_Trigger_And_NextPaymentDateIsNull_And_ZipCodeIsValid(
            DateTime nextPaymentDate,
            string postalCode,
            DateTime expectedNextPaymentDate)
        {
            // Arrange

            var rescheduleDateCalculateOptions =
                new RescheduleDateCalculateOptions
                {
                    NextPaymentDate = nextPaymentDate,
                    PostalCode = postalCode
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
                    rescheduleDateCalculateOptions);

            // Act

            var actionResult =
                await RescheduleDateCalculateHttpTrigger.Run(
                    httpRequest,
                    _logger);

            // Assert

            var okObjectResult =
                (OkObjectResult)actionResult;

            nextPaymentDate =
                (DateTime)okObjectResult.Value;

            nextPaymentDate.Date.Should().Be(expectedNextPaymentDate);
        }
    }
}
