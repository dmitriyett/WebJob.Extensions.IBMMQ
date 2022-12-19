using IBM.WMQ;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace WebJob.Extensions.IBMMQ.Binding;

public class IBMMQAsyncCollector : IAsyncCollector<MQMessage>
{
    private readonly IbmMqAttribute _attribute;
    private readonly ILogger _logger;

    public IBMMQAsyncCollector(IbmMqAttribute attribute, ILogger logger)
    {
        _attribute = attribute;
        _logger = logger;
    }

    public Task AddAsync(MQMessage item, CancellationToken cancellationToken = new CancellationToken())
    {
        var result = item.ReadString(item.MessageLength);
        item.DataOffset = 0;

        _logger.LogInformation(result);

        return Task.CompletedTask;
    }

    public Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Collector FlushAsync");
        return Task.CompletedTask;
    }
}