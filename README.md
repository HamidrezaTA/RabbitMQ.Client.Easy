# RabbitMQ.Client.Easy
The goal of this project is to make using of RabbitMQ client for dotnet core much easier.

# How to use
To use this project add RabbitMQ.Client.Easy to your project.
```
dotnet package add RabbitMQ.Client.Easy 
```
To use it in a dotnet core WebApi project:

- Use a IServiceCollection extension method named `AddRabbitMqClient()` and pass a `RabbitMqConfiguration` instance object to it as an argument.

```csharp
builder.Services.AddRabbitMqClient(new RabbitMqConfiguration()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
});
```
- Create a class and inherit the class from `Consumer` class and override the `Subscribe` method.
```csharp
public class SampleConsumer : Consumer
{
    public SampleConsumer(IConnection connection,
                              string queueName,
                              bool durable,
                              bool exclusive,
                              bool autoDelete,
                              bool autoAck) : base(connection, queueName, durable, exclusive, autoDelete,
                              autoAck)
    {

    }

    public override void Subscribe(byte[]? messageAsArray)
    {
        var message = Encoding.UTF8.GetString(messageAsArray);
        Console.WriteLine("Consumed {0}", message);
    }
}
```
- Use `AddConsumer<>()` extension method to set the class which you created before as a consumer on a queue name.
```csharp
builder.Services.AddRabbitMqClient(new RabbitMqConfiguration()
{
    HostName = "localhost",
    Port = 1566,
    UserName = "guest",
    Password = "guest"
}).AddConsumer<SampleConsumer>("sample-queue", true, true, false, false);
```
- In order to publish a message you should inject `IPublisher` to your controller or any another services. (For example here we injected IPublish interface in SampleController)
```csharp
[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly ILogger<SampleController> _logger;
    private readonly IPublisher _publisher;

    public SampleController(ILogger<SampleController> logger, IPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }
}
```
- And then we use publisher builder to send a message on a queue with a routing key to an extension.
```csharp
    [HttpPost]
    public async Task PostAction()
    {
        var message = "Hello from post action";
        await _publisher.SetExchange("sample-exchange", "direct", true)
                        .SetQueue("sample-queue", true, false)
                        .BindQueueToExchange("sample-routing-key").PublishAsync(message);
    }
```


