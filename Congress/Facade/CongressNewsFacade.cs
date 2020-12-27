using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;

using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.News;
using Radyn.News.DataStructure;
using Radyn.Utility;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressNewsFacade : CongressBaseFacade<CongressNews>, ICongressNewsFacade
    {
        internal CongressNewsFacade()
        {
        }

        internal CongressNewsFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }



        public bool Insert(Guid congressId, News.DataStructure.News news, NewsContent newsContent, NewsProperty property, HttpPostedFileBase @base)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.NewsConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                news.IsExternal = true;
                if (
                    !NewsComponent.Instance.NewsTransactionalFacade(this.NewsConnection)
                        .Insert(news, newsContent, property, @base))
                    throw new Exception("خطایی درذخیره اخبار وجود دارد");
                var congressNews = new CongressNews { NewsId = news.Id, CongressId = congressId };

               
                if (!new CongressNewsBO().Insert(this.ConnectionHandler, congressNews))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressNews);
                this.ConnectionHandler.CommitTransaction();
                this.NewsConnection.CommitTransaction();
               
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.NewsConnection.RollBack();
               Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.NewsConnection.RollBack();
              Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Update(Guid congressId, News.DataStructure.News news, NewsContent newsContent, NewsProperty property, HttpPostedFileBase @base)
        {
            try
            {
                this.NewsConnection.StartTransaction(IsolationLevel.ReadUncommitted);
               
                news.IsExternal = true;
                if (
                    !NewsComponent.Instance.NewsTransactionalFacade(this.NewsConnection)
                        .Update(news, newsContent, property, @base))
                {
                    throw new Exception("خطایی در ذخیره اخبار وجود دارد");
                }

              
                this.NewsConnection.CommitTransaction();
               
                return true;
            }
            catch (KnownException ex)
            {
                this.NewsConnection.RollBack();
               Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.NewsConnection.RollBack();
               Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

     
        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.NewsConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var obj = new CongressNewsBO().Get(this.ConnectionHandler, keys);
                if (!new CongressNewsBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressNews);
                if (!NewsComponent.Instance.NewsTransactionalFacade(this.NewsConnection).Delete(obj.NewsId))
                    throw new Exception("خطایی در حذف اخبار وجود دارد");

              

                this.ConnectionHandler.CommitTransaction();
                this.NewsConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.NewsConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.NewsConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<News.DataStructure.News> TopCount(Guid congressId, int? top)
        {
            try
            {
                var congressNewsBo = new CongressNewsBO();
                var shamsiDate = DateTime.Now.ShamsiDate() + "-" + DateTime.Now.GetTime();
                var byDescending = congressNewsBo.Select(ConnectionHandler, x => x.News,
                    x => x.CongressId == congressId && x.News.Enabled && x.News.Pined&&(x.News.ExpireDate==null || string.IsNullOrEmpty(x.News.ExpireDate) || ((x.News.ExpireDate+"-"+x.News.ExpireTime).CompareTo(shamsiDate)>0)),
                    new OrderByModel<CongressNews>() { Expression = x => x.News.PublishDate + "" + x.News.PublishTime, OrderType = OrderType.DESC });

                var @select = congressNewsBo.Select(ConnectionHandler, x => x.News,
                   x => x.CongressId == congressId && x.News.Enabled && !x.News.Pined && (x.News.ExpireDate == null||string.IsNullOrEmpty(x.News.ExpireDate) || ((x.News.ExpireDate + "-" + x.News.ExpireTime).CompareTo(shamsiDate) > 0)),
                   new OrderByModel<CongressNews>()
                   { Expression = x => x.News.PublishDate + "" + x.News.PublishTime, OrderType = OrderType.DESC });

                byDescending.AddRange(@select);
                return top != null && top > 0
                     ? byDescending.Take((int)top)
                     : byDescending;



            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        } public async Task<IEnumerable<News.DataStructure.News>> TopCountAsync(Guid congressId, int? top)
        {
            try
            {
                var congressNewsBo = new CongressNewsBO();
                var byDescending =await congressNewsBo.SelectAsync(ConnectionHandler, x => x.News,
                    news => news.CongressId == congressId && news.News.Enabled && news.News.Pined,
                    new OrderByModel<CongressNews>() { Expression = x => x.News.PublishDate + "" + x.News.PublishTime, OrderType = OrderType.DESC });

                var @select =await congressNewsBo.SelectAsync(ConnectionHandler, x => x.News,
                   news => news.CongressId == congressId && news.News.Enabled && !news.News.Pined,
                   new OrderByModel<CongressNews>()
                   { Expression = x => x.News.PublishDate + "" + x.News.PublishTime, OrderType = OrderType.DESC });

                byDescending.AddRange(@select);
                return top != null && top > 0
                     ? byDescending.Take((int)top)
                     : byDescending;



            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }
    }
}
