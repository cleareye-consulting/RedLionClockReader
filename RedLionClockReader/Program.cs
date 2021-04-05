using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RedLionClockReader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IClockReader>(provider =>
                        ClockReaderFactory.GetClockReader(hostContext.Configuration["DeviceModel"], hostContext.Configuration.GetValue<int>("DeviceAddress")));
                    var deviceId = hostContext.Configuration["deviceId"];
                    var endpoint = hostContext.Configuration["endpoint"];
                    services.AddSingleton<IValueSender>(provider => new ValueSender(deviceId, endpoint));
                    var refreshFrequency = TimeSpan.FromSeconds(hostContext.Configuration.GetValue<int>("RefreshFrequencyInSeconds"));
                    services.AddHostedService<Worker>(provider => new Worker(
                        provider.GetRequiredService<ILogger<Worker>>(),
                        provider.GetRequiredService<IClockReader>(),
                        provider.GetRequiredService<IValueSender>(),
                        refreshFrequency,
                        deviceId));
                });
    }
}
