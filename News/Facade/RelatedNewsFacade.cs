using Radyn.Framework.DbHelper;
using Radyn.News.BO;
using Radyn.News.DataStructure;
using Radyn.News.Facade.Interface;

namespace Radyn.News.Facade
{
    internal sealed class RelatedNewsFacade : NewsBaseFacade<RelatedNews>, IRelatedNewsFacade
    {
        internal RelatedNewsFacade() { }

        internal RelatedNewsFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

      
    }
}
