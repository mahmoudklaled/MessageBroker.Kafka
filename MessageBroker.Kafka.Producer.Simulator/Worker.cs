using MessageBroker.Kafka.Contract.Enums;
using MessageBroker.Kafka.Contract.Kafka;
using MessageBroker.Kafka.Contract.Models;

namespace MessageBroker.Kafka.Producer.Simulator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly KafkaProducer<InformationMessage> _informationProducer;
        private readonly KafkaProducer<LogMessage> _logProducer;
        private readonly KafkaProducer<ExceptionMessage> _exceptionProducer;

        public Worker(ILogger<Worker> logger, KafkaProducer<InformationMessage> informationProducer,
        KafkaProducer<LogMessage> logProducer, KafkaProducer<ExceptionMessage> exceptionProducer)
        {
            _logger = logger;
            _informationProducer = informationProducer;
            _logProducer = logProducer;
            _exceptionProducer = exceptionProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Simulate producing messages on the same topic
            var tasks = new List<Task>
                {
                    ProduceMessagesOnSingleTopic(stoppingToken),
                    ProduceMessagesOnMultipleTopics(stoppingToken)
                };

            await Task.WhenAll(tasks);
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker stopping");

            await base.StopAsync(stoppingToken);
        }

        private async Task ProduceMessagesOnSingleTopic(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = new Message<InformationMessage>(new InformationMessage
                {
                    Level= InformationLevel.Medium,
                    Message= "Testing Message"
                });
                await _informationProducer.ProduceAsync(Topics.Information, message);

                // Simulate high rate
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }

        private async Task ProduceMessagesOnMultipleTopics(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Produce to Information topic
                var informationMessage = new Message<InformationMessage> (new InformationMessage
                {
                    Level = InformationLevel.Medium,
                    Message = "Testing Message"
                });
                await _informationProducer.ProduceAsync(Topics.Information, informationMessage);

                // Produce to Logs topic
                var logMessage = new Message<LogMessage>(new LogMessage {
                    Message = "Log Message"
                });
                await _logProducer.ProduceAsync(Topics.Logs, logMessage);

                // Produce to Exception topic
                var exceptionMessage = new Message<ExceptionMessage>(new ExceptionMessage
                {
                    Message = "Exception Message"
                });
                await _exceptionProducer.ProduceAsync(Topics.Exception, exceptionMessage);

                // Simulate high rate
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }
    }
}
