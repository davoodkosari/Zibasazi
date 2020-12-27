using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.News.DA
{
    public sealed class NewsDA : DALBase<News.DataStructure.News>
    {
        public NewsDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public IEnumerable<DataStructure.News> GetByCategory(Guid categoryId, int? topCount)
        {
            var newsCommandBuilder = new NewsCommandBuilder();
            var query = newsCommandBuilder.GetByCategory(categoryId, topCount);
            return DBManager.GetCollection<DataStructure.News>(base.ConnectionHandler, query);
        }

        public IEnumerable<DataStructure.News> GetTopNews(int topCount, bool isSelected)
        {
            var newsCommandBuilder = new NewsCommandBuilder();
            var query = newsCommandBuilder.GetTopNews(topCount, isSelected);
            return DBManager.GetCollection<DataStructure.News>(base.ConnectionHandler, query);
        }
  public async Task<IEnumerable<DataStructure.News>> GetTopNewsAsync(int topCount, bool isSelected)
        {
            var newsCommandBuilder = new NewsCommandBuilder();
            var query = newsCommandBuilder.GetTopNews(topCount, isSelected);
            return await DBManager.GetCollectionAsync<DataStructure.News>(base.ConnectionHandler, query);
        }

        public IEnumerable<DataStructure.News> Search(string text)
        {
            var newsCommandBuilder = new NewsCommandBuilder();
            var query = newsCommandBuilder.Search(text);
            return DBManager.GetCollection<DataStructure.News>(base.ConnectionHandler, query);
        }
    }
    internal class NewsCommandBuilder
    {
        public SqlCommand GetByCategory(Guid categoryId, int? topCount)
        {
            var query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@categoryId", categoryId));
            query.Parameters.Add(new SqlParameter("@topCount", topCount != null ? "top(" + topCount + ")" : ""));
            query.CommandText =
                "select distinct @topCount* from News.News where NewsCategoryId=@categoryId order by PublishDate desc ,PublishTime desc";
            return query;
        }

        public SqlCommand GetTopNews(int topCount, bool isSelected)
        {
            var query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@topCount", topCount));
            var q =
            "SELECT  distinct  top(@topCount)  News.News.* " +
                                      " FROM         News.News INNER JOIN " +
                                      " News.NewsProperty ON News.News.Id = News.NewsProperty.Id";
            string where = "News.News.Enabled=1 and ";
            if (isSelected) where += "News.NewsProperty.IsSelection=1 and ";
            where = where.Substring(0, where.Length - 4);
            query.CommandText = string.Format("{0} where {1} order by News.News.PublishDate desc ,News.News.PublishTime desc", q, where);
            return query;
        }
        public SqlCommand Search(string text)
        {
            var query = new SqlCommand();
            const string q = "SELECT   distinct  News.News.*" +
                                 " FROM         News.News INNER JOIN " +
                                 " News.NewsContent ON News.News.Id = News.NewsContent.Id";
            var where = "";
            var split = text.Split();
            var str = "";
            var counter = 100;
            foreach (var s in split)
            {
                var param = $"@Title{s}{++counter}";
                query.Parameters.Add(new SqlParameter(param, s));
                str += $"News.NewsContent.Title1 like N'%{param}%' or ";
            }
            str = str.Substring(0, str.Length - 3);
            where += string.Format("({0}) OR ", str);

            var strbody = "";
                foreach (var sbody in strbody)
                {
                    var param = $"@Title{sbody}{++counter}";
                    query.Parameters.Add(new SqlParameter(param, sbody));
                    strbody += $"News.NewsContent.Title2 like N'%{param}%' or ";
                }
            strbody = strbody.Substring(0, strbody.Length - 3);
            where += string.Format("({0}) or ", strbody);

            var OverTitle = "";
                foreach (var over in split)
                {
                    var param = $"@Tilte{over}{++counter}";
                    query.Parameters.Add(new SqlParameter(param, over));
                    OverTitle += $"News.NewsContent.OverTitle like N'%{param}%' or ";
                }
            OverTitle = OverTitle.Substring(0, OverTitle.Length - 3);
            where += string.Format("({0}) or ", OverTitle);

            var Lead = "";
                foreach (var lead in split)
                {
                    var param = $"@Tilte{lead}{++counter}";
                    query.Parameters.Add(new SqlParameter(param, lead));
                    Lead += $"News.NewsContent.Lead like N'%{param}%' or ";
                }
            Lead = Lead.Substring(0, Lead.Length - 3);
            where += string.Format("({0}) or ", Lead);

            var Sutitr = "";
                foreach (var su in split)
                {
                    var param = $"@Title{su}{++counter}";
                    query.Parameters.Add(new SqlParameter(param, su));
                    Sutitr += $"News.NewsContent.Sutitr like N'%{param}%' or ";
                }
            Sutitr = Sutitr.Substring(0, Sutitr.Length - 3);
            where += string.Format("({0}) or ", Sutitr);



            where = where.Substring(0, where.Length - 3);
            query.CommandText =
                string.Format("{0} where {1} order by News.News.PublishDate desc ,News.News.PublishTime desc", q,
                    where);
            return query;

        }
    }
}
