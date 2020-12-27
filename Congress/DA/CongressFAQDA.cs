using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressFAQDA : DALBase<CongressFAQ>
    {
        public CongressFAQDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class CongressFAQCommandBuilder
    {
    }
}
