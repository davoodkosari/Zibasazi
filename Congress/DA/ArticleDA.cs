using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class ArticleDA : DALBase<Article>
    {
        public ArticleDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

    }
    internal class ArticleCommandBuilder
    {

    }
}
