using ClassLibrary1;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FunctionApp1.Startup))]
namespace FunctionApp1
{
    public class Startup : FunctionsStartup
    {
        // https://dotnetcoretutorials.com/2017/03/25/net-core-dependency-injection-lifetimes-explained/

        // Transient Lifetime
        // If in doubt, make it transient. Adding a transient service means that each time the service is requested, a new instance is created.

        // Singleton Lifetime
        // A singleton is an instance that will last the entire lifetime of the application.

        // Scoped Lifetime
        // Scoped lifetime actually means that within a created “scope” objects will be the same instance.

        public override void Configure(
            IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ICaseNumberGenerate, CaseNumberGenerate>();
        }
    }
}
