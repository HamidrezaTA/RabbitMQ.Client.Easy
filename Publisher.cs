using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Client.Easy
{
    public class Publisher : IPublisher
    {
        private string _queueName;
        private string _exchangeName;
        private string _routingKey;
        private readonly IModel _channel;
        public IConnection Connection { get; set; }

        public Publisher(IConnection connection)
        {
            Connection = connection;
            _channel = Connection.CreateModel();
        }

        public Task PublishAsync(object message)
        {
            var body = Encoding.UTF8.GetBytes(message.ToString());
            _channel.BasicPublish(exchange: _exchangeName, routingKey: _routingKey, basicProperties: null, body: body);

            return Task.CompletedTask;
        }

        public IPublisher SetQueue(string queueName, bool durable, bool autoDelete)
        {
            _queueName = queueName;
            _channel.QueueDeclare(queue: queueName, durable: durable, exclusive: false, autoDelete: autoDelete, arguments: null);
            return this;
        }

        public IPublisher SetExchange(string exchangeName, string exchangeType, bool durable)
        {
            _exchangeName = exchangeName;
            _channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType, durable: durable);
            return this;
        }

        public IPublisher BindQueueToExchange(string routingKey)
        {
            _routingKey = routingKey;
            _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: routingKey);
            return this;
        }
    }
}