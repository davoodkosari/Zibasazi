using System;
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
    public class GroupRegisterDiscountController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.GroupRegisterDiscountFacade.Where(x=>x.CongressId==this.Homa.Id);
            if (list.Count == 0) return this.Redirect("~/Congress/GroupRegisterDiscount/Create");
            return View(list);
        }
        public ActionResult GetValidList()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.GroupRegisterDiscountFacade.GetValidList(this.Homa.Id);
            return PartialView("PartialViewDetails", list);
        }
        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult GetDetail(Guid Id)
        {
            var articlePaymentType = CongressComponent.Instance.BaseInfoComponents.GroupRegisterDiscountFacade.Get(Id);
            if (articlePaymentType == null) return Content("false");
            return PartialView("PVDetails", articlePaymentType);
        }
        public ActionResult GetModify(Guid? Id, string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            ViewBag.Currency =
                new SelectList(
                    EnumUtils.ConvertEnumToIEnumerableInLocalization<Radyn.Common.Definition.Enums.CurrencyType>(),
                    "Key", "Value");
            if (!Id.HasValue) return PartialView("PVModify", new GroupRegisterDiscount(){Enable = true});
            var languageContent = CongressComponent.Instance.BaseInfoComponents.GroupRegisterDiscountFacade.GetLanuageContent(culture,Id);
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
            var groupRegisterDiscount = new GroupRegisterDiscount();
            try
            {
                this.RadynTryUpdateModel(groupRegisterDiscount);
                groupRegisterDiscount.CongressId = this.Homa.Id;
                groupRegisterDiscount.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.GroupRegisterDiscountFacade.Insert(groupRegisterDiscount))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = groupRegisterDiscount.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return this.Redirect("~/Congress/GroupRegisterDiscount/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(groupRegisterDiscount);
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
            var groupRegisterDiscount = CongressComponent.Instance.BaseInfoComponents.GroupRegisterDiscountFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(groupRegisterDiscount);
                groupRegisterDiscount.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.GroupRegisterDiscountFacade.Update(groupRegisterDiscount))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return this.Redirect("~/Congress/GroupRegisterDiscount/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(groupRegisterDiscount);
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
            var groupRegisterDiscount = CongressComponent.Instance.BaseInfoComponents.GroupRegisterDiscountFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.GroupRegisterDiscountFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Succeed);
                    return this.Redirect("~/Congress/GroupRegisterDiscount/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return this.Redirect("~/Congress/GroupRegisterDiscount/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(groupRegisterDiscount);
            }
        }
    }
}
