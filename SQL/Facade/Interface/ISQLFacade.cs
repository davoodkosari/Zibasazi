using Radyn.SQL.Enums;

namespace Radyn.SQL.Facade.Interface
{
    public interface ISQLFacade
    {
         object Execute(string connection, string commend, ref SqlOutType sqlOutType);
    }
}
