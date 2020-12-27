using System;
using System.Web;
using System.Web.Mvc;
using Radyn.Common.Component;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class SupporterController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.SupporterFacade.Where(
                    x => x.CongressId == this.Homa.Id);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult GetDetail(Guid Id)
        {
            var articlePaymentType = CongressComponent.Instance.BaseInfoComponents.SupporterFacade.Get(Id);
            if (articlePaymentType == null) return Content("false");
            return PartialView("PVDetails", articlePaymentType);
        }
        public ActionResult GetModify(Guid? Id, string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            ViewBag.SupportType =
               new SelectList(CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.SelectKeyValuePair(x=>x.Id,x=>x.Title,x => x.CongressId == this.Homa.Id), "Key", "Value");
            if (!Id.HasValue) return PartialView("PVModify", new Supporter());
            var languageContent = CongressComponent.Instance.BaseInfoComponents.SupporterFacade.GetLanuageContent(culture,Id);
            return PartialView("PVModify", languageContent);
        }
        [RadynAuthorize]
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var supporter = new Supporter();
            try
            {
                this.RadynTryUpdateModel(supporter);
                supporter.CongressId = this.Homa.Id;
                HttpPostedFileBase file = null;
                if (Session["ImageSupporter"] != null)
                {
                    file = (HttpPostedFileBase)Session["ImageSupporter"];
                    Session.Remove("ImageSupporter");
                }
               supporter.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.SupporterFacade.Insert(supporter, file))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = supporter.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Supporter/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View();
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {

            ViewBag.Id = Id;
            return View();
        }

       
        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var supporter = CongressComponent.Instance.BaseInfoComponents.SupporterFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(supporter);
                HttpPostedFileBase file = null;
                if (Session["ImageSupporter"] != null)
                {
                    file = (HttpPostedFileBase)Session["ImageSupporter"];
                    Session.Remove("ImageSupporter");
                }
                supporter.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.SupporterFacade.Update(supporter, file))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Supporter/Index");
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
                if (CongressComponent.Instance.BaseInfoComponents.SupporterFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/Supporter/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Supporter/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }

        public ActionResult List()
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.SupporterFacade.Where(
                    x => x.CongressId == this.Homa.Id);
            ViewBag.Type = CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.SelectKeyValuePair(x=>x.Id,x=>x.Title,x => x.CongressId == this.Homa.Id);
            return View(list);
        }
    }
}