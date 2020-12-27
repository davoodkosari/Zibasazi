using System.Web.Mvc;

namespace Radyn.WebApp.Areas.SQL
{
    public class SQLAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SQL";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SQL_default",
                "SQL/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
