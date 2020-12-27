using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.FAQ;
using Radyn.FAQ.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Filter;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressFAQController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.CongressFaqFacade.Select(x=>x.FAQ,x=>x.CongressId==this.Homa.Id);
          return View(list);
        }
        [RadynAuthorize]
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var value = collection["txtvalue"];
            var list = CongressComponent.Instance.BaseInfoComponents.CongressFaqFacade.Search(this.Homa.Id, value);
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
                HttpPostedFileBase image = null;
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }

                if (CongressComponent.Instance.BaseInfoComponents.CongressFaqFacade.Insert(this.Homa.Id, faq, faqContent, image))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = faq.Id });

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressFAQ/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
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
                    return this.SubmitRedirect(collection, new { Id = Id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressFAQ/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
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
                if (CongressComponent.Instance.BaseInfoComponents.CongressFaqFacade.Delete(this.Homa.Id, Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressFAQ/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressFAQ/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }
        [RadynAuthorize(DoAuthorize = false)]
        public ActionResult FAQSearchableList(string Date)
        {
            return PartialView("FAQSearchableList", CongressComponent.Instance.BaseInfoComponents.CongressFaqFacade.Select(x=>x.FAQ,x=>x.CongressId==this.Homa.Id));
        }
        [RadynAuthorize(DoAuthorize = false)]
        public ActionResult FAQList()
        {
            return View();
        }
    }
}