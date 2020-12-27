using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class UserChairFacade : CongressBaseFacade<UserChair>, IUserChairFacade
    {
        internal UserChairFacade() { }

        internal UserChairFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

      
    }
}
