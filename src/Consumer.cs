using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Client.Easy
{
    public abstract class Consumer : IHostedService
    {
        private IConnection _connection;
        private readonly string _queueName;
        private readonly bool _durable;
        private readonly bool _exclusive;
        private readonly bool _autoDelete;
        private readonly bool _autoAck;

        public Consumer(IConnection connection,
                                       string queueName,
                                       bool durable,
                                       bool exclusive,
                                       bool autoDelete,
                                       bool autoAck)
        {
            _autoDelete = autoDelete;
            _autoAck = autoAck;
            _exclusive = exclusive;
            _durable = durable;
            _queueName = queueName;
            _connection = connection;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartSubscribing();
            return Task.CompletedTask;
        }

        private void StartSubscribing()
        {
            var channel = _connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: _durable, exclusive: _exclusive, autoDelete: _autoDelete, arguments: null);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var messageAsArray = body.ToArray();
                await SubscribeAsync(messageAsArray);
            };

            channel.BasicConsume(queue: _queueName, autoAck: _autoAck, consumer: consumer);
        }

        public abstract Task SubscribeAsync(byte[]? messageAsArray);

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            return Task.CompletedTask;
        }
    }
}