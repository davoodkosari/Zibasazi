using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class ArticleTypeDA : DALBase<ArticleType>
    {
        public ArticleTypeDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class ArticleTypeCommandBuilder
    {
    }
}
