namespace EndpointsMonitor
{
    using App.Metrics;
    using App.Metrics.Counter;

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
    }
}