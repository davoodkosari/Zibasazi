using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebDesign;
using Radyn.WebDesign.Definition;

namespace Radyn.WebApp.Areas.WebDesign.Tools
{
    public static class AppExtention
    {
        public static KeyValuePair<string, string>? WebSitError()
        {

            if (SessionParameters.CurrentWebSite == null || SessionParameters.CurrentWebSite.Status == Enums.WebSiteStatus.NotRegistered)
                return new KeyValuePair<string, string>(Resources.WebDesign.NotFound, "/Content/Images/conference-alert/not-found.png");
            var status = SessionParameters.CurrentWebSite.Status;
            switch (status)
            {
                case Enums.WebSiteStatus.NotConfiged:

                    return new KeyValuePair<string, string>(Resources.WebDesign.NotConfig, "/Content/Images/conference-alert/setting.png");
                    break;

                case Enums.WebSiteStatus.Disabled:
                    return new KeyValuePair<string, string>(Resources.WebDesign.Disabled, "/Content/Images/conference-alert/disable.png");
                    break;

                case Enums.WebSiteStatus.NotRegistered:
                    return new KeyValuePair<string, string>(Resources.WebDesign.NotRegistered, "/Content/Images/conference-alert/not-found.png");
                    break;

            }
            return null;
        }


        public static List<KeyValuePair<string, string>> SlideList()
        {
            var liste = new List<KeyValuePair<string, string>>
            {
                 new KeyValuePair<string, string>(
                    "1",Resources.WebDesign.BigSlideId),
                new KeyValuePair<string, string>(
                    "0",Resources.WebDesign.AverageSlideId),
                new KeyValuePair<string, string>(
                    "-1",Resources.WebDesign.MiniSlideId)
            };

            return liste;
        }
        public static void GetWebSite()
        {
          var webSiteFacade = WebDesignComponent.Instance.WebSiteFacade;
            var authority = HttpContext.Current.Request.Url.Authority;
            if (authority.ToLower().StartsWith("www."))
                authority = authority.Replace("www.", "");
            if (HttpContext.Current.Session == null) return;
            if (SessionParameters.CurrentWebSite == null || SessionParameters.CurrentWebSite.Status != Enums.WebSiteStatus.NoProblem)
                SessionParameters.CurrentWebSite = webSiteFacade.GetWebSiteByUrl(authority);
            SessionParameters.Error = WebSitError();
            if (SessionParameters.UserOperation != null || SessionParameters.WebSiteSessionStarted || SessionParameters.CurrentWebSite.Status != Enums.WebSiteStatus.NoProblem) return;
            var list = WebDesignComponent.Instance.LanguageFacade.GetValidList(SessionParameters.CurrentWebSite.Id);
            if (list != null && list.Any())
            {
                if (list.Count() == 1)
                    SessionParameters.Culture = list.First().Id;
                else
                    SessionParameters.Culture = "fa-IR";
            }
            SessionParameters.WebSiteSessionStarted = true;
        }


    }
}