using System;
using System.Configuration;
using System.Linq;
using Quartz;
using Quartz.Impl;
using Radyn.Congress.Settings;

namespace Radyn.Congress.Agent.Definition
{
    internal static class DailyScheduler
    {

        private static JobSchedulerSetting JobScheduler
        {
            get
            {
                var jobScheduler = (JobSchedulerSetting)ConfigurationManager.GetSection("radyn/CongressScheduler");
                return jobScheduler;
            }
        }

        public static void Initial()
        {

           
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler();
                if (!scheduler.IsStarted)
                    scheduler.Start();
                for (var i = 0; i < JobScheduler.Triggers.Count; i++)
                {
                    var item = JobScheduler.Triggers[i];
                    var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Name.Equals(item.Class));
                    if (!types.Any())
                        throw new Exception("Radyn: " + item.Class + " not found in " + System.Reflection.Assembly.GetExecutingAssembly().FullName);
                    if (types.Count() > 1)
                        throw new Exception("Radyn: There are more than 1 from '" + item.Class + "' in " + System.Reflection.Assembly.GetExecutingAssembly().FullName);
                    var type = types.First();
                    var jobDetail = new JobDetail(item.Class + item.Id, item.Id, type);
                    var trigger = TriggerUtils.MakeDailyTrigger(item.Class + item.Id, item.Hour, item.Minute);
                    trigger.Priority = item.Priority;
                    scheduler.ScheduleJob(jobDetail, trigger);
                    var stack = Environment.StackTrace.Replace("\r", "").Split('\n').Where(x => x.Contains("Radyn"));
                    stack.ToList().Last();
                }

            
           
        }

    }
}
