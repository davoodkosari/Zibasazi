using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;
namespace Radyn.Congress.Facade
{
    internal sealed class ConfigurationSupportTypeFacade : CongressBaseFacade<ConfigurationSupportType>, IConfigurationSupportTypeFacade
    {
        internal ConfigurationSupportTypeFacade() { }

        internal ConfigurationSupportTypeFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

    }
}
