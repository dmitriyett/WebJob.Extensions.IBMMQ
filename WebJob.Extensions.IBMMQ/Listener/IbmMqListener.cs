using IBM.WMQ;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Scale;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebJob.Extensions.IBMMQ.Binding;
using WebJob.Extensions.IBMMQ.Scaling;

namespace WebJob.Extensions.IBMMQ.Listener;

public sealed class IbmMqListener : IListener, IScaleMonitorProvider
{
    private readonly IbmMqListenerSettings _settings;
    private readonly ILogger _logger;
    private readonly ITriggeredFunctionExecutor _executor;
    private readonly bool _singleDispatch;

    private CancellationTokenSource? _cancellationSource;
    private Task? _listenerLoop;

    public IbmMqListener(IbmMqListenerSettings settings, ILogger logger, ITriggeredFunctionExecutor executor,
        bool singleDispatch)
    {
        _settings = settings;
        _logger = logger;
        _executor = executor;
        _singleDispatch = singleDispatch;
    }

    public IScaleMonitor GetMonitor()
    {
        return new IBMMQScaleMonitor(_settings);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationSource?.Cancel();

        _cancellationSource = new CancellationTokenSource();

        var token = _cancellationSource.Token;

        _listenerLoop = Task.Run(() => ListenerLoop(token), token);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_listenerLoop == null)
        {
            return Task.CompletedTask;
        }

        _cancellationSource!.Cancel();
        return _listenerLoop;
    }

    public void Cancel()
    {
        _cancellationSource!.Cancel();
    }

    public void Dispose() => _cancellationSource?.Dispose();

    private async Task ListenerLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            MQMessage testMessage = new();
            testMessage.WriteString(JsonConvert.SerializeObject(_settings));
            testMessage.DataOffset = 0;

            // TriggeredFunctionData input = new()
            // {
            //     TriggerValue = _singleDispatch ? testMessage : new[] { testMessage }
            // };

            TriggeredFunctionData input = new()
            {
                TriggerValue = new MQTriggerInput(testMessage)
            };

            await _executor.TryExecuteAsync(input, token).ConfigureAwait(false);

            await Task.Delay(TimeSpan.FromSeconds(10), token).ConfigureAwait(false);
        }
    }
}