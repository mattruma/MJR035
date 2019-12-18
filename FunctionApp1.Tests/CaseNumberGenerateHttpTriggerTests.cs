using FluentAssertions;
using FunctionApp1.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FunctionApp1.Tests
{
    public class CaseNumberGenerateHttpTriggerTests : BaseTests
    {
        [Fact]
        public void When_Trigger()
        {
            // Arrange

            var httpRequest =
                HttpRequestHelper.CreateHttpRequest(
                    "POST",
                    "http://localhost");

            // Act

            var actionResult =
                CaseNumberGenerateHttpTrigger.Run(
                    httpRequest,
                    _logger);

            // Assert

            actionResult.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult =
                (OkObjectResult)actionResult;

            okObjectResult.Value.Should().BeOfType(typeof(int));
        }
    }
}
