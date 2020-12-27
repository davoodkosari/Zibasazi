using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Common;
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
    public class UserRegisterPaymentTypeController : CongressBaseController
    {

        public ActionResult GetDaysInfo(string culture,Guid? Id)
        {
            var list = CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.GetDaysInfo(this.Homa.Id, culture,Id);
            return PartialView("PVDaysInfo", list);
        }
        public ActionResult GetRegiterTypeInfo(Guid Id)
        {
            var list = CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.GetRegiterTypeInfo(Id);
            var registerPaymentType = CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.Get(Id);
            if (registerPaymentType != null)
                ViewBag.CurrencyType = registerPaymentType.CurrencyType;
            return PartialView("PVRegiterTypeInfo", list);
        }
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.Where(
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
            var userRegisterPaymentType = CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.Get(Id);
            if (userRegisterPaymentType == null) return Content("false");
            return PartialView("PVDetails", userRegisterPaymentType);
        }
        public ActionResult GetModify(Guid? Id, string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            ViewBag.Currency =
                new SelectList(
                    EnumUtils.ConvertEnumToIEnumerableInLocalization<Radyn.Common.Definition.Enums.CurrencyType>(),
                    "Key", "Value");
            ViewBag.culture = culture;
            if (!Id.HasValue) return PartialView("PVModify", new UserRegisterPaymentType());
            var languageContent = CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.GetLanuageContent(culture,Id);
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
            var paymentType = new UserRegisterPaymentType();
            try
            {
                this.RadynTryUpdateModel(paymentType);
                paymentType.CongressId = this.Homa.Id;
               var keyValuePairs = new Dictionary<int,decimal>();
                if (!collection["AllDays"].ToBool())
                {
                    foreach (var variable in collection.AllKeys.Where(x => x.StartsWith("Amount-")))
                    {
                        var key = variable.Substring(7, variable.Length - 7);
                        keyValuePairs.Add(key.ToInt(),collection[variable].ToDecimal());
                    }
                }
                paymentType.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.Insert(paymentType, keyValuePairs))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = paymentType.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserRegisterPaymentType/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(paymentType);
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
            var paymentType = CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(paymentType);
                var keyValuePairs = new Dictionary<int,decimal>();
                if (!collection["AllDays"].ToBool())
                {
                    foreach (var variable in collection.AllKeys.Where(x => x.StartsWith("Amount-")))
                    {
                        var key = variable.Substring(7, variable.Length - 7);
                        keyValuePairs.Add(key.ToInt(),collection[variable].ToDecimal());
                    }
                }
                paymentType.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.Update(paymentType, keyValuePairs))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserRegisterPaymentType/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(paymentType);
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
            var paymentType = CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/UserRegisterPaymentType/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserRegisterPaymentType/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(paymentType);
            }
        }
    }
}