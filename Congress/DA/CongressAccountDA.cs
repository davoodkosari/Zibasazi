using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressAccountDA : DALBase<CongressAccount>
    {
        public CongressAccountDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class CongressAccountCommandBuilder
    {
    }
}
