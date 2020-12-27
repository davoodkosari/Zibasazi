using Radyn.SQL.Facade;
using Radyn.SQL.Facade.Interface;

namespace Radyn.SQL
{
    public class SQLComponent
    {
        private SQLComponent()
        {

        }

        private static SQLComponent _instance;
        public static SQLComponent Instance
        {
            get { return _instance ?? (_instance = new SQLComponent()); }
        }

        public ISQLFacade SQLFacade
        {
            get { return new SQLFacade(); }
        }
    }


}
