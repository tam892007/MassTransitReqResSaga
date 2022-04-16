namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using System;

    public class TestConsumer : IConsumer<Test1>, IConsumer<Test2>
    {
        readonly IRequestClient<TestEvent> _client;
        public TestConsumer(
            IRequestClient<TestEvent> client
        ) 
        {
            _client = client;
        }

        public async Task Consume(ConsumeContext<Test1> context)
        {
            await _client.GetResponse<TestSagaResponse>(new TestEvent
            {
                Value = "TestEvent1",
                CorrelationId = new Guid("67042694-a40e-4687-8eea-8f8e5bee6a6a")
            });

            context.Respond<TestResponse1>(new TestResponse1
            {
                Value = "TestResponse1"
            });
        }

        public async Task Consume(ConsumeContext<Test2> context)
        {
            await Task.Delay(5000);
            await context.Publish(new TestEvent2
            {
                Value = "TestEvent2",
                CorrelationId = new Guid("67042694-a40e-4687-8eea-8f8e5bee6a6a"),
            });
        }
    }
}