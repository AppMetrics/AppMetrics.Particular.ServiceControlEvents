using System.Threading.Tasks;
using App.Metrics;
using NServiceBus;
using NServiceBus.Logging;
using ServiceControl.Contracts;

namespace EndpointsMonitor
{
    public class AppMetricsEventHandler :
        IHandleMessages<MessageFailed>,
        IHandleMessages<HeartbeatStopped>,
        IHandleMessages<HeartbeatRestored>
    {
        private static readonly string[] ErrorTags =
        {
            "messagetype",
            "processingendpoint",
            "sendingendpoint"
        };

        private static readonly ILog log = LogManager.GetLogger<AppMetricsEventHandler>();

        private readonly IMetrics _metrics;

        public AppMetricsEventHandler(IMetrics metrics) { _metrics = metrics; }

        public Task Handle(HeartbeatRestored message, IMessageHandlerContext context)
        {
            log.Info($"Heartbeats from {message.EndpointName} have been restored.");

            var tags = new MetricTags("endpoint", message.EndpointName);
            _metrics.Measure.Gauge.SetValue(NServiceBusMetricsRegistry.Gauges.HeartBeat, tags, 1);

            return Task.CompletedTask;
        }

        public Task Handle(HeartbeatStopped message, IMessageHandlerContext context)
        {
            log.Warn($"Heartbeats from {message.EndpointName} have stopped.");

            var tags = new MetricTags("endpoint", message.EndpointName);
            _metrics.Measure.Gauge.SetValue(NServiceBusMetricsRegistry.Gauges.HeartBeat, tags, -1);

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

            var tags = new MetricTags(ErrorTags, tagValues);
            _metrics.Measure.Counter.Increment(NServiceBusMetricsRegistry.Counters.ErrorsPerEndpoint, tags);

            return Task.CompletedTask;
        }
    }
}