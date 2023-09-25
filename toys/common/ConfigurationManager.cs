using RabbitMQ.Client.Easy;

namespace common;

public static class ConfigurationManager
{
    public static RabbitMqConfiguration GetConfiguration()
    {
        return new RabbitMqConfiguration()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };
    }
}
