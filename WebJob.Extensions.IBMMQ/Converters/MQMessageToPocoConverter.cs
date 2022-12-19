using IBM.WMQ;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace WebJob.Extensions.IBMMQ.Converters;

public class MQMessageToPocoConverter<T> : IConverter<MQMessage, T>
{
    public T Convert(MQMessage input)
    {
        var text = input.ReadString(input.MessageLength);
        input.DataOffset = 0;
        return JsonConvert.DeserializeObject<T>(text);
    }
}