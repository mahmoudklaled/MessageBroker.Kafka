using Confluent.Kafka;
using MessageBroker.Kafka.Contract.Enums;
using MessageBroker.Kafka.Contract.Models;
using MessageBroker.Kafka.Contract.Serializer;
using Newtonsoft.Json;

namespace MessageBroker.Kafka.Contract.Kafka
{
    public sealed class KafkaProducer
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducer(ProducerConfig config)
        {
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task ProduceAsync<T>(Topics topic, T message) where T : Message
        {
            try
            {
                var topicName = topic.ToString();
                var jsonMessage = JsonConvert.SerializeObject(message);

                var result = await _producer.ProduceAsync(topicName, new Message<string, string>
                {
                    Key = message.Id.ToString(),
                    Value = jsonMessage
                });

                Console.WriteLine($"Message with ID: {message.Id} sent to topic: {topicName} at partition: {result.Partition} and offset: {result.Offset}");
            }
            catch (ProduceException<string, string> e)
            {
                Console.WriteLine($"Failed to deliver message: {e.Message} [{e.Error.Code}]");
            }
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}
