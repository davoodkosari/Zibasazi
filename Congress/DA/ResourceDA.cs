using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class ResourceDA : DALBase<Resource>
    {
        public ResourceDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class ResourceCommandBuilder
    {
    }
}
