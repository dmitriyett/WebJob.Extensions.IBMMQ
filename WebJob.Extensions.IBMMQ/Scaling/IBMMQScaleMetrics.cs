using Microsoft.Azure.WebJobs.Host.Scale;

namespace WebJob.Extensions.IBMMQ.Scaling;

public class IBMMQScaleMetrics : ScaleMetrics
{
    public long MessageCount { get; set; }
}