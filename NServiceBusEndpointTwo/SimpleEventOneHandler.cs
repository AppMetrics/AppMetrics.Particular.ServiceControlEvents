using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBusEndpoint;

namespace NServiceBusEndpointTwo
{
    public class SimpleEventOneHandler : IHandleMessages<SimpleEventOne>
    {
        static readonly ILog log = LogManager.GetLogger<SimpleEventOneHandler>();

        public Task Handle(SimpleEventOne message, IMessageHandlerContext context)
        {
            log.Info($"Received event with Id = {message.Id}.");
            throw new Exception("Event BOOM!");
        }
    }
}