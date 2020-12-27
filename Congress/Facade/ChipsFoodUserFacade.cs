using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class ChipsFoodUserFacade : CongressBaseFacade<ChipsFoodUser>, IChipsFoodUserFacade
    {
        internal ChipsFoodUserFacade() { }

        internal ChipsFoodUserFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

       
        

       
    }
}
