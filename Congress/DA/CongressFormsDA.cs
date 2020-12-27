using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressFormsDA : DALBase<CongressForms>
    {
        public CongressFormsDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class CongressFormsCommandBuilder
    {
    }
}
