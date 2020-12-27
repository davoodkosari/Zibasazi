using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Radyn.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.Congress.Tools;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.FormGenerator.Tools;
using Radyn.WebApp.Areas.WebDesign.Security.Filter;
using Radyn.WebApp.Tools;

namespace Radyn.WebApp.Controllers
{
    public class HomeController : LocalizedController
    {
        [WebDesignHost]
        [CongressHost]

        public ActionResult Index(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                SessionParameters.Culture = culture;
                return RedirectToAction("Index");
            }
            if (SessionParameters.CurrentWebSite != null && SessionParameters.CurrentWebSite.Status == Radyn.WebDesign.Definition.Enums.WebSiteStatus.NoProblem)
                return this.Redirect("/Home/Portal");
            return View(SessionParameters.CurrentCongress);
        }


        [WebDesignHost]
        public ActionResult Portal(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                SessionParameters.Culture = culture;
                return RedirectToAction("Portal");
            }
            return View(SessionParameters.CurrentWebSite);
        }


        public ActionResult TestService()
        {
            try
            {
                PackageRepository.Work();
                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(FormCollection collection)
        {
            try
            {
                var bodys = this.GetDetailForm(collection);
                string emailAdress = SessionParameters.CurrentCongress != null ? SessionParameters.CurrentCongress.Configuration.MailFrom : null;
                if (!string.IsNullOrWhiteSpace(emailAdress))
                {
                    if (MessageComponenet.Instance.MailFacade.SendMail(emailAdress, Resources.CommonComponent.ContactUs, bodys))
                        ShowMessage(Resources.CommonComponent.SentAdminMessage);
                }
                else
                {
                    if (MessageComponenet.Instance.MailFacade.SendMail("info@" + Request.Url.Authority, Resources.CommonComponent.ContactUs, bodys))
                        ShowMessage(Resources.CommonComponent.SentAdminMessage);
                }
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
            }
            return View();
        }
        public ActionResult Error()
        {


            return View();
        }
       
       
    }
}
