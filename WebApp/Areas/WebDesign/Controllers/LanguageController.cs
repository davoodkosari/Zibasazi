using System;
using System.Web;
using System.Web.Mvc;
using Radyn.Common;
using Radyn.Common.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.WebDesign.Security.Filter;
using Radyn.WebDesign;

namespace Radyn.WebApp.Areas.WebDesign.Controllers
{
    public class LanguageController : WebDesignBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = WebDesignComponent.Instance.LanguageFacade.GetByWebSiteId(this.WebSite.Id);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(string Id)
        {
            ViewBag.Id = Id;
            return View();

        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Create(FormCollection collection)
        {
            var language = new Language();

            try
            {
                this.RadynTryUpdateModel(language, collection);

                HttpPostedFileBase image = null;
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }

                if (WebDesignComponent.Instance.LanguageFacade.Insert(this.WebSite.Id, language, image))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInInsert + exception.Message, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return View(language);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Edit(string Id, FormCollection collection)
        {
            var language = CommonComponent.Instance.LanguageFacade.Get(Id);

            try
            {

                this.RadynTryUpdateModel(language, collection);
                HttpPostedFileBase image = null;
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }

                if (CommonComponent.Instance.LanguageFacade.Update(language, image))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInEdit + exception.Message, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                ViewBag.Id = Id;
                return View(language);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Delete(string Id, FormCollection collection)
        {
            try
            {
                if (WebDesignComponent.Instance.LanguageFacade.Delete(this.WebSite.Id, Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInDelete + exception.Message, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                ViewBag.Id = Id;
                return View();
            }
        }



        public ActionResult CultureBar()
        {
            var list = WebDesignComponent.Instance.LanguageFacade.GetValidList(this.WebSite.Id);
            return PartialView("LanguageBar", list);
        }


    }
}
