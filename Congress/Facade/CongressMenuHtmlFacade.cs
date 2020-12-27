using System;
using System.Data;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressMenuHtmlFacade : CongressBaseFacade<CongressMenuHtml>, ICongressMenuHtmlFacade
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
                var CongressMenuHtmlBO = new CongressMenuHtmlBO();
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




      

        

        public bool Insert(Guid congressId, MenuHtml htmlDesgin)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                htmlDesgin.IsExternal = true;
                var htmlDesginTransactinalFacade =
                    ContentManagerComponent.Instance.MenuHtmlTransactinalFacade(this.ContentManagerConnection);
                if (htmlDesgin.Enabled)
                {
                    var list = new CongressMenuHtmlBO().Where(this.ConnectionHandler,
                        html => html.CongressId == congressId);
                   
                    foreach (var html in list)
                    {

                        html.MenuHtml.Enabled = false;
                        if (!htmlDesginTransactinalFacade.Update(html.MenuHtml))
                            throw new Exception("خطایی در ویرایش Html وجود دارد");
                    }
                }
                htmlDesgin.CurrentUICultureName = htmlDesgin.CurrentUICultureName;
                if (!htmlDesginTransactinalFacade.Insert(htmlDesgin))
                    throw new Exception("خطایی در ذخیره Html وجود دارد");
                var CongressMenuHtml = new CongressMenuHtml { MenuHtmlId = htmlDesgin.Id, CongressId = congressId };
                if (!new CongressMenuHtmlBO().Insert(this.ConnectionHandler, CongressMenuHtml))
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

        public bool Update(Guid congressId, MenuHtml htmlDesgin)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var htmlDesginTransactinalFacade =
                    ContentManagerComponent.Instance.MenuHtmlTransactinalFacade(this.ContentManagerConnection);
                if (htmlDesgin.Enabled)
                {
                    var list = new CongressMenuHtmlBO().Where(this.ConnectionHandler,
                        html =>  html.CongressId == congressId);
                    foreach (var html in list)
                    {

                        html.MenuHtml.Enabled = false;
                        if (!htmlDesginTransactinalFacade.Update(html.MenuHtml))
                            throw new Exception("خطایی در ویرایش Html وجود دارد");
                    }
                }
                htmlDesgin.CurrentUICultureName = htmlDesgin.CurrentUICultureName;
                if (!htmlDesginTransactinalFacade.Update(htmlDesgin))
                    throw new Exception("خطایی در ویرایش Html وجود دارد");
                var CongressMenuHtml = new CongressMenuHtml { MenuHtmlId = htmlDesgin.Id, CongressId = congressId };
                if (!new CongressMenuHtmlBO().Update(this.ConnectionHandler, CongressMenuHtml))
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
