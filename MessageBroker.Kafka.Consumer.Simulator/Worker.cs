using MessageBroker.Kafka.Contract.Kafka;
using MessageBroker.Kafka.Contract.Models;

namespace MessageBroker.Kafka.Consumer.Simulator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly KafkaConsumer<InformationMessage> _informationConsumer;
        private readonly KafkaConsumer<LogMessage> _logConsumer;
        private readonly KafkaConsumer<ExceptionMessage> _exceptionConsumer;

        public Worker(ILogger<Worker> logger,
                      KafkaConsumer<InformationMessage> informationConsumer,
                      KafkaConsumer<LogMessage> logConsumer,
                      KafkaConsumer<ExceptionMessage> exceptionConsumer)
        {
            _logger = logger;
            _informationConsumer = informationConsumer;
            _logConsumer = logConsumer;
            _exceptionConsumer = exceptionConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var informationTask = Task.Run(() => _informationConsumer.Consume(stoppingToken), stoppingToken);
            var logTask = Task.Run(() => _logConsumer.Consume(stoppingToken), stoppingToken);
            var exceptionTask = Task.Run(() => _exceptionConsumer.Consume(stoppingToken), stoppingToken);

            await Task.WhenAll(informationTask, logTask, exceptionTask);
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consumer Worker stopping");
            _informationConsumer.Close();
            _logConsumer.Close();
            _exceptionConsumer.Close();
            await base.StopAsync(stoppingToken);
        }
    }
}
