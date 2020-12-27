using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.FAQ;
using Radyn.FAQ.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Filter;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.WebDesign.Security.Filter;
using Radyn.WebDesign;

namespace Radyn.WebApp.Areas.WebDesign.Controllers
{
    public class FAQController : WebDesignBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = WebDesignComponent.Instance.FaqFacade.GetByWebSiteId(this.WebSite.Id);
            return View(list);
        }
        [RadynAuthorize]
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var value = collection["txtvalue"];
            var list = WebDesignComponent.Instance.FaqFacade.Search(this.WebSite.Id, value);
            ViewBag.value = value;
            return View(list);
        }
        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [RadynAuthorize]
        [SourceCodeFile("ImageBrowser Controller", "~/Controllers/ImageBrowserController.cs")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var faq = new Radyn.FAQ.DataStructure.FAQ();
            try
            {
                var faqContent = new FAQContent();
                this.RadynTryUpdateModel(faq, collection);
                this.RadynTryUpdateModel(faqContent, collection);
                faqContent.LanguageId = SessionParameters.Culture;
                HttpPostedFileBase image = null;
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }

                if (WebDesignComponent.Instance.FaqFacade.Insert(this.WebSite.Id, faq, faqContent, image))
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
                return View(faq);
            }
        }

        [RadynAuthorize]
        [SourceCodeFile("ImageBrowser Controller", "~/Controllers/ImageBrowserController.cs")]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var faq = FAQComponent.Instance.FAQFacade.Get(Id);
            try
            {
                var content = FAQComponent.Instance.FaqContentFacade.Get(Id, collection["LanguageId"]) ??
                new FAQContent();
                this.RadynTryUpdateModel(content, collection);
                this.RadynTryUpdateModel(faq, collection);
                HttpPostedFileBase image = null;
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                if (FAQComponent.Instance.FAQFacade.Update(faq, content, image))
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
                return View();
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            try
            {
                if (WebDesignComponent.Instance.FaqFacade.Delete(this.WebSite.Id, Id))
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
        [RadynAuthorize(DoAuthorize = false)]
        public ActionResult FAQSearchableList(string Date)
        {
            return PartialView("FAQSearchableList", WebDesignComponent.Instance.FaqFacade.GetByWebSiteId(this.WebSite.Id).ToList());
        }
        [RadynAuthorize(DoAuthorize = false)]
        public ActionResult FAQList()
        {
            return View();
        }
    }
}