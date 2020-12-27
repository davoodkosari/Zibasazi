using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Radyn.Common;
using Radyn.Congress;
using Radyn.WebApp.AppCode.Security;

namespace Radyn.WebApp.AppCode.Filter
{
    public class Localization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (string.IsNullOrEmpty(SessionParameters.Culture))
            {
              var defaultLang =CommonComponent.Instance.LanguageFacade.Select(x=>x.Id,c => c.IsDefault);
                if (defaultLang != null && defaultLang.Count > 0 && !string.IsNullOrEmpty(defaultLang[0]))
                    SessionParameters.Culture = defaultLang[0];
                else
                    SessionParameters.Culture = "fa-IR";
            }
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SessionParameters.Culture);
            base.OnActionExecuting(filterContext);
        }
    }
}