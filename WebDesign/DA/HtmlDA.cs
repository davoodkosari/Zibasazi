using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.DA
{
    public sealed class HtmlDA : DALBase<Html>
    {
        public HtmlDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class HtmlCommandBuilder
    {
    }
}
