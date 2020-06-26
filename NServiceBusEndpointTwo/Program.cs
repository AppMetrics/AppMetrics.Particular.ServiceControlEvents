using System;
using System.Threading.Tasks;
using NServiceBus;

namespace NServiceBusEndpointTwo
{
    class Program
    {
        const string EndpointName = "NServiceBusEndpointTwo";

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

                var simpleMessage = new SimpleMessageTwo { Id = guid };
                await endpointInstance.Send(EndpointName, simpleMessage).ConfigureAwait(false);
                Console.WriteLine($"Sent a new message with Id = {guid}.");
                Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
            }

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}