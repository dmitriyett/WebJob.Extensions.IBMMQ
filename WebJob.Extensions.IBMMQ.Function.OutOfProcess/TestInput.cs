using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Worker.Extensions.IBMMQ;

namespace WebJob.Extensions.IBMMQ.Function.OutOfProcess;

public class TestInput
{
    private readonly ILogger<TestInput> _logger;

    public TestInput(ILogger<TestInput> logger)
    {
        _logger = logger;
    }

    [Function(nameof(TestInputWithString))]
    public Task TestInputWithString(
        [IbmMqTrigger("QMGR", "SRVRNM", "321", "CHNL", "Queue")] string input)
    {
        _logger.LogInformation(input);

        return Task.CompletedTask;
    }

    public record Poco(string QueueManager, string ServerName, int PortNumber, string Channel, string QueueName);

    [Function(nameof(TestInputWithPoco))]
    public Task TestInputWithPoco(
        [IbmMqTrigger("QMGR", "SRVRNM", "321", "CHNL", "Queue")] Poco input)
    {
        _logger.LogInformation(input.ToString());

        return Task.CompletedTask;
    }
}