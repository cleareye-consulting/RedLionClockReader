using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClearEye.RedLionClockReader
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly TimeSpan refreshFrequency;
        private readonly WorkerHelper helper;

        public Worker(ILogger<Worker> logger, IClockReader clockReader, IValueSender valueSender, TimeSpan refreshFrequency, string deviceId)
        {
            this.logger = logger;
            this.refreshFrequency = refreshFrequency;
            helper = new WorkerHelper(clockReader, valueSender);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var previousValue = decimal.MinValue;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {                    
                    var value = helper.GetValue();
                    if (value != previousValue)
                    {
                        await helper.PostValue(value);
                        previousValue = value;
                    }                    
                }
                catch (TimeoutException ex)
                {
                    logger.LogWarning(ex.ToString());
                }
                catch (HttpRequestException ex)
                {
                    logger.LogWarning(ex.ToString());
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex.ToString());
                    break;
                }
                await Task.Delay((int)refreshFrequency.TotalMilliseconds, stoppingToken);
            }
        }
    }
}
