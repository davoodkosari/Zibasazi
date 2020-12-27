using System;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.WebDesign.Security.Filter;
using Radyn.WebApp.Areas.WebDesign.Tools;
using Radyn.WebDesign;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Definition;


namespace Radyn.WebApp.Areas.WebDesign.Controllers
{
    public class MailController : LocalizedController
    {
        public ActionResult SendMail(string mail, string subject, string body)
        {

            
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

    }
}