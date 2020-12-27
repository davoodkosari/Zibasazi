using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class SupportTypeController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.Where(x=>x.CongressId==this.Homa.Id);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(Int16 Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult GetDetail(int Id)
        {
            var articlePaymentType = CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.Get(Id);
            if (articlePaymentType == null) return Content("false");
            return PartialView("PVDetails", articlePaymentType);
        }
        public ActionResult GetModify(int? Id)
        {
            ViewBag.SupporterShowType = new SelectList(
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.SupporterShowType>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.SupporterShowType>(),
                            keyValuePair.Value)), "Key", "Value");
            if (!Id.HasValue) return PartialView("PVModify", new SupportType());
            var supportType = CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.Get(Id);
            return PartialView("PVModify", supportType);
        }
        [RadynAuthorize]
        public ActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var supportType = new SupportType();
            try
            {
                this.RadynTryUpdateModel(supportType);
                supportType.CongressId = this.Homa.Id;
                supportType.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.Insert(this.Homa.Id,supportType))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = supportType.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/SupportType/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(supportType);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Int16 Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Int16 Id, FormCollection collection)
        {
            var supportType = CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(supportType);
                supportType.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.Update(this.Homa.Id, supportType))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/SupportType/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(supportType);
            }
        }
        public ActionResult GetSupportTypes(bool enable=true,Guid? congressId=null)
        {
            ViewBag.enable = enable;
            return PartialView("PartialViewSupportTypes", CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.GetSupportTypeModel(congressId.HasValue?congressId.Value:this.Homa.Id));
        }
        [RadynAuthorize]
        public ActionResult Delete(Int16 Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(Int16 Id, FormCollection collection)
        {
            var supportType = CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/SupportType/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/SupportType/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(supportType);
            }
        }
    }
}