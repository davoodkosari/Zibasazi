using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class ArticleTypeFacade : CongressBaseFacade<ArticleType>, IArticleTypeFacade
    {
        internal ArticleTypeFacade()
        {
        }

        internal ArticleTypeFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       
     

        
    }
}
