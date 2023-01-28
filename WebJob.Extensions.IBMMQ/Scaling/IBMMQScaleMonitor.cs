using Microsoft.Azure.WebJobs.Host.Scale;
using WebJob.Extensions.IBMMQ.Binding;

namespace WebJob.Extensions.IBMMQ.Scaling;

internal class IBMMQScaleMonitor : IScaleMonitor<IBMMQScaleMetrics>
{
    public IBMMQScaleMonitor(IBMMQListenerSettings settings)
    {
        Descriptor = new ScaleMonitorDescriptor($"IBMMQTrigger-{settings.ServerName}-{settings.QueueManager}-{settings.QueueName}");
    }

    Task<ScaleMetrics> IScaleMonitor.GetMetricsAsync()
    {
        return Task.FromResult((ScaleMetrics)new IBMMQScaleMetrics { MessageCount = 0 });
    }

    Task<IBMMQScaleMetrics> IScaleMonitor<IBMMQScaleMetrics>.GetMetricsAsync()
    {
        return Task.FromResult(new IBMMQScaleMetrics { MessageCount = 0 });
    }

    public ScaleStatus GetScaleStatus(ScaleStatusContext context)
    {
        return new ScaleStatus { Vote = ScaleVote.None };
    }

    public ScaleStatus GetScaleStatus(ScaleStatusContext<IBMMQScaleMetrics> context)
    {
        return new ScaleStatus { Vote = ScaleVote.None };
    }

    public ScaleMonitorDescriptor Descriptor { get; }
}