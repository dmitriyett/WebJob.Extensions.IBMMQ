using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;

namespace Worker.Extensions.IBMMQ
{
    public class IbmMqTriggerAttribute : TriggerBindingAttribute
    {
        public IbmMqTriggerAttribute(string queueManager, string serverName, string portNumber, string channel, string queueName)
        {
            QueueManager = queueManager;
            ServerName = serverName;
            PortNumber = portNumber;
            Channel = channel;
            QueueName = queueName;
        }

        public string QueueManager { get; }
        public string ServerName { get; }
        public string PortNumber { get; }
        public string Channel { get; }
        public string QueueName { get; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? SslCipherSpec { get; set; }
        public string? CertificateName { get; set; }
        public int MaxBatchSize { get; set; } = 25;
    }
}
