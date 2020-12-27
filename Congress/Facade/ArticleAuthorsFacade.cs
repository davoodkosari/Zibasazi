using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class ArticleAuthorsFacade : CongressBaseFacade<ArticleAuthors>, IArticleAuthorsFacade
    {
        internal ArticleAuthorsFacade() { }

        internal ArticleAuthorsFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

      

        
    }
}
