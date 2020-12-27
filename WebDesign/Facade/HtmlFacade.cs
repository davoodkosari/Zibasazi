using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Radyn.Common.DataStructure;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Definition;
using Radyn.ContentManager.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Security.DataStructure;
using Radyn.Utility;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Facade.Interface;
using Content = Radyn.WebDesign.DataStructure.Content;

namespace Radyn.WebDesign.Facade
{
    internal sealed class HtmlFacade : WebDesignBaseFacade<Html>, IHtmlFacade
    {
        internal HtmlFacade() { }

        internal HtmlFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }


        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressHtmlBo = new HtmlBO();
                var obj = congressHtmlBo.Get(this.ConnectionHandler, keys);
                if (!congressHtmlBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف Html همایش وجود دارد");
                if (!ContentManagerComponent.Instance.HtmlDesginTransactinalFacade(this.ContentManagerConnection).Delete(obj.HtmlDesginId))
                    throw new Exception("خطایی در حذف Html وجود دارد");
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



        public List<Partials> GetWebDesignContent(Guid WebId, string culture)
        {
            try
            {

                var contents = new ContentBO().Select(this.ConnectionHandler, new Expression<Func<Content, object>>[] { x => x.ContentId, x => x.WebSiteContent.Title, x => x.WebSiteContent.Enabled }, x => x.WebId == WebId);
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

        public bool Insert(Guid websiteId, HtmlDesgin htmlDesgin)
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

                var congressHtml = new Html { HtmlDesginId = htmlDesgin.Id, WebId = websiteId };
                if (!new HtmlBO().Insert(this.ConnectionHandler, congressHtml))
                    throw new Exception("خطایی در ذخیره Html وجود دارد");
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
