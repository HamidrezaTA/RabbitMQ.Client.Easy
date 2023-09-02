namespace RabbitMQ.Client.Easy
{
    public interface IPublisher
    {
        IPublisher SetQueue(string queueName, bool durable, bool autoDelete);
        IPublisher SetExchange(string exchangeName, string exchangeType, bool durable);
        IPublisher BindQueueToExchange(string routingKey);
        Task PublishAsync(object message);
    }
}