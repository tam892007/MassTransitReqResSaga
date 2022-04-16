namespace Company.StateMachines
{
    using System;
    using MassTransit;

    public class TestState :
        SagaStateMachineInstance 
    {
        public int CurrentState { get; set; }

        public string Value { get; set; }

        public Guid CorrelationId { get; set; }
        public Guid? RequestId { get; set; }
        public Uri ResponseAddress { get; set; }
    }
}