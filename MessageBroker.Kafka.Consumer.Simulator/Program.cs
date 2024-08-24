using Confluent.Kafka;
using MessageBroker.Kafka.Consumer.Simulator;
using MessageBroker.Kafka.Contract.Enums;
using MessageBroker.Kafka.Contract.Kafka;
using MessageBroker.Kafka.Contract.Models;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = "localhost:9092",
                    GroupId = "consumer-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = true, // Ensure auto-commit is enabled
                    AutoCommitIntervalMs = 5000 // Commit offsets every 5 seconds
                };
                
                services.AddSingleton(new KafkaConsumer<Message>(consumerConfig, Topics.All.ToString()));
                services.AddHostedService<Worker>();
            });
}