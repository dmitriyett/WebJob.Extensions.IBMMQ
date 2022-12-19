using System.Text;
using IBM.WMQ;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;
using WebJob.Extensions.IBMMQ.Binding;
using WebJob.Extensions.IBMMQ.Converters;

namespace WebJob.Extensions.IBMMQ.Config;

[Extension("IBMMQ")]
public class IBMMQExtensionConfigProvider : IExtensionConfigProvider
{
    private readonly INameResolver _nameResolver;
    private readonly IConverterManager _converterManager;
    private readonly ILogger _logger;

    public IBMMQExtensionConfigProvider(INameResolver nameResolver, ILoggerFactory loggerFactory,
        IConverterManager converterManager)
    {
        _nameResolver = nameResolver;
        _converterManager = converterManager;
        _logger = loggerFactory.CreateLogger(LogCategories.CreateTriggerCategory("IBMMQ"));
    }

    public void Initialize(ExtensionConfigContext context)
    {
        // An extension should provide the following:
        // Bindings that expose the SDK native types.
        // Bindings to BCL types (like System, byte[], stream) or POCOs. That way, customers can use the binding without having to directly use the native SDK.
        // Bindings to JObject and JArray. This enables the binding to be consumed from non-.NET languages, such as JavaScript and Powershell.
        context
            .AddConverter<MQMessage, string>(MQMessageToString)
            .AddConverter<MQMessage, byte[]>(MQMessageToBytes)
            .AddOpenConverter<MQMessage, OpenType.Poco>(typeof(MQMessageToPocoConverter<>))
            .AddConverter<string, MQMessage>(StringToMQMessage)
            .AddConverter<byte[], MQMessage>(BytesToMQMessage)
            .AddOpenConverter<OpenType.Poco, MQMessage>(typeof(PocoToMQMessageConverter<>));

        var triggerBindingRule = context.AddBindingRule<IbmMqTriggerAttribute>();

        // There is also BindToTrigger<> overload that was supposed to automatically plug in
        // trigger provided values to converter infrastructure, but it seems there are two problems with it:
        // 1. internal binding adapter wraps strings into DirectInvokeString class and does MQMessage -> DirectInvokeString conversion.
        // since that converter is not registered, MQMessage -> POCO converter is selected implicitly, leading to deserialization issues
        // this issue could be easily worked around by registering dedicated MQMessage -> DirectInvokeString converter
        // 2. internal binding adapter demands a direct MQMessage -> TTargetType converter to be registered.
        // that is fine in most cases, but is not working in batch trigger scenarios.
        // To work that around you can have two options: register multiple bindings
        // (i.e. BindToTrigger<MQMessage>(...) and BindToTrigger<MQMessage[]>(...))
        // or utilize ITriggerBindingStrategy pattern as shown in this example.
        triggerBindingRule.BindToTrigger(new IbmMqTriggerBindingProvider(_nameResolver, _logger, _converterManager));

        var bindingRule = context.AddBindingRule<IbmMqAttribute>();

        // BindToValueProvider does not provide automatic conversion juice, you'll have to cook it yourself
        // but it provides you OnComplete trigger essentially (via IValueBinder.SetValue), that could be used as kind of Flush pattern
        bindingRule.BindToInput(new MQAttributeToMQQueueConverter());
        bindingRule.BindToCollector(attr => new IBMMQAsyncCollector(attr, _logger));
    }

    private static string MQMessageToString(MQMessage msg)
    {
        var result = msg.ReadString(msg.MessageLength);
        msg.DataOffset = 0;
        return result;
    }

    private static byte[] MQMessageToBytes(MQMessage msg)
    {
        var bytes = msg.ReadBytes(msg.MessageLength);
        msg.DataOffset = 0;
        return bytes;
    }

    private static MQMessage StringToMQMessage(string msg)
    {
        MQMessage mq = new MQMessage();
        mq.WriteString(msg);
        mq.DataOffset = 0;
        return mq;
    }

    private static MQMessage BytesToMQMessage(byte[] bytes)
    {
        MQMessage mq = new MQMessage();
        // that's the way they do it.
        mq.WriteBytes(Encoding.Unicode.GetString(bytes));
        mq.DataOffset = 0;
        return mq;
    }
}