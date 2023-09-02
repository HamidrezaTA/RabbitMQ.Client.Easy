using Microsoft.Extensions.DependencyInjection;

namespace RabbitMQ.Client.Easy
{
    public static class RabbitMqClient
    {
        public static IServiceCollection AddRabbitMqClient(this IServiceCollection services,
                                             RabbitMqConfiguration rabbitMqConfigurations)
        {
            services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = rabbitMqConfigurations.HostName,
                    Port = rabbitMqConfigurations.Port,
                    UserName = rabbitMqConfigurations.UserName,
                    Password = rabbitMqConfigurations.Password
                };

                return factory.CreateConnection();
            });

            services.AddScoped<IPublisher, Publisher>();

            return services;
        }

        public static IServiceCollection AddConsumer<T>(this IServiceCollection services,
                                                        string queueName,
                                                        bool durable,
                                                        bool exclusive,
                                                        bool autoDelete,
                                                        bool autoAck) where T : Consumer
        {
            services.AddSingleton<ConsumerFactory<T>>();
            services.AddHostedService(provider =>
            {
                var factory = provider.GetRequiredService<ConsumerFactory<T>>();
                return factory.Create(queueName, durable, exclusive, autoDelete, autoAck);
            });

            return services;
        }
    }
}