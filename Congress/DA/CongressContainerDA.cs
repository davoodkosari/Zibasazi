using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressContainerDA : DALBase<CongressContainer>
    {
        public CongressContainerDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class CongressContainerCommandBuilder
    {
    }
}
