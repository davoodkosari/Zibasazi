using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class PivotDA : DALBase<Pivot>
    {
        public PivotDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class PivotCommandBuilder
    {
    }
}
