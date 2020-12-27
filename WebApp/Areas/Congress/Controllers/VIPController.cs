using Radyn.Common.Component;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class VIPController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.VipFacade.Where(
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
            var articlePaymentType = CongressComponent.Instance.BaseInfoComponents.VipFacade.Get(Id);
            if (articlePaymentType == null) return Content("false");
            return PartialView("PVDetails", articlePaymentType);
        }
        public ActionResult GetModify(Guid? Id, string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            ViewBag.IsNew = Id == null;
            if (!Id.HasValue) return PartialView("PVModify", new VIP());
            var languageContent = CongressComponent.Instance.BaseInfoComponents.VipFacade.GetLanuageContent(culture, Id);
            return PartialView("PVModify", languageContent);
        }
        [RadynAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var vIP = new VIP(){EnterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode { RealEnterpriseNode = new RealEnterpriseNode() }};
            try
            {

                this.RadynTryUpdateModel(vIP);
                this.RadynTryUpdateModel(vIP.EnterpriseNode);
                this.RadynTryUpdateModel(vIP.EnterpriseNode.RealEnterpriseNode);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                HttpPostedFileBase fileResume = null;
                if (Session["ImageResume"] != null)
                {
                    fileResume = (HttpPostedFileBase)Session["ImageResume"];
                    Session.Remove("ImageResume");
                }
                vIP.CongressId = this.Homa.Id;
                vIP.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.VipFacade.Insert(vIP, fileResume, file))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, new[] { Resources.Common.Ok, " window.location='/Congress/VIP/Index'; " }, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = vIP.Id });

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/VIP/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(vIP);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var vIP = CongressComponent.Instance.BaseInfoComponents.VipFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(vIP);
                this.RadynTryUpdateModel(vIP.EnterpriseNode);
                this.RadynTryUpdateModel(vIP.EnterpriseNode.RealEnterpriseNode);
                vIP.CurrentUICultureName = collection["LanguageId"];
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                HttpPostedFileBase fileResume = null;
                if (Session["ImageResume"] != null)
                {
                    fileResume = (HttpPostedFileBase)Session["ImageResume"];
                    Session.Remove("ImageResume");
                }
                if (CongressComponent.Instance.BaseInfoComponents.VipFacade.Update(vIP, fileResume, file))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, new[] { Resources.Common.Ok, " window.location='/Congress/VIP/Index'; " }, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/VIP/Index");
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
                if (CongressComponent.Instance.BaseInfoComponents.VipFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/VIP/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/VIP/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }


        public ActionResult VIPDetail(Guid id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.VipFacade.Get(id));
        }

    }
}