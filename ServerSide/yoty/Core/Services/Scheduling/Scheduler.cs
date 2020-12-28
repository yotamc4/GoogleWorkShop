using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace YOTY.Service.Core.Services.Scheduling
{
    public class Scheduler : IScheduler
    {
        public Scheduler()
        {
            RecurringJob.AddOrUpdate(() => BidsUpdateJobs.UpdateBidsPhaseDaily(), Cron.Hourly, TimeZoneInfo.Local);
            Console.WriteLine("Scheduled Recurring Jobs");
        }
    }
}
