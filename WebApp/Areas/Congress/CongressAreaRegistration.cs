using System.Web.Mvc;

namespace Radyn.WebApp.Areas.Congress
{
    public class CongressAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Congress";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Congress_default",
                "Congress/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
           
        }
    }
}
