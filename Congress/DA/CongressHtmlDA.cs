using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressHtmlDA : DALBase<CongressHtml>
    {
        public CongressHtmlDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class CongressHtmlCommandBuilder
    {
    }
}
