using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class ArticleAuthorsDA : DALBase<ArticleAuthors>
    {
        public ArticleAuthorsDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class ArticleAuthorsCommandBuilder
    {
    }
}
