using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class ConfigurationDA : DALBase<Configuration>
    {
        public ConfigurationDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class ConfigurationCommandBuilder
    {
    }
}
