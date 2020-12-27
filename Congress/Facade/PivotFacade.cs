using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class PivotFacade : CongressBaseFacade<Pivot>, IPivotFacade
    {
        internal PivotFacade()
        {
        }

        internal PivotFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       


    }
}
