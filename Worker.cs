using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Test
{
    public class Worker : BackgroundService
    {
        private IRequestClient<Test1> _testClient1;
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                _testClient1 = scope.ServiceProvider.GetRequiredService<IRequestClient<Test1>>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    await _testClient1.GetResponse<TestResponse1>(new Test1 { Value = "Test1" }, stoppingToken);
                    await Task.Delay(10000, stoppingToken);
                }
            }
        }
    }
}