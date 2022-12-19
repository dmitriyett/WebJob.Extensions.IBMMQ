using IBM.WMQ;
using Microsoft.Azure.WebJobs;

namespace WebJob.Extensions.IBMMQ.Converters;

public class MQAttributeToMQQueueConverter : IConverter<IbmMqAttribute, MQQueue>
{
    public MQQueue Convert(IbmMqAttribute input)
    {
        throw new NotImplementedException();
    }
}