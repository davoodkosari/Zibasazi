using System;
using System.Web.Mvc;
using Radyn.Common.Component;
using Radyn.Reservation;
using Radyn.Reservation.DataStructure;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;


namespace Radyn.WebApp.Areas.Reservation.Controllers
{
    public class ChairTypeController : LocalizedController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = ReservationComponent.Instance.ChairTypeFacade.GetAll();
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
            var paymentType = ReservationComponent.Instance.ChairTypeFacade.Get(Id);
            if (paymentType == null) return Content("false");
            return PartialView("PVDetails", paymentType);
        }
        public ActionResult GetModify(Guid? Id, string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            ViewBag.Currency =
                new SelectList(
                    EnumUtils.ConvertEnumToIEnumerableInLocalization<Radyn.Common.Definition.Enums.CurrencyType>(),
                    "Key", "Value");
            if (TempData["HallList"] == null)
                TempData["HallList"] = new SelectList(ReservationComponent.Instance.HallFacade.GetAll(), "Id", "Name");
            if (!Id.HasValue) return PartialView("PVModify", new ChairType());
            var languageContent = ReservationComponent.Instance.ChairTypeFacade.GetLanuageContent(culture, Id);
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
            var chairType = new ChairType();
            try
            {
                this.RadynTryUpdateModel(chairType);
                chairType.CurrentUICultureName = collection["LanguageId"];
                if (ReservationComponent.Instance.ChairTypeFacade.Insert(chairType))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Reservation/ChairType/Index");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Reservation/ChairType/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(chairType);
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
            var paymentType = ReservationComponent.Instance.ChairTypeFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(paymentType);
                paymentType.CurrentUICultureName = collection["LanguageId"];
                if (ReservationComponent.Instance.ChairTypeFacade.Update(paymentType))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Reservation/ChairType/Index");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Reservation/ChairType/Index");
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
            var paymentType = ReservationComponent.Instance.ChairTypeFacade.Get(Id);
            try
            {
                if (ReservationComponent.Instance.ChairTypeFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Reservation/ChairType/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Reservation/ChairType/Index");
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