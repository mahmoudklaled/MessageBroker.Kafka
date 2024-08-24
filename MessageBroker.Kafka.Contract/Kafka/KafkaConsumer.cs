using Confluent.Kafka;
using MessageBroker.Kafka.Contract.Models;
using MessageBroker.Kafka.Contract.Serializer;

namespace MessageBroker.Kafka.Contract.Kafka
{
    public class KafkaConsumer<T> : IDisposable where T : Message
    {
        private readonly IConsumer<string, T> _consumer;
        private Action<T> _handleMethod;
        private bool _disposed = false;

        public KafkaConsumer(ConsumerConfig config, string topic)
        {
            _consumer = new ConsumerBuilder<string, T>(config)
                .SetValueDeserializer(new MessageDeserializer<T>())
                .Build();

            _consumer.Subscribe(topic);
            
        }

        public void Consume(CancellationToken cancellationToken, Action<T> handleMethod = null)
        {
            try
            {
                _handleMethod = handleMethod;
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
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
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Error occurred: {ex.Error.Reason}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred: {e.Message}");
            }
        }

        private void HandleMessage(T message)
        {
            _handleMethod?.Invoke(message);
        }

        public void Close()
        {
            _consumer.Close(); 
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
