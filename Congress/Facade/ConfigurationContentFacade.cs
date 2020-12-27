using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class ConfigurationContentFacade : CongressBaseFacade<ConfigurationContent>, IConfigurationContentFacade
    {
        internal ConfigurationContentFacade() { }

        internal ConfigurationContentFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }


       
    }
}
