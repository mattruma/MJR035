using FluentAssertions;
using Xunit;

namespace FunctionApp1.Tests
{
    public class CaseNumberGenerateTests : BaseTests
    {
        [Fact]
        public void When_GenerateAsync()
        {
            var caseNumberGenerate =
                new CaseNumberGenerate();

            var caseNumberGenerated =
                caseNumberGenerate.Generate();

            caseNumberGenerated.Should().BeGreaterThan(10000);
        }
    }
}
