namespace Contracts
{
    using System;

    public record TestEvent
    {
        public Guid CorrelationId { get; init; }
        public string Value { get; init; }
    }

    public record TestEvent2
    {
        public Guid CorrelationId { get; init; }
        public string Value { get; init; }
    }
}