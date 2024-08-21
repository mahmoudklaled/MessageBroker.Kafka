using Confluent.Kafka;
using MessageBroker.Kafka.Contract.Models;
using MessageBroker.Kafka.Contract.Serializer;
using Newtonsoft.Json;

namespace MessageBroker.Kafka.Contract.Kafka
{
    public class KafkaConsumer<T> : IDisposable
    {
        private readonly IConsumer<string, Message<T>> _consumer;
        private bool _disposed = false;

        public KafkaConsumer(ConsumerConfig config, string topic)
        {
            _consumer = new ConsumerBuilder<string, Message<T>>(config)
                .SetValueDeserializer(new MessageDeserializer<T>())
                .Build();

            _consumer.Subscribe(topic);
        }

        public void Consume(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    if (consumeResult != null)
                    {
                        var message = consumeResult.Message.Value;
                        Console.WriteLine($"Consumed message with ID: {message.Id} at {consumeResult.TopicPartitionOffset}");

                        // Process the message here
                        HandleMessage(message);
                    }
                }
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error occurred: {e.Error.Reason}");
            }
        }
        public void Close()
        {
            _consumer.Close();  // Gracefully shuts down the consumer
        }

        private void HandleMessage(Message<T> message)
        {
            // Implement your message processing logic here
            Console.WriteLine($"Processing message: {JsonConvert.SerializeObject(message)}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _consumer?.Close();
                    _consumer?.Dispose();
                }
                _disposed = true;
            }
        }

        ~KafkaConsumer()
        {
            Dispose(false);
        }
    }
}
