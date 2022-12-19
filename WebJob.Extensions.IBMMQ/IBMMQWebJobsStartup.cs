using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Hosting;
using WebJob.Extensions.IBMMQ;

[assembly: WebJobsStartup(typeof(IBMMQWebJobsStartup))]

namespace WebJob.Extensions.IBMMQ;

public class IBMMQWebJobsStartup : IWebJobsStartup
{
    public void Configure(IWebJobsBuilder builder)
    {
        builder.AddIBMMQ();
    }
}