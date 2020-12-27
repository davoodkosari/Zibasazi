using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.ContentManager;

using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Definition;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressHtmlFacade : CongressBaseFacade<CongressHtml>, ICongressHtmlFacade
    {
        internal CongressHtmlFacade()
        {
        }

        internal CongressHtmlFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public List<Partials> GetWebDesignContent(Guid homaId, string culture)
        {
            try
            {

                var contents = new CongressContentBO().Select(this.ConnectionHandler, new Expression<Func<CongressContent, object>>[] { x => x.ContentId, x => x.Content.Title, x => x.Content.Enabled }, x => x.CongressId == homaId);
                var @select = contents.Select(i => (int)i.ContentId);
                return ContentManagerComponent.Instance.PartialsFacade.GetContentPartials(@select, culture);



            }

            catch (KnownException ex)
            {

                throw new KnownException(ex.Message, ex);
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
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressHtmlBO = new CongressHtmlBO();
                var obj = congressHtmlBO.Get(this.ConnectionHandler, keys);
                if (!congressHtmlBO.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف Html همایش وجود دارد");
                if (
                    !ContentManagerComponent.Instance.HtmlDesginTransactinalFacade(this.ContentManagerConnection)
                        .Delete(obj.HtmlDesginId))
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




      

       

        public bool Insert(Guid congressId, HtmlDesgin htmlDesgin)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                htmlDesgin.IsExternal = true;
                var htmlDesginTransactinalFacade =
                    ContentManagerComponent.Instance.HtmlDesginTransactinalFacade(this.ContentManagerConnection);
               htmlDesgin.CurrentUICultureName = htmlDesgin.CurrentUICultureName;
                if (!htmlDesginTransactinalFacade.Insert(htmlDesgin))
                    throw new Exception("خطایی در ذخیره Html وجود دارد");
                var congressHtml = new CongressHtml { HtmlDesginId = htmlDesgin.Id, CongressId = congressId };
                if (!new CongressHtmlBO().Insert(this.ConnectionHandler, congressHtml))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressHtml);
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

        public bool Update(Guid congressId, HtmlDesgin htmlDesgin)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var htmlDesginTransactinalFacade =
                    ContentManagerComponent.Instance.HtmlDesginTransactinalFacade(this.ContentManagerConnection);
                htmlDesgin.CurrentUICultureName = htmlDesgin.CurrentUICultureName;
                if (!htmlDesginTransactinalFacade.Update(htmlDesgin))
                    throw new Exception("خطایی در ویرایش Html وجود دارد");
                var congressHtml = new CongressHtml { HtmlDesginId = htmlDesgin.Id, CongressId = congressId };
                if (!new CongressHtmlBO().Update(this.ConnectionHandler, congressHtml))
                    throw new Exception(Resources.Congress.ErrorInEditCongressHtml);
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
