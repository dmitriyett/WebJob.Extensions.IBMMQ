using Microsoft.Azure.WebJobs.Description;

namespace WebJob.Extensions.IBMMQ;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
[Binding]
public class IbmMqAttribute : Attribute
{
    public IbmMqAttribute(string queueManager, string serverName, string portNumber, string channel, string queueName)
    {
        QueueManager = queueManager;
        ServerName = serverName;
        PortNumber = portNumber;
        Channel = channel;
        QueueName = queueName;
    }

    [AutoResolve]
    public string QueueManager { get; }
    [AutoResolve]
    public string ServerName { get; }
    [AutoResolve]
    // string, so that it could be resolved and bound in runtime
    public string PortNumber { get; }
    [AutoResolve]
    public string Channel { get; }
    [AutoResolve]
    public string QueueName { get; }
    [AutoResolve]
    public string? UserName { get; set; }
    [AutoResolve]
    public string? Password { get; set; }
    [AutoResolve]
    public string? SslCipherSpec { get; set; }
    [AutoResolve]
    public string? CertificateName { get; set; }
}