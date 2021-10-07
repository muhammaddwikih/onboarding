﻿using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace onboarding.bll.Kafka
{
    public class KafkaSender : IkafkaSender, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ProducerConfig _producerConfig;
        private readonly int _timeoutMs = 30000;
        private readonly ILogger _logger;
        private readonly IProducer<string, string> _producer;

        public KafkaSender(IConfiguration configuration, ILogger<KafkaSender> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var bootstrapServers = _configuration.GetValue<string>("Kafka:BootstrapServers");

            _producerConfig = new ProducerConfig
            {
                Acks = Acks.All,
                EnableIdempotence = true,
                BootstrapServers = bootstrapServers,
                MessageTimeoutMs = _timeoutMs,
                Partitioner = Partitioner.ConsistentRandom
            };

            _producer = new ProducerBuilder<string, string>(_producerConfig).Build();
        }

        public async Task SendAsync(string topic, object message)
        {
            try
            {
                string stringValue = JsonConvert.SerializeObject(message, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                _logger.LogDebug($"Sending. Topic: {topic} Message: {stringValue}");

                var msg = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = stringValue
                };

                DeliveryResult<string, string> deliveryResult = await _producer.ProduceAsync(topic, msg);
                _logger.LogTrace($"Sent. Topic: {deliveryResult.Topic} Partition: {deliveryResult.Partition.Value} Offset: {deliveryResult.Offset.Value} Message: {deliveryResult.Message.Value}");
            }
            catch (ProduceException<Null, string> e)
            {
                if (e.Error.Code == ErrorCode.Local_MsgTimedOut)
                {
                    throw new TimeoutException($"Error code \"{e.Error.Code}\". Failed to send within {_timeoutMs} milliseconds.", e);
                }
                else
                {
                    throw new Exception($"Error code \"{e.Error.Code}\". Failed to send message.", e);
                }
            }
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _producer?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
