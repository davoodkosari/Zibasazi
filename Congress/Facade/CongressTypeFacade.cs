using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressTypeFacade : CongressBaseFacade<CongressType>, ICongressTypeFacade
    {
        internal CongressTypeFacade()
        {
        }

        internal CongressTypeFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       
     

        
    }
}
