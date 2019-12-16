using FunctionApp1.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionApp1.Tests
{
    public abstract class BaseTests
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger _logger = LoggerHelper.CreateLogger(LoggerTypes.List);

        protected BaseTests()
        {
            // NOTE: Make sure to set these files to copy to output directory

            _configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .AddJsonFile("appsettings.Development.json")
                 .Build();
        }
    }
}
