using Kemar.TMS.Business.Interfaces;
using Kemar.UrgeTruck.Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Kemar.TMS.Business.Services
{
    public class CheckTaskStatus : BackgroundService
    {
        public IServiceProvider Services { get; }
        private readonly IConfiguration _configuration;
        private const int CYCLE_SLEEP_TIME = 1 * 60 * 1000 * 60;

        public CheckTaskStatus(IServiceProvider services, IConfiguration configuration)
        {
            Services = services;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var sleeptime = _configuration.GetSection("AppSettings").GetSection("TaskServiceSleepTime").Value;
            int time = CYCLE_SLEEP_TIME;
            if (sleeptime != null)
                time = Convert.ToInt32(sleeptime) * 1000 * 60;
            using (var scope = Services.CreateScope())
            {
                var SLAException =
                    scope.ServiceProvider
                        .GetRequiredService<ICheckTaskStatus>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        await SLAException.StartServiceAsync();
                    }
                    catch (Exception ex)
                    {
                        Logger.Information("CheckTaskStatus Service Falied - " + ex);
                    }
                    await Task.Delay(time, stoppingToken);
                }
            }
        }
    }
}
