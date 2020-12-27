using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Radyn.Web.Mvc.Utility;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;

namespace Radyn.WebApp.Areas.Congress.Security
{
    public class CongressUserAuthorize : AuthorizeAttribute, IRadynActionInvoke
    {
       
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.RequestContext.HttpContext.Response.Redirect(Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/UserPanel/Login?returnUrl=" + filterContext.RequestContext.HttpContext.Request.RawUrl);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (SessionParameters.CongressUser == null)
                return false;
            //چک کردن اینکه کاربر مورد نظر حتماً در همایش خودش باشد
            return SessionParameters.CongressUser.CongressId == SessionParameters.CurrentCongress.Id;
        }

        private bool Autherized(HttpContextBase httpContext, bool checkAccess = true, string url = null)
        {
            if (SessionParameters.CongressUser != null) return true;
            httpContext.Response.Redirect(Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/UserPanel/Login");
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