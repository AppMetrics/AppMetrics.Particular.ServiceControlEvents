using System;
using NServiceBus;

public class SimpleMessageTwo : IMessage
{
    public Guid Id { get; set; }
}