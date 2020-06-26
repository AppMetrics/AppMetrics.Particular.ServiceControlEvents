using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;

namespace EndpointsMonitor
{
    public static class NServiceBusMetricsRegistry
    {
        public static string ContextName = "NServiceBusEndpointMonitor";

        public static class Counters
        {
            public static readonly CounterOptions ErrorsPerEndpoint = new CounterOptions
                                                                      {
                                                                          Context = ContextName,
                                                                          Name = "Message Failures",
                                                                          MeasurementUnit = Unit.Errors,
                                                                          ResetOnReporting = true
                                                                      };
        }

        public static class Gauges
        {
            public static readonly GaugeOptions HeartBeat = new GaugeOptions
                                                            {
                                                                Context = ContextName,
                                                                Name = "Heartbeat"
                                                            };
        }
    }
}