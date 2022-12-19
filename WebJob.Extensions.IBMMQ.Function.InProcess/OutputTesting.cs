using System.Threading.Tasks;
using IBM.WMQ;
using Microsoft.Azure.WebJobs;
using WebJob.Extensions.IBMMQ;

namespace CustomTriggersDemo.Function;

public class OutputTesting
{
    [FunctionName(nameof(MQMessageOutput))]
    [return: IbmMq("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")]
    public Task<MQMessage> MQMessageOutput(
        [TimerTrigger("* * * * *", RunOnStartup = true)] TimerInfo timerInfo)
    {
        MQMessage msg = new();
        msg.WriteString("This is output");
        msg.DataOffset = 0;

        return Task.FromResult(msg);
    }

    [FunctionName(nameof(StringOutput))]
    [return: IbmMq("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")]
    public Task<string> StringOutput(
        [TimerTrigger("* * * * *", RunOnStartup = true)] TimerInfo timerInfo)
    {
        return Task.FromResult("This is output");
    }

    public record OutputPoco(string Name, int PortVal);

    [FunctionName(nameof(PocoOutput))]
    [return: IbmMq("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")]
    public Task<OutputPoco> PocoOutput(
        [TimerTrigger("* * * * *", RunOnStartup = true)] TimerInfo timerInfo)
    {
        return Task.FromResult(new OutputPoco("hop", 123));
    }

    [FunctionName(nameof(MQMessageAsyncCollector))]
    public async Task MQMessageAsyncCollector(
        [TimerTrigger("* * * * *", RunOnStartup = true)]
         TimerInfo timerInfo,
        [IbmMq("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")] IAsyncCollector<MQMessage> mqMsgCollector)
    {
        var msg = await MQMessageOutput(null);
        await mqMsgCollector.AddAsync(msg);
    }

    [FunctionName(nameof(MQMessageCollector))]
    public async Task MQMessageCollector(
        [TimerTrigger("* * * * *", RunOnStartup = true)]
         TimerInfo timerInfo,
         [IbmMq("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")] ICollector<MQMessage> mqMsgCollector)
    {
        var msg = await MQMessageOutput(null);
        mqMsgCollector.Add(msg);
    }

    [FunctionName(nameof(StringAsyncCollector))]
    public async Task StringAsyncCollector(
        [TimerTrigger("* * * * *", RunOnStartup = true)] TimerInfo timerInfo,
        [IbmMq("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")] IAsyncCollector<string> mqMsgCollector)
    {
        var msg = await StringOutput(null);
        await mqMsgCollector.AddAsync(msg);
    }

    [FunctionName(nameof(StringCollector))]
    public async Task StringCollector(
        [TimerTrigger("* * * * *", RunOnStartup = true)] TimerInfo timerInfo,
        [IbmMq("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")] ICollector<string> mqMsgCollector)
    {
        var msg = await StringOutput(null);
        mqMsgCollector.Add(msg);
    }

    [FunctionName(nameof(PocoAsyncCollector))]
    public async Task PocoAsyncCollector(
        [TimerTrigger("* * * * *", RunOnStartup = true)] TimerInfo timerInfo,
        [IbmMq("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")] IAsyncCollector<OutputPoco> mqMsgCollector)
    {
        var msg = await PocoOutput(null);
        await mqMsgCollector.AddAsync(msg);
    }

    [FunctionName(nameof(PocoCollector))]
    public async Task PocoCollector(
        [TimerTrigger("* * * * *", RunOnStartup = true)] TimerInfo timerInfo,
        [IbmMq("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")] ICollector<OutputPoco> mqMsgCollector)
    {
        var msg = await PocoOutput(null);
        mqMsgCollector.Add(msg);
    }
}