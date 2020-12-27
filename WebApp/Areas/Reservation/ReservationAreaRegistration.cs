using System.Web.Mvc;

namespace Radyn.WebApp.Areas.Reservation
{
    public class ReservationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reservation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Reservation_default",
                "Reservation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Reservation_default1",
                "Reservation/{controller}/{action}/{id}/{*catchall}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
