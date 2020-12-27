using System;
using System.Collections.Generic;
using System.Data;
using Radyn.Common;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Facade.Interface;

namespace Radyn.WebDesign.Facade
{
    internal sealed class WebSiteFacade : WebDesignBaseFacade<WebSite>, IWebSiteFacade
    {
        internal WebSiteFacade() { }

        internal WebSiteFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
        public bool Insert(WebSite homa, List<WebSiteAlias> list)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);

                if (!new WebSiteBO().Insert(this.ConnectionHandler, homa, list))
                    throw new Exception("خطا در ثبت اطلاعات");

                this.ConnectionHandler.CommitTransaction();

                return true;
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();

                throw new KnownException(ex.Message, ex);
            }
        }
        public override bool Delete(params object[] keys)
        {
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);




                if (!new WebSiteBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطا در حذف اطلاعات");
                this.ConnectionHandler.CommitTransaction();



                return true;
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();


                throw new KnownException(ex.Message, ex);
            }

        }
        public bool Update(WebSite webSite, List<WebSiteAlias> list)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);

                if (!new WebSiteBO().Update(this.ConnectionHandler, webSite, list))
                    throw new Exception("خطا در ثبت اطلاعات");
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();


                throw new KnownException(ex.Message, ex);
            }
        }
        public WebSite GetWebSiteByUrl(string authority)
        {
            try
            {
                return new WebSiteBO().GetWebSiteByUrl(this.ConnectionHandler, authority);
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

       
    }
}
