using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressTypeDA : DALBase<CongressType>
    {
        public CongressTypeDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class CongressTypeCommandBuilder
    {
    }
}
