using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Radyn.Security;
using Radyn.Security.DataStructure;
using Radyn.Web.Mvc.Utility;
using Radyn.WebApp.Areas.Common.Tools;

namespace Radyn.WebApp.AppCode.Security
{
    public class RadynAuthorize : AuthorizeAttribute, IActionFilter, IRadynActionInvoke
    {
        public RadynAuthorize()
        {
            this.DoAuthorize = true;
            this.CheckAccess = true;
            this.ValidRequest = true;
            this.AccessLevel = UserType.User;
        }
        public bool DoAuthorize { get; set; }
        public bool ValidRequest { get; set; }
        public UserType AccessLevel { get; set; }
        public bool CheckAccess { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            return this.Autherized(httpContext, CheckAccess);
        }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.Autherized(filterContext.HttpContext, CheckAccess);

        }

        private bool Autherized(HttpContextBase httpContext, bool checkAccess = true, string url = null)
        {
            if (!DoAuthorize) return true;
            if (!ValidRequest)
            {
                httpContext.Response.Redirect("/Account/AccessDeny");
                return false;
            }
            if (!AppCode.Filter.Autherized.IsAuthenticated())
            {

                httpContext.Response.Redirect(Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Security/User/Login?returnUrl=" + httpContext.Request.RawUrl);
                return false;

            }
            switch (this.AccessLevel)
            {

                case UserType.Host when SessionParameters.UserType != UserType.Host:
                    httpContext.Response.Redirect(Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "~/Account/AccessDeny");
                    return false;
                case UserType.User when SessionParameters.UserType != UserType.User && SessionParameters.UserType != UserType.Host:
                    httpContext.Response.Redirect(Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "~/Account/AccessDeny");
                    return false;

            }

            if (AccessLevel != UserType.User || SessionParameters.User == null || !checkAccess) return true;
            if (string.IsNullOrEmpty(url))
            {
                if (httpContext == null || httpContext.Request == null || httpContext.Request.Url == null)
                    return false;
                var routeData = RouteTable.Routes.GetRouteData(httpContext);
                if (routeData != null)
                {
                    var area = routeData.DataTokens["area"];
                    var controller = routeData.Values["controller"];
                    var action = routeData.Values["action"];
                    if (area != null)
                        url += "/" + area;
                    url += "/" + controller;
                    url += "/" + action;
                    url = url.ToLower();
                }
                else
                    url = httpContext.Request.Url.AbsolutePath.ToLower();
            }

            var hasAccess = SecurityComponent.Instance.UserFacade.AccessMenu(SessionParameters.User.Id, url);
            if (hasAccess == null && url.EndsWith("/index"))
            {
                url = url.Substring(0, url.LastIndexOf("/index"));
                hasAccess = SecurityComponent.Instance.UserFacade.AccessMenu(SessionParameters.User.Id, url);
            }
            if (hasAccess == null && !url.StartsWith("/account"))
            {

                httpContext.Response.Redirect("/Account/AccessDeny");
                return false;

            }

            return true;
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }



        public bool OnRadynActionExecuting(HttpContextBase HttpRequestBase, string url)
        {
            return this.Autherized(HttpRequestBase, CheckAccess, url);
        }

        public async Task<bool> OnRadynActionExecutingAsync(HttpContextBase HttpRequestBase, string url)
        {
            return this.Autherized(HttpRequestBase, CheckAccess, url);
        }
    }
}