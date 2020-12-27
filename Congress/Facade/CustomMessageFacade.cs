using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    public sealed class CustomMessageFacade : CongressBaseFacade<CustomMessage>, ICustomMessageFacade
    {
        public CustomMessageFacade()
        {
        }

        internal CustomMessageFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }
    }
}
