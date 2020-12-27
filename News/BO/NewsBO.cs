using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.News.DA;

namespace Radyn.News.BO
{
internal class NewsBO : BusinessBase<News.DataStructure.News>
{
    public IEnumerable<DataStructure.News> GetByCategory(IConnectionHandler connectionHandler, Guid categoryId, int? topCount)
    {
        var newsDa = new NewsDA(connectionHandler);
        return newsDa.GetByCategory(categoryId, topCount);
    }

    public IEnumerable<DataStructure.News> GetTopNews(IConnectionHandler connectionHandler, int topCount, bool isSelected)
    {
        var newsDa = new NewsDA(connectionHandler);
        return newsDa.GetTopNews(topCount, isSelected);
    }public async Task<IEnumerable<DataStructure.News>> GetTopNewsAsync(IConnectionHandler connectionHandler, int topCount, bool isSelected)
    {
        var newsDa = new NewsDA(connectionHandler);
        return await newsDa.GetTopNewsAsync(topCount, isSelected);
    }

    public IEnumerable<DataStructure.News> Search(IConnectionHandler connectionHandler, string text)
    {
        var newsDa = new NewsDA(connectionHandler);
        return newsDa.Search(text);
    }

    public override bool Update(IConnectionHandler connectionHandler, DataStructure.News obj)
    {
        if (!base.Update(connectionHandler, obj)) return false;
        var oldobj = this.Get(connectionHandler, obj.Id);
        if (oldobj.ThumbnailId.HasValue && obj.ThumbnailId == null)
            FileManagerComponent.Instance.FileFacade.Delete(oldobj.ThumbnailId);
        return true;
       
    }
}
}
