using IBM.WMQ;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Triggers;

namespace WebJob.Extensions.IBMMQ.Binding;

#pragma warning disable CS0618
public class IBMMQTriggerBindingStrategy : ITriggerBindingStrategy<MQMessage, IBMMQTriggerInput>
#pragma warning restore CS0618
{
    // when was invoked from portal
    public IBMMQTriggerInput ConvertFromString(string message)
    {
        MQMessage mqMessage = new();
        mqMessage.WriteString(message);
        return new IBMMQTriggerInput(mqMessage);
    }

    // those two methods is where you plugin metadata binding.
    // ServiceBus uses it to populate things like MessageActions, MessageId, DeliveryCount, etc.
    // in GetBindingContract you configure metadata type mapping
    // in GetBindingData you populate those keys with actual values.
    // for example: GetBindingContract => dictionary["MessageId"] = typeof(string)
    // and GetBindingData => dictionary["MessageId"] = input.Messages[0].MessageID
    public Dictionary<string, Type> GetBindingContract(bool isSingleDispatch) => new();
    public Dictionary<string, object> GetBindingData(IBMMQTriggerInput value) => new();

    public MQMessage BindSingle(IBMMQTriggerInput value, ValueBindingContext context) => value.Messages[0];

    public MQMessage[] BindMultiple(IBMMQTriggerInput value, ValueBindingContext context) => value.Messages;
}