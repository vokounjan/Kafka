using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProducer
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "192.168.0.241:9092",
            };

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    while (true)
                    {
                        var dr = await p.ProduceAsync("TestTopic", new Message<Null, string> { Value = "test" });
                        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");

                        await Task.Delay(TimeSpan.FromSeconds(5));
                    }
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}