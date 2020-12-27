using System;
using Radyn.Congress.Agent.Definition;
using Radyn.Congress.Facade;
using Radyn.Congress.Facade.Interface;



namespace Radyn.Congress
{
    public class CongressComponent
    {

        internal CongressComponent()
        {

        }
        private static CongressComponent _instance;
        public static CongressComponent Instance
        {
            get
            {
                return _instance ?? new CongressComponent();
            }

        }
        private BaseInfoComponents _baseInfoComponents;
        public BaseInfoComponents BaseInfoComponents
        {
            get
            {
                return _baseInfoComponents ?? new BaseInfoComponents();
            }
        }
        private ReportComponents _reportComponents;
        public ReportComponents ReportComponents
        {
            get
            {
                return _reportComponents ?? new ReportComponents();
            }
        }

        public ICongressMenuFacade MenuFacade
        {
            get
            {
                return new CongressMenuFacade();
            }
        }
        public void Initialize()
        {
            try
            {
                DailyScheduler.Initial();
            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
            }
        }
       
    }
}
