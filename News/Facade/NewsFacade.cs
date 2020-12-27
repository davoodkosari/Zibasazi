using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Radyn.EnterpriseNode;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.News.BO;
using Radyn.News.DataStructure;
using Radyn.News.Facade.Interface;
using Radyn.Security;

namespace Radyn.News.Facade
{
    internal sealed class NewsFacade : NewsBaseFacade<News.DataStructure.News>, INewsFacade
    {
        internal NewsFacade()
        {
        }

        internal NewsFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      

        public bool Insert(DataStructure.News news, NewsContent content, NewsProperty property,
            HttpPostedFileBase fileBase)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (fileBase != null)
                    news.ThumbnailId =
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Insert(fileBase);
                if (!new NewsBO().Insert(this.ConnectionHandler, news))
                    throw new Exception("خطایی در ذخیره اخبار وجود دارد");
                if (content != null)
                {
                    content.Id = news.Id;
                    if (!new NewsContentBO().Insert(this.ConnectionHandler, content))
                        throw new Exception("خطایی در ذخیره محتوای اخبار وجود دارد");
                }
                if (property != null)
                {
                    property.Id = news.Id;
                    if (!new NewsPropertyBO().Insert(this.ConnectionHandler, property))
                        throw new Exception("خطایی در ذخیره تنظیمات اخبار وجود دارد");
                }
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var newsBO = new NewsBO();
                var obj = newsBO.Get(this.ConnectionHandler, keys);
                var newsContentBO = new NewsContentBO();
                var content = newsContentBO.Where(this.ConnectionHandler,
                    newsContent => newsContent.Id == obj.Id);
                foreach (var newsContent in content)
                {
                    if (!newsContentBO.Delete(this.ConnectionHandler, newsContent.Id, newsContent.LanguageId))
                        throw new Exception("خطایی در حذف محتوای اخبار وجود دارد");
                }
                var newsPropertyBO = new NewsPropertyBO();
                var firstOrDefault =
                    newsPropertyBO.FirstOrDefault(this.ConnectionHandler, property => property.Id == obj.Id);
                if (firstOrDefault != null)
                {
                    if (!newsPropertyBO.Delete(this.ConnectionHandler, firstOrDefault.Id))
                        throw new Exception("خطایی در حذف تنظیمات اخبار وجود دارد");
                }
                if (!newsBO.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف اخبار وجود دارد");
                if (obj.ThumbnailId != null)
                    FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                        .Delete(obj.ThumbnailId);
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Update(DataStructure.News obj, NewsContent content, NewsProperty property,
            HttpPostedFileBase fileBase)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (fileBase != null)
                {
                    if (obj.ThumbnailId.HasValue && property.HasAttachment)
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Update(fileBase, obj.ThumbnailId.Value);
                    else
                        obj.ThumbnailId =
                            FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                                .Insert(fileBase);
                }
                if (property != null)
                {
                    if (!new NewsPropertyBO().Update(this.ConnectionHandler, property))
                        throw new Exception("خطایی در ویرایش تنظیمات خبر وجود دارد");
                }
              
                if (content != null)
                {
                    if (content.Id == 0)
                    {
                        content.Id = obj.Id;
                        if (!new NewsContentBO().Insert(this.ConnectionHandler, content))
                            throw new Exception("خطایی در ذخیره محتوای اخبار وجود دارد");
                    }
                    else
                    {
                        if (!new NewsContentBO().Update(this.ConnectionHandler, content))
                            throw new Exception("خطایی در ویرایش محتوای اخبار وجود دارد");
                    }
                }
                if (!new NewsBO().Update(this.ConnectionHandler, obj))
                    throw new Exception("خطایی در ویرایش اخبار وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<DataStructure.News> GetByCategory(Guid categoryId, int? topCount)
        {
            try
            {
                var newses = new NewsBO().GetByCategory(this.ConnectionHandler, categoryId, topCount);
               return newses;
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

        public IEnumerable<DataStructure.News> GetTopNews(int topCount = 0, bool isSelected = true)
        {
            try
            {
                var newses = new NewsBO().GetTopNews(this.ConnectionHandler, topCount, isSelected);
                return newses;
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
public async Task<IEnumerable<DataStructure.News>> GetTopNewsAsync(int topCount = 0, bool isSelected = true)
        {
            try
            {
                var newses =await new NewsBO().GetTopNewsAsync(this.ConnectionHandler, topCount, isSelected);
                return newses;
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

        public IEnumerable<DataStructure.News> Search(string text)
        {
            try
            {
                var newses = new NewsBO().Search(this.ConnectionHandler, text);
                return newses;
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
