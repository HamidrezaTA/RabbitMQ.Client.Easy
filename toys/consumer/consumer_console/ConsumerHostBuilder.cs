using common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Easy;

namespace consumer_console
{
    public static class ConsumerHostBuilder
    {
        public static IHostBuilder CreateHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var rabbitMqConfigurations = ConfigurationManager.GetConfiguration();
                services.AddRabbitMqClient(rabbitMqConfigurations).AddConsumer<ConsoleConsumer>("test-queue", true, false, true, true);
            });
    }
}