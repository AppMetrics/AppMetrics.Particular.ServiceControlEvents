namespace NServiceBusEndpointOne
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBusEndpoint;

    class Program
    {
        const string EndpointName = "NServiceBusEndpointOne";

        static async Task Main()
        {
            Console.Title = EndpointName;
            var endpointConfiguration = new EndpointConfiguration(EndpointName);
            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");

            #region DisableRetries

            var recoverability = endpointConfiguration.Recoverability();

            recoverability.Delayed(
                customizations: retriesSettings => { retriesSettings.NumberOfRetries(0); });
            recoverability.Immediate(
                customizations: retriesSettings => { retriesSettings.NumberOfRetries(0); });

            #endregion

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");

            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key != ConsoleKey.Enter)
                {
                    break;
                }

                var guid = Guid.NewGuid();

                var simpleMessage = new SimpleMessageOne { Id = guid };
                await endpointInstance.Send(EndpointName, simpleMessage).ConfigureAwait(false);
                Console.WriteLine($"Sent a new message with Id = {guid}.");

                var simpleEvent = new SimpleEventOne {Id = guid};
                await endpointInstance.Publish(simpleEvent).ConfigureAwait(false);
                Console.WriteLine($"Published a new event with Id = {guid}.");
                
                Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
            }

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}