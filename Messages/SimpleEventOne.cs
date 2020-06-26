using System;
using NServiceBus;

namespace NServiceBusEndpoint
{
    public class SimpleEventOne : IEvent
    {
        public Guid Id { get; set; }
    }
}