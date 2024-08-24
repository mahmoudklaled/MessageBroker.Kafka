using MessageBroker.Kafka.Contract.Kafka;
using MessageBroker.Kafka.Contract.Models;

namespace MessageBroker.Kafka.Consumer.Simulator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly KafkaConsumer<Message> _messageConsumer;
        public Worker(ILogger<Worker> logger, KafkaConsumer<Message> messageConsumer)
        {
            _logger = logger;
            _messageConsumer = messageConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var messageTask = Task.Run(() => _messageConsumer.Consume(stoppingToken , HandleMessage), stoppingToken); ;
            await Task.WhenAll(messageTask);
        }

        private void HandleMessage(Message message)
        {
            if(message is ExceptionMessage)
                Console.WriteLine("Exception message :P");
            else if (message is InformationMessage)
                Console.WriteLine("Information message :P");
            else if (message is LogMessage)
                Console.WriteLine("Log message :P");
            else if (message is Message)
                Console.WriteLine("Message :P");
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consumer Worker stopping");
            _messageConsumer.Close();
            await base.StopAsync(stoppingToken);
        }
    }
}
