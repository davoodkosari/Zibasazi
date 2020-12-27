using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class HomaAliasDA : DALBase<HomaAlias>
    {
        public HomaAliasDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class HomaAliasCommandBuilder
    {
    }
}
