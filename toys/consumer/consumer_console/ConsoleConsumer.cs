using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using common;
using RabbitMQ.Client.Easy;


namespace consumer_console
{
    public class ConsoleConsumer : Consumer
    {
        public ConsoleConsumer(
            RabbitMQ.Client.IConnection connection, string queueName, bool durable, bool exclusive, bool autoDelete,
            bool autoAck) : base(connection, queueName, durable, exclusive, autoDelete, autoAck)
        {

        }

        public override async Task SubscribeAsync(byte[]? messageAsArray)
        {
            var message = JsonSerializer.Deserialize<Message>(Encoding.UTF8.GetString(messageAsArray));

            Console.WriteLine($"Processing message with Id: {message.Id} with delay: {message.TimeToDelayInSecond}");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await client.GetStringAsync($"https://f3b3cqc9-8080.uks1.devtunnels.ms/{message.Id}/{message.TimeToDelayInSecond}");

            Console.WriteLine($"Message processed with Id: {message.Id}");
        }
    }
}