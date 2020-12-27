using Radyn.Congress.DataStructure;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System;
using System.Collections.Generic;

namespace Radyn.Congress.DA
{
    public sealed class CongressContentDA : DALBase<CongressContent>
    {
        public CongressContentDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
        public List<Content> Search(string txtSearch, Guid id)
        {
            CongressContentCommandBuilder congressContentCommandBuilder = new CongressContentCommandBuilder();
            string query = congressContentCommandBuilder.Search(txtSearch, id);
            return DBManager.GetCollection<Content>(base.ConnectionHandler, query);
        }

        public List<News.DataStructure.News> SearchNews(string txtSearch, Guid id)
        {
            CongressContentCommandBuilder congressContentCommandBuilder = new CongressContentCommandBuilder();
            string query = congressContentCommandBuilder.SearchNews(txtSearch, id);
            return DBManager.GetCollection<News.DataStructure.News>(base.ConnectionHandler, query);
        }
    }
    internal class CongressContentCommandBuilder
    {
        public string Search(string txtSearch, Guid id)
        {
            return string.Format(" SELECT         ContentManage.[Content].* " +
 " FROM            ContentManage.ContentContent INNER JOIN " +
                        " ContentManage.[Content] ON ContentManage.ContentContent.Id = ContentManage.[Content].Id INNER JOIN " +
                       "  Congress.CongressContent ON ContentManage.[Content].Id = Congress.CongressContent.ContentId " +
                   "  WHERE        ((ContentManage.[Content].Title LIKE N'%{0}%') OR (ContentManage.[Content].Keyword LIKE N'%{0}%') OR (ContentManage.[Content].Abstract LIKE N'%{0}%') OR (ContentManage.ContentContent.Abstract LIKE N'%{0}%') " +
                    "     OR (ContentManage.ContentContent.Text LIKE N'%{0}%') OR (ContentManage.ContentContent.Description LIKE N'%{0}%') OR (ContentManage.ContentContent.Title LIKE N'%{0}%') OR " +
                  "       (ContentManage.[Content].Subject LIKE N'%{0}%') OR (ContentManage.[Content].Text LIKE N'%{0}%') OR (ContentManage.[Content].Description LIKE N'%{0}%')) And (Congress.CongressContent.CongressId = '{1}')", txtSearch, id);




        }

        public string SearchNews(string txtSearch, Guid id)
        {
            return string.Format(" SELECT   News.News.* " +
                                 " FROM            News.News INNER JOIN " +
                          " Congress.CongressNews ON News.News.Id = Congress.CongressNews.NewsId INNER JOIN " +
                        " News.NewsContent ON News.News.Id = News.NewsContent.Id INNER JOIN " +
                        " News.NewsContentType ON News.News.Id = News.NewsContentType.Id " +
          " WHERE        (Congress.CongressNews.CongressId = '{0}') AND ((News.NewsContent.Title1 = N'{1}') OR (News.NewsContent.Title2 = N'{1}') OR (News.NewsContent.Body LIKE '{1}') OR (News.NewsContent.OverTitle = N'{1}') OR " +
                         "(News.NewsContentType.Title = N'{1}'))", id, txtSearch);
        }

    }
}
