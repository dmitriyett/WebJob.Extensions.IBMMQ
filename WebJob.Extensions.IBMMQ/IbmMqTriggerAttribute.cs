using Microsoft.Azure.WebJobs.Description;

namespace WebJob.Extensions.IBMMQ;

[AttributeUsage(AttributeTargets.Parameter)]
[Binding]
public class IbmMqTriggerAttribute : Attribute
{
    public IbmMqTriggerAttribute(string queueManager, string serverName, string portNumber, string channel, string queueName)
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
    public int MaxBatchSize { get; set; } = 25;
}