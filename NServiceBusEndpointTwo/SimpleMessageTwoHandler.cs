using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace NServiceBusEndpointTwo
{
    public class SimpleMessageHandler : IHandleMessages<SimpleMessageTwo>
    {
        static readonly ILog log = LogManager.GetLogger<SimpleMessageHandler>();

        public Task Handle(SimpleMessageTwo message, IMessageHandlerContext context)
        {
            log.Info($"Received message with Id = {message.Id}.");
            throw new Exception("BOOM!");
        }
    }
}