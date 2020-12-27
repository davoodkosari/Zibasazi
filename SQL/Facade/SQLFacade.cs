using System.Data.SqlTypes;
using Radyn.SQL.BO;
using Radyn.SQL.Enums;
using Radyn.SQL.Facade.Interface;
namespace Radyn.SQL.Facade
{
    public class SQLFacade : ISQLFacade
    {
        public object Execute(string connection, string commend, ref SqlOutType sqlOutType)
        {
            return new SQLBO().ExecuteCommand(connection,commend,ref sqlOutType);
        }
    }
}
