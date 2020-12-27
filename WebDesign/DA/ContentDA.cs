using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.DA
{
    public sealed class ContentDA : DALBase<Content>
    {
        public ContentDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class ContentCommandBuilder
    {
    }
}
