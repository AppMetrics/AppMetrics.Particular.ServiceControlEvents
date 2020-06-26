using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Formatters.Ascii;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    const string EndpointName = "EndpointsMonitor";

    static async Task Main(string[] args)
    {
        Console.Title = EndpointName;

        var host = Host.CreateDefaultBuilder(args).ConfigureMetricsWithDefaults(
            builder =>
            {
                builder.OutputMetrics.AsInfluxDbLineProtocol();
                builder.Report.ToInfluxDb("http://127.0.0.1:8086", "nservicebus", TimeSpan.FromSeconds(1));
                builder.Report.ToConsole(
                    options =>
                    {
                        options.FlushInterval = TimeSpan.FromSeconds(5);
                        options.MetricsOutputFormatter = new MetricsTextOutputFormatter();
                    });
            }).UseNServiceBus(
            context =>
            {
                var cfg = new EndpointConfiguration(EndpointName);
                cfg.UseSerialization<NewtonsoftSerializer>();
                cfg.EnableInstallers();
                cfg.UsePersistence<InMemoryPersistence>();
                cfg.SendFailedMessagesTo("error");

                var transport = cfg.UseTransport<LearningTransport>();

                var conventions = cfg.Conventions();
                conventions.DefiningEventsAs(
                    type =>
                    {
                        return typeof(IEvent).IsAssignableFrom(type) ||
                               // include ServiceControl events
                               type.Namespace != null &&
                               type.Namespace.StartsWith("ServiceControl.Contracts");
                    });

                return cfg;
            }).UseConsoleLifetime().Build();

        await host.StartAsync();

        Console.WriteLine("Press any key to finish.");
        Console.ReadKey();
    }
}