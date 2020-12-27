using System;
using Quartz;
using Radyn.Congress.Facade;

namespace Radyn.Congress.Agent.Jobs
{
    public sealed class DateChanged : IJob
    {
        public void Execute(JobExecutionContext context)
        {
            
            try
            {
                
                new HomaFacade().DailyEvaulation();
            }
            catch (Exception)
            {
            }

           
        }
    }
}
