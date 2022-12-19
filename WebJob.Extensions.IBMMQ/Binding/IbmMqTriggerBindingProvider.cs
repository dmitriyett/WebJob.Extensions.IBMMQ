using System.Reflection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Microsoft.Extensions.Logging;
using WebJob.Extensions.IBMMQ.Listener;

namespace WebJob.Extensions.IBMMQ.Binding;

public class IbmMqTriggerBindingProvider : ITriggerBindingProvider
{
    private readonly INameResolver _nameResolver;
    private readonly ILogger _logger;
    private readonly IConverterManager _converterManager;

    public IbmMqTriggerBindingProvider(INameResolver nameResolver, ILogger logger, IConverterManager converterManager)
    {
        _nameResolver = nameResolver;
        _logger = logger;
        _converterManager = converterManager;
    }

    public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
    {
        var attribute = context.Parameter.GetCustomAttribute<IbmMqTriggerAttribute>(inherit: false);

        // this method is called for every function parameter.
        // first not null is bound against that parameter.
        if (attribute == null)
        {
            return Task.FromResult<ITriggerBinding>(null!);
        }

        // AutoResolveAttribute is honored by certain adapters, namely BindToInput, BindToValueProvider, BindToCollector
        // in other cases you have to implement that functionality yourself
        IbmMqListenerSettings listenerSettings = new(
            _nameResolver.ResolveWholeString(attribute.QueueManager),
            _nameResolver.ResolveWholeString(attribute.ServerName),
            int.Parse(_nameResolver.ResolveWholeString(attribute.PortNumber)),
            _nameResolver.ResolveWholeString(attribute.Channel),
            _nameResolver.ResolveWholeString(attribute.QueueName))
        {
            UserName = _nameResolver.ResolveWholeString(attribute.UserName),
            Password = _nameResolver.ResolveWholeString(attribute.Password),
            CertificateName = _nameResolver.ResolveWholeString(attribute.CertificateName),
            SslCipherSpec = _nameResolver.ResolveWholeString(attribute.SslCipherSpec)
        };

        Task<IListener> CreateListener(ListenerFactoryContext listenerContext, bool singleDispatch)
        {
            var listener = new IbmMqListener(listenerSettings, _logger, listenerContext.Executor, singleDispatch);
            return Task.FromResult((IListener)listener);
        }

        var binding = BindingFactory.GetTriggerBinding(new IbmMqTriggerBindingStrategy(), context.Parameter,
            _converterManager,
            CreateListener);

        return Task.FromResult(binding);

        // var singleDispatch = !context.Parameter.ParameterType.IsArray;

        // return Task.FromResult((ITriggerBinding)new IbmMqTriggerBinding(listenerSettings, _logger, singleDispatch));
    }
}