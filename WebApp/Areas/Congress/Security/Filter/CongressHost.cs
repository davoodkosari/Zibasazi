using System.Web.Mvc;
using Radyn.WebApp.Areas.Congress.Tools;

namespace Radyn.WebApp.Areas.Congress.Security.Filter
{
    public class CongressHost : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {


            try
            {
                AppExtention.GetHoma(filterContext.HttpContext);
            }
            catch 
            {

                
            }




        }


        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }

}