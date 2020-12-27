using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class ConfigurationSupportTypeDA : DALBase<ConfigurationSupportType>
    {
        public ConfigurationSupportTypeDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class ConfigurationSupportTypeCommandBuilder
    {
    }
}
