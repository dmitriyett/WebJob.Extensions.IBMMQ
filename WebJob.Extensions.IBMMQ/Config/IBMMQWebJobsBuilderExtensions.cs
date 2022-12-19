using Microsoft.Azure.WebJobs;
using WebJob.Extensions.IBMMQ.Config;

// TODO: check whether specific namespace required for isolated function
namespace Microsoft.Extensions.Hosting;

public static class IBMMQWebJobsBuilderExtensions
{
    public static IWebJobsBuilder AddIBMMQ(this IWebJobsBuilder builder)
    {
        builder.AddExtension<IBMMQExtensionConfigProvider>();

        return builder;
    }
}