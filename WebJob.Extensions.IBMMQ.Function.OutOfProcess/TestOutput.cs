using Microsoft.Azure.Functions.Worker;
using Worker.Extensions.IBMMQ;

namespace WebJob.Extensions.IBMMQ.Function.OutOfProcess;

public class TestOutput
{
    [Function(nameof(TestOutputWithString))]
    [IbmMqOutput("QMGR", "SRVR", "543", "CHNL", "TestPoco")]
    public Task<string> TestOutputWithString(
        [TimerTrigger("* * * * *")] TimerInfo timerInfo,
        FunctionContext context)
    {
        return Task.FromResult("This is output from isolated");
    }

    public record Poco(string Name);

    [Function(nameof(TestOutputWithPoco))]
    [IbmMqOutput("QMGR", "SRVR", "543", "CHNL", "TestPoco")]
    public Task<Poco> TestOutputWithPoco(
        [TimerTrigger("* * * * *")] TimerInfo timerInfo,
        FunctionContext context)
    {
        return Task.FromResult(new Poco("This is output from isolated"));
    }
}