namespace Company.StateMachines
{
    using System;
    using Contracts;
    using MassTransit;

    public class TestStateMachine :
        MassTransitStateMachine<TestState> 
    {
        public TestStateMachine()
        {
            InstanceState(x => x.CurrentState, Created, Completed);

            Event(() => TestEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => TestEvent2, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(TestEvent)
                    .Then(ctx => {
                        
                        if (!ctx.TryGetPayload(out SagaConsumeContext<TestState, TestEvent> payload))
                            throw new Exception("Unable to retrieve required payload for callback data.");

                        ctx.Saga.RequestId = payload.RequestId;
                        ctx.Saga.ResponseAddress = payload.ResponseAddress;

                    })
                    .ThenAsync(ctx => ctx.Publish(new Test2 { Value = "Test2" }))
                    .TransitionTo(Created)
            );

            During(Created,
                When(TestEvent2)
                    .Then(context => context.RespondAsync(new TestSagaResponse
                    {
                        Value = "TestSagaResponse"
                    }))
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }

        public State Created { get; private set; }
        public State Completed { get; private set; }

        public Event<TestEvent> TestEvent { get; private set; }
        public Event<TestEvent2> TestEvent2 { get; private set; }
    }
}