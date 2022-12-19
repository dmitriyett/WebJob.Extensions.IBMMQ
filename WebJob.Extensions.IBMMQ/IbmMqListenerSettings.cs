namespace WebJob.Extensions.IBMMQ;

public record IbmMqListenerSettings(string QueueManager, string ServerName, int PortNumber, string Channel, string QueueName)
{
    public string? UserName { get; init; }
    public string? Password { get; init; }
    public string? SslCipherSpec { get; init; }
    public string? CertificateName { get; init; }
    public int MaxBatchSize { get; init; } = 25;
}