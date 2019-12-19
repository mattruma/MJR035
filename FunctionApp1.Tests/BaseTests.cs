using Bogus;
using FunctionApp1.Tests.Helpers;
using Microsoft.Extensions.Logging;

namespace FunctionApp1.Tests
{
    public abstract class BaseTests
    {
        protected readonly ILogger _logger = LoggerHelper.CreateLogger(LoggerTypes.List);
        protected readonly Faker _faker;

        protected BaseTests()
        {
            _faker = new Bogus.Faker();
        }
    }
}
