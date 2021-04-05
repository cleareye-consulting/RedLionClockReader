using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClearEye.RedLionClockReader
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly TimeSpan refreshFrequency;
        private readonly WorkerHelper helper;

        public Worker(ILogger<Worker> logger, IClockReader clockReader, IValueSender valueSender, TimeSpan refreshFrequency, string deviceId)
        {
            _logger = logger;
            this.refreshFrequency = refreshFrequency;
            helper = new WorkerHelper(clockReader, valueSender);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var value = helper.GetValue();
                await helper.PostValue(value);
                await Task.Delay((int)refreshFrequency.TotalMilliseconds, stoppingToken);
            }
        }
    }
}
