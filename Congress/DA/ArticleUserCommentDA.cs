using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class ArticleUserCommentDA : DALBase<ArticleUserComment>
    {
        public ArticleUserCommentDA(IConnectionHandler connectionHandler)
         : base(connectionHandler)
        { }

    }
    internal class ArticleUserCommentCommandBuilder
    {

    }

}
