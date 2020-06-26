namespace NServiceBusEndpoint
{
    using System;
    using NServiceBus;

    public class SimpleEventOne : IEvent
    {
        public Guid Id { get; set; }
    }
}