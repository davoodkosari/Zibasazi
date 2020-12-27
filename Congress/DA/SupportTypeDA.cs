using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class SupportTypeDA : DALBase<SupportType>
    {
        public SupportTypeDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class SupportTypeCommandBuilder
    {
    }
}
