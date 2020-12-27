using System;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using Radyn.Congress;
using Radyn.Web.Mvc;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.Statistics.Components;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;
using Radyn.Congress.Facade.Interface;
using Radyn.Statistics;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.App_Start;

namespace Radyn.WebApp
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters 
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Error(object sender, EventArgs e)
        {
           
        }


        protected void Application_Start()
        {
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            Application["reported"] = false;
            RadynMvcControlsRouteRegistrar.Register();
            FileManager.FileManagerComponent.RegisterFileManagerRoute();
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            AppCode.Themes.ThemeRegisterer.Register();
            AppCode.Themes.HomaThemesRegister.Register();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AddStimulReportFonts();
            var thread = new Thread(ProcessOnStartUp);
            thread.Start();


        }

        private  void AddStimulReportFonts()
        {
            try
            {
                Stimulsoft.Base.StiFontCollection.AddFontFile(Server.MapPath("~/AppCode/Fonts/BNazanin/BNazanin.ttf"));
                Stimulsoft.Base.StiFontCollection.AddFontFile(Server.MapPath("~/AppCode/Fonts/dastnevis/dastnevis.ttf"));
                Stimulsoft.Base.StiFontCollection.AddFontFile(Server.MapPath("~/AppCode/Fonts/Titr/Titr.ttf"));
                Stimulsoft.Base.StiFontCollection.AddFontFile(Server.MapPath("~/AppCode/Fonts/Vazir/ttf/Vazir-FD.ttf"));
                Stimulsoft.Base.StiFontCollection.AddFontFile(Server.MapPath("~/AppCode/Fonts/Vazir/ttf/Vazir-Medium-FD.ttf"));
                Stimulsoft.Base.StiFontCollection.AddFontFile(Server.MapPath("~/AppCode/Fonts/Vazir/ttf/Vazir-Bold-FD.ttf"));
                Stimulsoft.Base.StiFontCollection.AddFontFile(Server.MapPath("~/AppCode/Fonts/Yekan/Yekan.ttf"));
                Stimulsoft.Base.StiFontCollection.AddFontFile(Server.MapPath("~/AppCode/Fonts/Zar/zar.ttf"));
                Stimulsoft.Base.StiFontCollection.AddFontFile(Server.MapPath("~/AppCode/Fonts/Zar/Far_Zar.ttf"));
            }
            catch 
            {

               
            }
        }
        void Application_PreSendRequestHeaders(Object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Headers.Add("X-Frame-Options", "DENY");
            Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Remove("Server");

        }
        public static void ProcessOnStartUp()
        {
            CongressComponent.Instance.Initialize();

        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
          


        }
        protected void Session_Start()
        {
            Web.HttpContext.Current.Session["SesstionId"] = HttpContext.Current.Session.SessionID;
            LogCompoenents.AddLog(HttpContext.Current.Request, (string)Web.HttpContext.Current.Session["SesstionId"]);

        }

        protected void Session_End()
        {
            if (System.Web.HttpContext.Current != null)
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                string[] allDomainCookes = Request.Cookies.AllKeys;
                foreach (string domainCookie in allDomainCookes)
                {
                    if (Request.Cookies[domainCookie] == null) continue;
                    Request.Cookies[domainCookie].Expires = DateTime.Now.AddDays(-1d);
                }
                

            }

        }

    }
}
