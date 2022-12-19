using IBM.WMQ;

namespace WebJob.Extensions.IBMMQ.Binding;

public class MQTriggerInput
{
    public MQTriggerInput(MQMessage message)
    {
        Messages = new[] { message };
    }

    public MQTriggerInput(IEnumerable<MQMessage> messages)
    {
        Messages = messages.ToArray();
    }

    public MQMessage[] Messages { get; }
}