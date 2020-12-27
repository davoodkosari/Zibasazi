using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressNewsDA : DALBase<CongressNews>
    {
        public CongressNewsDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class CongressNewsCommandBuilder
    {
    }
}
