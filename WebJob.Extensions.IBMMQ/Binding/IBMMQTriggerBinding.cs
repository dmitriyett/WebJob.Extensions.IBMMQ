using IBM.WMQ;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Microsoft.Extensions.Logging;
using WebJob.Extensions.IBMMQ.Listener;

namespace WebJob.Extensions.IBMMQ.Binding;

internal class IBMMQTriggerBinding : ITriggerBinding
{
    private readonly IBMMQListenerSettings _settings;
    private readonly ILogger _logger;
    private readonly bool _singleDispatch;

    private readonly Task<ITriggerData> _emptyBindingDataTask =
        Task.FromResult<ITriggerData>(new TriggerData(null, new Dictionary<string, object>()));

    public IBMMQTriggerBinding(IBMMQListenerSettings settings, ILogger logger, bool singleDispatch)
    {
        _settings = settings;
        _logger = logger;
        _singleDispatch = singleDispatch;
    }

    public IReadOnlyDictionary<string, Type> BindingDataContract { get; } = new Dictionary<string, Type>();

    public Task<ITriggerData> BindAsync(object value, ValueBindingContext context) => _emptyBindingDataTask;

    public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
    {
        return Task.FromResult((IListener)new IbmMqListener(_settings, _logger, context.Executor, _singleDispatch));
    }

    public ParameterDescriptor ToParameterDescriptor() => new IbmMqTriggerParameterDescriptor(_settings.QueueName);

    public Type TriggerValueType => typeof(MQMessage);

    private sealed class IbmMqTriggerParameterDescriptor : TriggerParameterDescriptor
    {
        private readonly string _queueName;

        public IbmMqTriggerParameterDescriptor(string queueName)
        {
            _queueName = queueName;
        }

        public override string GetTriggerReason(IDictionary<string, string> arguments)
        {
            return $"IBM MQ message detected in the queue: {_queueName} at {DateTime.UtcNow}";
        }
    }
}