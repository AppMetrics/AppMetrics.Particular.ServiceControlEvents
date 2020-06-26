using System;
using NServiceBus;

namespace NServiceBusEndpoint
{
    public class SimpleMessageOne : IMessage
    {
        public Guid Id { get; set; }
    }
}