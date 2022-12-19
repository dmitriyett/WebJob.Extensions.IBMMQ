using IBM.WMQ;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace WebJob.Extensions.IBMMQ.Converters;

public class PocoToMQMessageConverter<T> : IConverter<T, MQMessage>
{
    public MQMessage Convert(T input)
    {
        var text = JsonConvert.SerializeObject(input);
        MQMessage mqMsg = new();
        mqMsg.WriteString(text);
        mqMsg.DataOffset = 0;
        return mqMsg;
    }
}