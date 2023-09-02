using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMQ.Client.Easy
{
    public class ConsumerFactory<T> where T : Consumer
    {
        private readonly IConnection _connection;
        public ConsumerFactory(IConnection connection)
        {
            _connection = connection;
        }

        public T Create(string queueName, bool durable, bool exclusive, bool autoDelete, bool autoAck)
        {
            return (T)Activator.CreateInstance(typeof(T), _connection, queueName, durable, exclusive, autoDelete, autoAck);
        }
    }
}