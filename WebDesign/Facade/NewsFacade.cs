using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.News;
using Radyn.News.DataStructure;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.Facade.Interface;

namespace Radyn.WebDesign.Facade
{
    internal sealed class NewsFacade : WebDesignBaseFacade<DataStructure.News>, INewsFacade
    {
        internal NewsFacade() { }

        internal NewsFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }


        public bool Insert(Guid websiteId, News.DataStructure.News news, NewsContent newsContent, NewsProperty newsproperty, HttpPostedFileBase file)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.NewsConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                news.IsExternal = true;
                if (!NewsComponent.Instance.NewsTransactionalFacade(this.NewsConnection).Insert(news, newsContent, newsproperty, file))
                    throw new Exception("خطایی درذخیره اخبار وجود دارد");
                var congressNews = new Radyn.WebDesign.DataStructure.News { NewsId = news.Id, WebId = websiteId };
                if (!new NewsBO().Insert(this.ConnectionHandler, congressNews))
                    throw new Exception("خطایی درذخیره اخبار وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.NewsConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.NewsConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.NewsConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<News.DataStructure.News> TopCount(Guid websiteId, int? top)
        {
            try
            {
                var enumerable = new NewsBO().Where(ConnectionHandler, news => news.WebId == websiteId);
                var list = new List<News.DataStructure.News>();
                foreach (var newse in enumerable)
                {
                    if (newse.WebSiteNews.Enabled)
                        list.Add(newse.WebSiteNews);
                }
                var outlist = list.OrderByDescending(news => news.PublishDate).ThenByDescending(x => x.PublishTime);
                return top != null && top > 0 ? outlist.Take((int)top) : outlist;
            }
            catch (KnownException knownException)
            {

                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {

                throw new KnownException(ex.Message, ex);
            }
        }

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.NewsConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var obj = new NewsBO().Get(this.ConnectionHandler, keys);
                if (!new NewsBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف اخبار وجود دارد");
                if (!NewsComponent.Instance.NewsTransactionalFacade(this.NewsConnection).Delete(obj.NewsId))
                    throw new Exception("خطایی در حذف اخبار وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.NewsConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.NewsConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.NewsConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
