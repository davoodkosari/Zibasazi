using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface ICongressNewsFacade : IBaseFacade<CongressNews>
    {
        bool Insert(Guid congressId, News.DataStructure.News news, News.DataStructure.NewsContent newsContent,
            News.DataStructure.NewsProperty property, HttpPostedFileBase @base);

        bool Update(Guid congressId, News.DataStructure.News news, News.DataStructure.NewsContent newsContent,
        News.DataStructure.NewsProperty property, HttpPostedFileBase @base);

        IEnumerable<News.DataStructure.News> TopCount(Guid congressId, int? top);
         Task<IEnumerable<News.DataStructure.News>> TopCountAsync(Guid congressId, int? top);
    }
}
