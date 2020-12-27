using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;

namespace Radyn.WebApp.Controllers
{
    public class AccountController : LocalizedController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated || SessionParameters.UserOperation == null)
                return Redirect("/security/User/Login");
            if (SessionParameters.UserOperation.Count() == 1)
                return Redirect("/Security/User/Menu?oid=" + SessionParameters.UserOperation.First().Id);
            return View(SessionParameters.UserOperation);
        }

        public ActionResult AccessDeny()
        {

            

            return View();
        }
    }
}
