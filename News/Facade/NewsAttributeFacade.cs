using Radyn.Framework.DbHelper;
using Radyn.Gallery;
using Radyn.News.BO;
using Radyn.News.DataStructure;
using Radyn.News.Facade.Interface;

namespace Radyn.News.Facade
{
    internal sealed class NewsAttributeFacade : NewsBaseFacade<NewsAttribute>, INewsAttributeFacade
    {
        internal NewsAttributeFacade() { }

        internal NewsAttributeFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

       
    }
}
