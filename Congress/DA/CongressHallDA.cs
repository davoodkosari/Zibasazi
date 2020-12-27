using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressHallDA : DALBase<CongressHall>
    {
        public CongressHallDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class CongressHallCommandBuilder
    {
    }
}
