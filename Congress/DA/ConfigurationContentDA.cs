using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class ConfigurationContentDA : DALBase<ConfigurationContent>
    {
        public ConfigurationContentDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class ConfigurationContentCommandBuilder
    {
    }
}
