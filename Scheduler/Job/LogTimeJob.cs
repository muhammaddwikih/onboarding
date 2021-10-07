using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace onboarding.Scheduler.Job
{
    [DisallowConcurrentExecution]
    public class LogTimeJob : IJob
    {
        private readonly ILogger _logger;

        public LogTimeJob(ILogger<LogTimeJob> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Current Date : {DateTime.UtcNow}");
        }
    }
}
