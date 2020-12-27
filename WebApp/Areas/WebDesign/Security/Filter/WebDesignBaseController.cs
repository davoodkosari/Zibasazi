using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.WebDesign.Tools;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Definition;

namespace Radyn.WebApp.Areas.WebDesign.Security.Filter
{
    [WebDesignHost]
    public class WebDesignBaseController : LocalizedController
    {

        public WebSite WebSite
        {
            get
            {
                if (System.Web.HttpContext.Current.Session == null) return null;
                AppExtention.GetWebSite();
                return SessionParameters.CurrentWebSite;

            }


        }





    }
    [WebDesignHost]
    public class WebDesignBaseController<T> : LocalizedController<T> where T : class
    {

        public WebSite WebSite
        {
            get
            {
                if (System.Web.HttpContext.Current.Session == null) return null;
                AppExtention.GetWebSite();
                return SessionParameters.CurrentWebSite;

            }


        }





    }
}