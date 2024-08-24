using MessageBroker.Kafka.Contract.Enums;
using MessageBroker.Kafka.Contract.Kafka;
using MessageBroker.Kafka.Contract.Models;

namespace MessageBroker.Kafka.Producer.Simulator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly KafkaProducer _kafkaProducer;

        public Worker(ILogger<Worker> logger, KafkaProducer informationProducer)
        {
            _logger = logger;
            _kafkaProducer = informationProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Simulate producing messages on the same topic
            var tasks = new List<Task>
                {
                    ProduceInformationMessages(stoppingToken),
                    ProduceDifferentMessages(stoppingToken)
                };

            await Task.WhenAll(tasks);
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker stopping");

            await base.StopAsync(stoppingToken);
        }
        private async Task ProduceInformationMessages(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = new InformationMessage
                {
                    Level = InformationLevel.Medium,
                    Message = "Testing Message"
                };
                await _kafkaProducer.ProduceAsync(Topics.All, message);

                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }

        private async Task ProduceDifferentMessages(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var informationMessage = new InformationMessage
                {
                    Level = InformationLevel.Medium,
                    Message = "Information Message"
                };
                await _kafkaProducer.ProduceAsync(Topics.All, informationMessage);

                var logMessage = new LogMessage
                {
                    Message = "Log Message"
                };
                await _kafkaProducer.ProduceAsync(Topics.All, logMessage);

                var exceptionMessage = new ExceptionMessage
                {
                    Message = "Exception Message"
                };
                await _kafkaProducer.ProduceAsync(Topics.All, exceptionMessage);

                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }
    }
}
