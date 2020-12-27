
using System.Collections.Generic;
using System.Web;

namespace Radyn.WebApp.AppCode.Security
{
    public partial class SessionParameters
    {

        public static WebDesign.DataStructure.WebSite CurrentWebSite
        {
            get
            {
                if (Web.HttpContext.Current != null && Web.HttpContext.Current.Session["CurrentWebSite"] != null)
                    return (WebDesign.DataStructure.WebSite)Web.HttpContext.Current.Session["CurrentWebSite"];
                return null;
            }
            set
            {

                Web.HttpContext.Current.Session["CurrentWebSite"] = value;
            }
        }
      
        public static bool WebSiteSessionStarted
        {
            get
            {
                if (Web.HttpContext.Current != null && Web.HttpContext.Current.Session["WebSiteSessionStarted"] != null)
                    return (bool)Web.HttpContext.Current.Session["WebSiteSessionStarted"];
                return false;
            }
            set
            {

                Web.HttpContext.Current.Session["WebSiteSessionStarted"] = value;
            }
        }
    }
}