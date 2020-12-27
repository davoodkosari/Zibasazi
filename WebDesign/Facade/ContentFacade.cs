using System;
using System.Collections.Generic;
using System.Data;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.Facade.Interface;
using Content = Radyn.WebDesign.DataStructure.Content;

namespace Radyn.WebDesign.Facade
{
    internal sealed class ContentFacade : WebDesignBaseFacade<Content>, IContentFacade
    {
        internal ContentFacade() { }

        internal ContentFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }


        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressContentBo = new ContentBO();
                var obj = congressContentBo.Get(this.ConnectionHandler, keys);
                if (!congressContentBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف محتوا وجود دارد");
                if (!ContentManagerComponent.Instance.ContentTransactionalFacade(this.ContentManagerConnection).Delete(obj.ContentId))
                    throw new Exception("خطایی در حذف محتوا وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
        public IEnumerable<ContentManager.DataStructure.Content> GetByWebSiteId(Guid webSiteId, bool onlyenabled)
        {
            try
            {
                
               return new ContentBO().Select(this.ConnectionHandler,x=>x.WebSiteContent,x=>x.WebId == webSiteId&&x.WebSiteContent.Enabled);
              
              
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

        public bool Insert(Guid websiteId, ContentManager.DataStructure.Content content, ContentContent contentcontent)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                content.IsExternal = true;
                if (!ContentManagerComponent.Instance.ContentTransactionalFacade(this.ContentManagerConnection).Insert(content, contentcontent))
                    throw new Exception("خطایی در ذخیره محتوا وجود دارد");
                var congressContent = new Content { ContentId = content.Id, WebId = websiteId };
                if (!new ContentBO().Insert(this.ConnectionHandler, congressContent))
                    throw new Exception("خطایی در ذخیره محتوا وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
