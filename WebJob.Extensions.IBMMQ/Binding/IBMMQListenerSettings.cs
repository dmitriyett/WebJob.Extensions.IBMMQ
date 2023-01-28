using System.Collections;
using IBM.WMQ;

namespace WebJob.Extensions.IBMMQ.Binding;

internal record IBMMQListenerSettings(string QueueManager, string ServerName, int PortNumber, string Channel, string QueueName)
{
    public string Transport { get; set; } = MQC.TRANSPORT_MQSERIES_MANAGED;
    public int? ConnectOptions { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? SslCipherSpec { get; set; }
    public string? CertificateName { get; set; }
    public int MaxBatchSize { get; init; } = 25;

    public Hashtable BuildConnectionProperties()
    {
        Hashtable properties = new()
        {
            { MQC.TRANSPORT_PROPERTY, Transport },
            { MQC.HOST_NAME_PROPERTY, ServerName },
            { MQC.PORT_PROPERTY, PortNumber },
            { MQC.CHANNEL_PROPERTY, Channel },
        };

        if (ConnectOptions is not null)
        {
            properties.Add(MQC.CONNECT_OPTIONS_PROPERTY, ConnectOptions);
        }
        
        if (!string.IsNullOrEmpty(CertificateName))
        {
            properties.Add(MQC.SSL_CERT_STORE_PROPERTY, "*USER");
            properties.Add(MQC.CERT_LABEL_PROPERTY, CertificateName);
            properties.Add(MQC.SSL_CIPHER_SPEC_PROPERTY, SslCipherSpec);
            properties.Add(MQC.OUTBOUND_SNI_PROPERTY, MQC.OUTBOUND_SNI_HOSTNAME);
        }

        if (!string.IsNullOrEmpty(UserName))
        {
            properties.Add(MQC.USER_ID_PROPERTY, UserName);
            properties.Add(MQC.PASSWORD_PROPERTY, Password);
        }

        return properties;
    }
}