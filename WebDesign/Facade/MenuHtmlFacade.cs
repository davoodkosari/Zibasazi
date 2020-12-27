using System;
using System.Data;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.BO;

namespace Radyn.WebDesign.Facade
{
    internal sealed class CongressMenuHtmlFacade : WebDesignBaseFacade<WebDesign.DataStructure.MenuHtml>, WebDesign.Facade.Interface.IMenuHtmlFacade
    {
        internal CongressMenuHtmlFacade()
        {
        }

        internal CongressMenuHtmlFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

    

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var CongressMenuHtmlBO = new MenuHtmlBO();
                var obj = CongressMenuHtmlBO.Get(this.ConnectionHandler, keys);
                if (!CongressMenuHtmlBO.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف Html همایش وجود دارد");
                if (
                    !ContentManagerComponent.Instance.MenuHtmlTransactinalFacade(this.ContentManagerConnection)
                        .Delete(obj.MenuHtmlId))
                    throw new Exception("خطایی در حذف Html وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

     

        public bool Insert(Guid WebSiteId, MenuHtml htmlDesgin)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                htmlDesgin.IsExternal = true;
                var htmlDesginTransactinalFacade =ContentManagerComponent.Instance.MenuHtmlTransactinalFacade(this.ContentManagerConnection);
               htmlDesgin.CurrentUICultureName = htmlDesgin.CurrentUICultureName;
                if (!htmlDesginTransactinalFacade.Insert(htmlDesgin))
                    throw new Exception("خطایی در ذخیره Html وجود دارد");
                var CongressMenuHtml = new WebDesign.DataStructure.MenuHtml { MenuHtmlId = htmlDesgin.Id, WebSiteId = WebSiteId };
                if (!new MenuHtmlBO().Insert(this.ConnectionHandler, CongressMenuHtml))
                    throw new Exception("خطایی در ذخیره Html وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

      
    }
}
