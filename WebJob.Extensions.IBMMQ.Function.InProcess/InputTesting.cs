using System.Threading.Tasks;
using IBM.WMQ;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace WebJob.Extensions.IBMMQ.Function.InProcess;

public class InputTesting
{
    [FunctionName(nameof(TestWithSingleMessage))]
    public Task TestWithSingleMessage(
        [IbmMqTrigger("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")] MQMessage msg,
        ILogger log)
    {
        log.LogInformation("Triggered with {Message}", msg.ReadString(msg.MessageLength));

        return Task.CompletedTask;
    }
    [FunctionName(nameof(TestWithMultipleMessages))]
    public Task TestWithMultipleMessages(
        [IbmMqTrigger("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")]
         MQMessage[] msgs,
        ILogger log)
    {
        foreach (var msg in msgs)
        {
            log.LogInformation("Triggered with {Message}", msg.ReadString(msg.MessageLength));
        }

        return Task.CompletedTask;
    }


    [FunctionName(nameof(TestWithStringMessage))]
    public Task TestWithStringMessage(
        [IbmMqTrigger("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")]
         string msg,
        ILogger log)
    {
        log.LogInformation("Triggered with {Message}", msg);

        return Task.CompletedTask;
    }

    [FunctionName(nameof(TestWithStringArray))]
    public Task TestWithStringArray(
        [IbmMqTrigger("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")]
         string[] msgs,
        ILogger log)
    {
        foreach (var msg in msgs)
        {
            log.LogInformation("Triggered with {Message}", msg);
        }

        return Task.CompletedTask;
    }

    public record SimplePoco(string Name);

    [FunctionName(nameof(TestWithPoco))]
    public Task TestWithPoco(
        [IbmMqTrigger("QMGR/%TestGettingThisFromConfig%", "SRVNM", "123", "CHNL", "queue name")]
         SimplePoco msg,
        ILogger log)
    {
        log.LogInformation("Triggered with {Message}", msg);

        return Task.CompletedTask;
    }
}