namespace NServiceBusEndpointOne
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using NServiceBusEndpoint;

    public class SimpleMessageOneHandler : IHandleMessages<SimpleMessageOne>
    {
        static ILog log = LogManager.GetLogger<SimpleMessageOneHandler>();

        public Task Handle(SimpleMessageOne message, IMessageHandlerContext context)
        {
            log.Info($"Received message with Id = {message.Id}.");
            throw new Exception("Message BOOM!");
        }
    }
}