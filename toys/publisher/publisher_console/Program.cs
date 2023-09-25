using System.Text.Json;
using common;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Easy;

Console.WriteLine("Running ....");

var rabbitMqConfigurations = ConfigurationManager.GetConfiguration();
var serviceProvider = new ServiceCollection().AddRabbitMqClient(rabbitMqConfigurations)
                                             .BuildServiceProvider();

var publisher = serviceProvider.GetService<IPublisher>();


publisher.SetExchange("test-exchange", "direct", true)
         .SetQueue("test-queue", true, true)
         .BindQueueToExchange("test-routingKey");

var messageId = 0;

while (true)
{
    Console.WriteLine("Enter time to delay in second");
    var timeToDelay = Console.ReadLine();

    var message = new Message() { Id = messageId, TimeToDelayInSecond = int.Parse(timeToDelay) };

    await publisher.PublishAsync(JsonSerializer.Serialize(message));
    messageId++;

    Console.WriteLine("Press enter to Continue");
    var keyInfo = Console.ReadKey(intercept: true);
    if (keyInfo.Key != ConsoleKey.Enter)
        break;

}

Console.WriteLine("Shutting down ....");

