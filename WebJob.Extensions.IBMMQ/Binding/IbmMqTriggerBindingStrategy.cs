using IBM.WMQ;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Triggers;

namespace WebJob.Extensions.IBMMQ.Binding;

public class IbmMqTriggerBindingStrategy : ITriggerBindingStrategy<MQMessage, MQTriggerInput>
{
    // when was invoked from portal
    public MQTriggerInput ConvertFromString(string message)
    {
        MQMessage mqMessage = new();
        mqMessage.WriteString(message);
        return new MQTriggerInput(mqMessage);
    }

    // those two methods is where you plugin metadata binding.
    // ServiceBus uses it to populate things like MessageActions, MessageId, DeliveryCount, etc.
    // in GetBindingContract you configure metadata type mapping
    // in GetBindingData you populate those keys with actual values.
    // for example: GetBindingContract => dictionary["MessageId"] = typeof(string)
    // and GetBindingData => dictionary["MessageId"] = input.Messages[0].MessageID
    public Dictionary<string, Type> GetBindingContract(bool isSingleDispatch) => new();
    public Dictionary<string, object> GetBindingData(MQTriggerInput value) => new();

    public MQMessage BindSingle(MQTriggerInput value, ValueBindingContext context) => value.Messages[0];

    public MQMessage[] BindMultiple(MQTriggerInput value, ValueBindingContext context) => value.Messages;
}