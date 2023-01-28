using IBM.WMQ;

namespace WebJob.Extensions.IBMMQ.Binding;

public class IBMMQTriggerInput
{
    public IBMMQTriggerInput(MQMessage message)
    {
        Messages = new[] { message };
    }

    public IBMMQTriggerInput(IEnumerable<MQMessage> messages)
    {
        Messages = messages.ToArray();
    }

    public MQMessage[] Messages { get; }
}