using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using onboarding.bll.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace onboarding.bll
{
    public class MovieConsumerService : IConsumeProcess
    {
        private readonly ILogger _logger;

        public MovieConsumerService(ILogger logger)
        {
            _logger = logger;
        }

        public Task ConsumeAsync<TKey>(ConsumeResult<TKey, string> consumeResult, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(consumeResult.Message.Value);
            return Task.CompletedTask;
        }
    }
}
