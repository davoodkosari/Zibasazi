using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Radyn.ContentManager.BO;
using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Facade.Interface;
using Radyn.ContentManager.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;

namespace Radyn.ContentManager.Facade
{
    internal sealed class ContentFacade : ContentManagerBaseFacade<Content>, IContentFacade
    {
        internal ContentFacade()
        {
        }

        internal ContentFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      
     

        public IEnumerable<Content> Search(string qry)
        {
            try
            {
                var list = new ContentBO().Search(this.ConnectionHandler, qry);
                return list;
            }
            catch (KnownException ex)
            {
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public override bool Insert(Content obj)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new ContentBO().Insert(this.ConnectionHandler, obj))
                    throw new Exception("خطایی در ذخیره محتوا وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var obj = new ContentBO().Get(this.ConnectionHandler, keys);
                var contentContents = new ContentContentBO().Where(ConnectionHandler,
                    contentContent => contentContent.Id == obj.Id);
                if (contentContents.Count > 0)
                {
                    foreach (var contentContent in contentContents)
                    {
                        if (
                            !new ContentContentBO().Delete(this.ConnectionHandler, contentContent.Id,
                                contentContent.LanguageId))
                            throw new Exception("خطایی در حذف محتوای محتوا وجود دارد");
                    }
                }
                if (!new ContentBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف  محتوا وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }



        public bool Update(Content content, ContentContent contentContent)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (contentContent != null)
                {
                    if (contentContent.Id == 0)
                    {
                        contentContent.Id = content.Id;
                        if (!new ContentContentBO().Insert(this.ConnectionHandler, contentContent))
                            throw new Exception("خطایی در ذخیره محتوا وجود دارد");

                    }
                    else if (!new ContentContentBO().Update(this.ConnectionHandler, contentContent))
                        throw new Exception("خطایی در ویرایش محتوا وجود دارد");
                }
                if (!new ContentBO().Update(this.ConnectionHandler, content))
                    throw new Exception("خطایی در ویرایش محتوا وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public bool Insert(Content content, ContentContent contentContent)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new ContentBO().Insert(this.ConnectionHandler, content))
                    throw new Exception("خطایی در ذخیره محتوا وجود دارد");
                if (contentContent != null)
                {
                    contentContent.Id = content.Id;
                    if (!new ContentContentBO().Insert(this.ConnectionHandler, contentContent))
                        throw new Exception("خطایی در ذخیره محتوا وجود دارد");
                }
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }



        public string GetHtml(int Id, string culture)
        {
            try
            {
                var content = new ContentBO().Get(this.ConnectionHandler, Id);
                if (content == null) return string.Empty;
                 return new ContentBO().GetHtml(this.ConnectionHandler,content,culture);
            }
            catch (KnownException ex)
            {

                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
   public async Task<string> GetHtmlAsync(int Id, string culture)
        {
            try
            {
                var content =await new ContentBO().GetAsync(this.ConnectionHandler, Id);
                if (content == null) return string.Empty;
                 return await new ContentBO().GetHtmlAsync(this.ConnectionHandler,content,culture);
            }
            catch (KnownException ex)
            {

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

