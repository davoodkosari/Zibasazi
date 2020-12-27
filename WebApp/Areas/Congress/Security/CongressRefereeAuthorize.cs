using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Radyn.Web.Mvc.Utility;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;

namespace Radyn.WebApp.Areas.Congress.Security
{
    public class CongressRefereeAuthorize : AuthorizeAttribute, IRadynActionInvoke
    {
       



        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            filterContext.RequestContext.HttpContext.Response.Redirect(Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/RefereePanel/Login?returnUrl=" + filterContext.RequestContext.HttpContext.Request.RawUrl);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return SessionParameters.CongressReferee != null;
        }
         private bool Autherized(HttpContextBase httpContext, bool checkAccess = true, string url = null)
        {
            if (SessionParameters.CongressReferee != null) return true;
            httpContext.Response.Redirect(Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/RefereePanel/Login");
            return false;
        }


      
        public async Task<bool> OnRadynActionExecutingAsync(HttpContextBase HttpRequestBase, string url)
        {
            return this.Autherized(HttpRequestBase, false, url);
        }
        public bool OnRadynActionExecuting(HttpContextBase HttpRequestBase, string url)
        {
            return this.Autherized(HttpRequestBase, false, url);
        }
    }
}