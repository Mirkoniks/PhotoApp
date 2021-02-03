using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoApp.Services.ChallangeService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoApp.Services.UpdateService
{
    public class ChallangeUpdateService : IHostedService
    {
        private Timer _timer;
        private readonly IChallangeService challangeService;

        public ChallangeUpdateService(IServiceScopeFactory factory)
        {
            this.challangeService = factory.CreateScope().ServiceProvider.GetRequiredService<IChallangeService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;
            DateTime tomorow = DateTime.Now.Date.AddDays(1);

            double milisecondsToGo = tomorow.Subtract(now).TotalMilliseconds;

            _timer = new Timer(UpdateChallange, null, 0, (int)milisecondsToGo);
            return Task.CompletedTask;
        }

        void UpdateChallange(object state)
        {
            challangeService.RunChallagesCheckAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //New Timer does not have a stop. 
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
