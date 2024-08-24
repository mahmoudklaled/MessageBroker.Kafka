using Confluent.Kafka;
using MessageBroker.Kafka.Contract.Kafka;
using MessageBroker.Kafka.Contract.Models;
using MessageBroker.Kafka.Producer.Simulator;

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
            var producerConfig = new ProducerConfig 
            { 
                BootstrapServers = "localhost:9092" ,
                EnableDeliveryReports = false, // Disabling delivery reports can improve performance in cases where you do not need to track message delivery success.
                CompressionType = CompressionType.Lz4 // or CompressionType.Zstd or a good balance between compression efficiency and speed
            };
            
            //var producerConfig = new ProducerConfig
            //{
            //    BootstrapServers = "localhost:9092",
            //    Acks = Acks.None,
            //    LingerMs = 20,
            //    BatchSize = 131072, // 128KB
            //    CompressionType = CompressionType.Lz4,
            //    MessageSendMaxRetries = 5,
            //    RetryBackoffMs = 100,
            //    EnableIdempotence = true,
            //    SocketSendBufferBytes = 1048576, // 1MB
            //    SocketReceiveBufferBytes = 1048576, // 1MB
            //    QueueBufferingMaxMessages = 100000,
            //    QueueBufferingMaxKbytes = 1048576, // 1GB
            //    EnableDeliveryReports = false
            //};

            services.AddSingleton(new KafkaProducer(producerConfig));
            //services.AddSingleton(new KafkaProducer(producerConfig));
            //services.AddSingleton(new KafkaProducer(producerConfig));

            services.AddHostedService<Worker>();
        });
}
