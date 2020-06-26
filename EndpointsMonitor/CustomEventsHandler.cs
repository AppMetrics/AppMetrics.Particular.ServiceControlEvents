using System.Threading.Tasks;
using App.Metrics;
using EndpointsMonitor;
using NServiceBus;
using NServiceBus.Logging;
using ServiceControl.Contracts;

#region ServiceControlEventsHandlers

public class CustomEventsHandler :
    IHandleMessages<MessageFailed>,
    IHandleMessages<HeartbeatStopped>,
    IHandleMessages<HeartbeatRestored>
{
    static ILog log = LogManager.GetLogger<CustomEventsHandler>();

    static readonly string[] Tags =
    {
        "messagetype",
        "processingendpoint",
        "sendingendpoint"
    };

    readonly IMetrics Metrics;

    public CustomEventsHandler(IMetrics metrics)
    {
        Metrics = metrics;
    }

    public Task Handle(HeartbeatRestored message, IMessageHandlerContext context)
    {
        log.Info($"Heartbeats from {message.EndpointName} have been restored.");
        return Task.CompletedTask;
    }

    public Task Handle(HeartbeatStopped message, IMessageHandlerContext context)
    {
        log.Warn($"Heartbeats from {message.EndpointName} have stopped.");
        return Task.CompletedTask;
    }

    public Task Handle(MessageFailed message, IMessageHandlerContext context)
    {
        log.Error($"Received ServiceControl 'MessageFailed' event for a {message.MessageType} with ID {message.FailedMessageId}.");

        var tagValues = new[]
        {
            message.MessageType,
            message.ProcessingEndpoint.Name,
            message.SendingEndpoint.Name
        };
        
        var tags = new MetricTags(Tags, tagValues);
        Metrics.Measure.Counter.Increment(NServiceBusMetricsRegistry.Counters.ErrorsPerEndpoint, tags);

        return Task.CompletedTask;
    }
}

#endregion