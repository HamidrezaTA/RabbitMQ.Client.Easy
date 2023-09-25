using common;
using consumer_console;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Consumer is running...");

ConsumerHostBuilder.CreateHost(args).Build().Run();

Console.WriteLine("Press enter to shut it down ....");
Console.ReadLine();
Console.WriteLine("Shutting down ....");
