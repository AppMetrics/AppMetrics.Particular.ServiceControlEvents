using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBusEndpoint;

namespace NServiceBusEndpointOne
{
    public class SimpleMessageOneHandler : IHandleMessages<SimpleMessageOne>
    {
        static readonly ILog log = LogManager.GetLogger<SimpleMessageOneHandler>();

        public Task Handle(SimpleMessageOne message, IMessageHandlerContext context)
        {
            log.Info($"Received message with Id = {message.Id}.");
            throw new Exception("Message BOOM!");
        }
    }
}