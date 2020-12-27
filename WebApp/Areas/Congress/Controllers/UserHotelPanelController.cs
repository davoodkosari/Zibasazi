using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Payment.Tools;
using Radyn.Utility;
using Radyn.Web.Html;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.FormGenerator.Tools;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class UserHotelPanelController : CongressBaseController
    {


        public ActionResult GetChild(Guid hotelId)
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.UserFacade.GetChildUsersForHotel(
                    SessionParameters.CongressUser, hotelId);
            return PartialView("PartialViewUserChild", list);
        }
        public ActionResult GetChildReserve(Guid hotelId, bool allowDelete = false)
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.Where(
                    x =>
                        (x.UserId == SessionParameters.CongressUser.Id ||
                         x.User.ParentId == SessionParameters.CongressUser.Id) && x.HotelId == hotelId);
            ViewBag.AllowDelete = allowDelete;
            return PartialView("PartialViewHotelUser", list);
        }

        [CongressUserAuthorize]
        public ActionResult IndexHotel()
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.Where(x=>x.UserId==SessionParameters.CongressUser.Id||x.User.ParentId== SessionParameters.CongressUser.Id);
          
            return View(list);
        }

        [CongressUserAuthorize]
        public ActionResult CreateHotel()
        {
            return ValidateCreate();

        }

        private ActionResult ValidateCreate()
        {


            var congressConfiguration = this.Homa.Configuration;
            if (congressConfiguration.HotelRezervStartDate.CompareTo(DateTime.Now.ShamsiDate()) > 0)
            {
                ShowMessage(Resources.Congress.HotelRezervDateNotStart + Tag.NewLine + Resources.Congress.PlesaetryIn + congressConfiguration.HotelRezervStartDate + Resources.Congress.Refer, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");

            }
            if (congressConfiguration.HotelRezervEndDate.CompareTo(DateTime.Now.ShamsiDate()) < 0)
            {
                ShowMessage(Resources.Congress.HotelRezervDateIsEnd, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");
            }
            var countByUserId = CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.Count(x=>x.HotelId,x=>x.UserId==SessionParameters.CongressUser.Id||x.User.ParentId== SessionParameters.CongressUser.Id);
            if (countByUserId >= congressConfiguration.MaxHotelPerUser)
            {
                ShowMessage(Resources.Congress.HotelRezervCountIsFull, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                {
                    return Redirect("~/Congress/UserPanel/Home");
                }
            }

            var getunusedByUserId = CongressComponent.Instance.BaseInfoComponents.HotelFacade.GetReservableHotel(this.Homa.Id);
            if (!getunusedByUserId.Any())
            {
                ShowMessage(Resources.Congress.HotelForReservNotFount, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");
            }
            ViewBag.Hotels = new SelectList(getunusedByUserId, "Id", "DescriptionField");
            ViewBag.dailyCount = this.Homa.Configuration.HotelReservDailyCount;
            return View();
        }

        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult CreateHotel(FormCollection collection)
        {
           
            try
            {
               
                var daysCount = collection["DaysCount"].ToInt();
                var hotelId = collection["Id"];
                if (string.IsNullOrEmpty(hotelId))
                {
                     ShowMessage(Resources.Congress.HotelForReservNotFount, Resources.Common.MessaageTitle,
                      messageIcon: MessageIcon.Error);
                    return Redirect("~/Congress/UserHotelPanel/CreateHotel");
                }
                if (daysCount == 0)
                {
                    ShowMessage(Resources.Congress.Please_Enter_Rezerv_DayCount, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Error);
                    return Redirect("~/Congress/UserHotelPanel/CreateHotel");
                }
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.MessaageTitle,
                       messageIcon: MessageIcon.Warning);
                    return Redirect("~/Congress/UserHotelPanel/CreateHotel");
                }
                var transactionDiscountAttaches = Payment.Tools.AppExtentions.FillTransactionDiscount(collection);
                var config = this.Homa.Configuration;
                if (transactionDiscountAttaches.Count > config.DisscountCount)
                {
                    ShowMessage(
                        Resources.Congress.YouCanNotUseDiscoutOverThanMax + Tag.NewLine +
                        Resources.Congress.DiscountCount + config.DisscountCount, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Warning);
                    return Redirect("~/Congress/UserHotelPanel/CreateHotel");
                }
                var users = new List<Guid>();
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("CheckSelect"));
                if (!string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    var strings = collection[firstOrDefault].Split(',');
                    foreach (var value in strings)
                    {
                        if (string.IsNullOrEmpty(value)) continue;
                        users.Add(value.ToGuid());
                    }
                }
                var list =
                    CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.HotelUserInsert(this.Homa.Id,hotelId.ToGuid(),
                        SessionParameters.CongressUser, transactionDiscountAttaches,
                         "/Congress/UserHotelPanel/UpdateStatusAfterTransaction?tempId=", postFormData, daysCount, users);
                if (list.Key)
                {
                    this.ClearFormGeneratorData(postFormData.Id);
                    if (list.Value!=Guid.Empty)
                    {

                        return Redirect("~" + Extentions.PrepaymenyUrl(list.Value));
                    }
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/UserHotelPanel/IndexHotel");

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserHotelPanel/CreateHotel");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserHotelPanel/CreateHotel");
            }
        }

        [CongressUserAuthorize]
        public ActionResult EditHotel(Guid HotelId)
        {
            return ValidateEdit(HotelId);
        }
        private ActionResult ValidateEdit(Guid HotelId)
        {


            var congressConfiguration = this.Homa.Configuration;
            if (congressConfiguration.HotelRezervStartDate.CompareTo(DateTime.Now.ShamsiDate()) > 0)
            {
                ShowMessage(Resources.Congress.HotelRezervDateNotStart + Tag.NewLine + Resources.Congress.PlesaetryIn + congressConfiguration.HotelRezervStartDate + Resources.Congress.Refer, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");

            }
            if (congressConfiguration.HotelRezervEndDate.CompareTo(DateTime.Now.ShamsiDate()) < 0)
            {
                ShowMessage(Resources.Congress.HotelRezervDateIsEnd, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");
            }
           
            ViewBag.dailyCount = this.Homa.Configuration.HotelReservDailyCount;
            ViewBag.Id = HotelId;
            return View();
        }
        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult EditHotel(Guid HotelId, FormCollection collection)
        {
           
            try
            {

                var daysCount = collection["DaysCount"].ToInt();
                var config = this.Homa.Configuration;
                var transactionDiscountAttaches = Payment.Tools.AppExtentions.FillTransactionDiscount(collection);
                if (transactionDiscountAttaches.Count > config.DisscountCount)
                {
                    ShowMessage(
                        Resources.Congress.YouCanNotUseDiscoutOverThanMax + Tag.NewLine +
                        Resources.Congress.DiscountCount + config.DisscountCount, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Warning);
                    return Redirect("~/Congress/UserHotelPanel/EditHotel?HotelId=" + HotelId);

                }
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.MessaageTitle,
                       messageIcon: MessageIcon.Warning);
                    return Redirect("~/Congress/UserHotelPanel/EditHotel?HotelId=" + HotelId);
                }
                var users = new List<Guid>();
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("CheckSelect"));
                if (!string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    var strings = collection[firstOrDefault].Split(',');
                    foreach (var value in strings)
                    {
                        if (string.IsNullOrEmpty(value)) continue;
                        users.Add(value.ToGuid());
                    }
                }
                var list = CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.HotelUserInsert(this.Homa.Id,HotelId,
                    SessionParameters.CongressUser, transactionDiscountAttaches,
                    "/Congress/UserHotelPanel/UpdateStatusAfterTransaction?tempId=", postFormData, daysCount, users);
                if (list.Key)
                {
                    this.ClearFormGeneratorData(postFormData.Id);
                    if (list.Value != Guid.Empty)
                    {

                        return Redirect("~" + Extentions.PrepaymenyUrl(list.Value));
                    }
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/UserHotelPanel/IndexHotel");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                 messageIcon: MessageIcon.Succeed);
                return Redirect("~/Congress/UserHotelPanel/EditHotel?HotelId=" + HotelId);

            }
            catch (Exception exception)
            {

                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserHotelPanel/EditHotel?HotelId=" + HotelId);

            }
        }
        public ActionResult UpdateStatusAfterTransaction(Guid tempId)
        {
            try
            {
               var tr= CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.UpdateStatusAfterTransaction(this.Homa.Id,SessionParameters.CongressUser.Id, tempId);
                return tr != Guid.Empty
             ? Redirect("~/Payment/Transaction/TransactionResult?Id=" + tr +
                        "&callbackurl=/Congress/UserHotelPanel/IndexHotel")
                        : Redirect("~/Congress/UserHotelPanel/IndexHotel");
               
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserHotelPanel/IndexHotel");
            }
        }
        [CongressUserAuthorize]
        public ActionResult DetailsHotel(Guid HotelId)
        {
            ViewBag.Id = HotelId;
            return View();
        }
        [CongressUserAuthorize]
        public ActionResult DeleteHotel(Guid HotelId)
        {
            ViewBag.Id = HotelId;
            return View();
        }

        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult DeleteHotel(Guid HotelId, FormCollection collection)
        {

            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.Delete(HotelId, SessionParameters.CongressUser.Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/UserHotelPanel/IndexHotel");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserHotelPanel/IndexHotel");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = HotelId;
                return View();
            }
        }

    }
}
