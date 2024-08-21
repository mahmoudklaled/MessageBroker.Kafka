using Confluent.Kafka;
using MessageBroker.Kafka.Contract.Enums;
using MessageBroker.Kafka.Contract.Models;
using MessageBroker.Kafka.Contract.Serializer;

namespace MessageBroker.Kafka.Contract.Kafka
{
    public sealed class KafkaProducer<T>
    {
        private readonly IProducer<string, Message<T>> _producer;

        public KafkaProducer(ProducerConfig config)
        {
            _producer = new ProducerBuilder<string, Message<T>>(config)
                .SetValueSerializer(new MessageSerializer<T>())
                .Build();
        }

        public async Task ProduceAsync(Topics topic, Message<T> message)
        {
            try
            {
                var topicName = topic.ToString();
                var result = await _producer.ProduceAsync(topicName, new Message<string, Message<T>>
                {
                    Key = message.Id.ToString(),
                    Value = message
                });

                Console.WriteLine($"Message with ID: {message.Id} sent to topic: {topicName} at partition: {result.Partition} and offset: {result.Offset}");
            }
            catch (ProduceException<string, Message<T>> e)
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
