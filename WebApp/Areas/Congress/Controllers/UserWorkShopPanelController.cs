using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Payment.Tools;
using Radyn.Utility;
using Radyn.Web.Html;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.FormGenerator.Tools;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class UserWorkShopPanelController : CongressBaseController
    {


        public ActionResult GetChild(Guid workShopId)
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.UserFacade.GetChildUsersForWorkShops(
                    SessionParameters.CongressUser, workShopId);
            return PartialView("PartialViewUserChild", list);
        }
        public ActionResult GetChildReserve(Guid workShopId, bool allowDelete=false)
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.Where(x=>(x.UserId== SessionParameters.CongressUser.Id || x.User.ParentId== SessionParameters.CongressUser.Id) &&
                    x.WorkShopId== workShopId);
            ViewBag.AllowDelete = allowDelete;
            return PartialView("PartialViewWorkShopUser", list);
        }

        [CongressUserAuthorize]
        public ActionResult IndexWorkShop()
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.Where(
                    x =>
                        x.UserId == SessionParameters.CongressUser.Id ||
                        x.User.ParentId == SessionParameters.CongressUser.Id);
           
            return View(list);
        }
        [CongressUserAuthorize]
        public ActionResult CreateWorkShop()
        {
            return ValidateCreate();
        }
        private ActionResult ValidateCreate()
        {

            var congressConfiguration = this.Homa.Configuration;
            if (congressConfiguration.WorkShopRezervStartDate.CompareTo(DateTime.Now.ShamsiDate()) > 0)
            {
                ShowMessage(Resources.Congress.WorkShopRezervDateNotStart + Tag.NewLine + Resources.Congress.PlesaetryIn + congressConfiguration.WorkShopRezervStartDate + Resources.Congress.Refer, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");

            }
            if (congressConfiguration.WorkShopRezervEndDate.CompareTo(DateTime.Now.ShamsiDate()) < 0)
            {
                ShowMessage(Resources.Congress.WorkShopRezervDateInEnd, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");
            }
            var countByUserId = CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.Count(x=>x.UserId==SessionParameters.CongressUser.Id||x.User.ParentId== SessionParameters.CongressUser.Id);
            if (countByUserId >= congressConfiguration.MaxWorkShopPerUser)
            {
                ShowMessage(Resources.Congress.WorkShopRezervCountIsFull, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                {
                    return Redirect("~/Congress/UserPanel/Home");
                }
            }
            var getunusedByUserId = CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.GetReservableWorkshop(this.Homa.Id);
            if (!getunusedByUserId.Any())
            {
                ShowMessage(Resources.Congress.WorkShopForReservNotFount, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");
            }
            ViewBag.WorkShops = new SelectList(getunusedByUserId, "Id", "DescriptionField");
            return View();


        }

        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult CreateWorkShop(FormCollection collection)
        {
            
            try
            {

                var workshopId = collection["Id"];
                if (string.IsNullOrEmpty(workshopId))
                {
                    ShowMessage(Resources.Congress.WorkShopForReservNotFount, Resources.Common.MessaageTitle,
                      messageIcon: MessageIcon.Warning);
                    return Redirect("~/Congress/UserWorkShopPanel/CreateWorkShop");
                }
                var config = this.Homa.Configuration;
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.MessaageTitle,
                       messageIcon: MessageIcon.Warning);
                    return Redirect("~/Congress/UserWorkShopPanel/CreateWorkShop");
                }
                var transactionDiscountAttaches = Payment.Tools.AppExtentions.FillTransactionDiscount(collection);
                if (transactionDiscountAttaches.Count > config.DisscountCount)
                {
                    ShowMessage(Resources.Congress.YouCanNotUseDiscoutOverThanMax + Tag.NewLine + Resources.Congress.DiscountCount + config.DisscountCount, Resources.Common.MessaageTitle,
                     messageIcon: MessageIcon.Warning);
                    return Redirect("~/Congress/UserWorkShopPanel/CreateWorkShop");
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
                    CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.WorkShopUserInsert(this.Homa.Id,
                        workshopId.ToGuid(), SessionParameters.CongressUser, transactionDiscountAttaches,
                       "/Congress/UserWorkShopPanel/UpdateStatusAfterTransaction?tempId=", postFormData, users);
                if (list.Key)
                {
                    this.ClearFormGeneratorData(postFormData.Id);
                    if (list.Value != Guid.Empty)
                        return Redirect("~" + Extentions.PrepaymenyUrl(list.Value));
                    
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/UserWorkShopPanel/IndexWorkShop");

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserWorkShopPanel/CreateWorkShop");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserWorkShopPanel/CreateWorkShop");
            }
        }

        public ActionResult UpdateStatusAfterTransaction(Guid tempId)
        {
            try
            {
                var tr=CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.UpdateStatusAfterTransaction(this.Homa.Id,SessionParameters.CongressUser.Id, tempId);
                return tr != Guid.Empty
                   ? Redirect("~/Payment/Transaction/TransactionResult?Id=" + tr +
                              "&callbackurl=/Congress/UserPanel/Home")
                   : Redirect("~/Congress/UserWorkShopPanel/IndexWorkShop");
                
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserWorkShopPanel/IndexWorkShop");
            }
        }

        [CongressUserAuthorize]
        public ActionResult EditWorkShop(Guid WorkShopId)
        {
            return ValidateEdit(WorkShopId);
        }
        private ActionResult ValidateEdit(Guid WorkShopId)
        {
            var congressConfiguration = this.Homa.Configuration;
            if (congressConfiguration.WorkShopRezervStartDate.CompareTo(DateTime.Now.ShamsiDate()) > 0)
            {
                ShowMessage(Resources.Congress.WorkShopRezervDateNotStart + Tag.NewLine + Resources.Congress.PlesaetryIn + congressConfiguration.WorkShopRezervStartDate + Resources.Congress.Refer, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");

            }
            if (congressConfiguration.WorkShopRezervEndDate.CompareTo(DateTime.Now.ShamsiDate()) < 0)
            {
                ShowMessage(Resources.Congress.WorkShopRezervDateInEnd, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");
            }
            ViewBag.Id = WorkShopId;
            return View();
        }
        [HttpPost]
        [CongressUserAuthorize]
        public ActionResult EditWorkShop(Guid WorkShopId, FormCollection collection)
        {
            
            try
            {
                var config = this.Homa.Configuration;
                var transactionDiscountAttaches = Payment.Tools.AppExtentions.FillTransactionDiscount(collection);
                if (transactionDiscountAttaches.Count > config.DisscountCount)
                {
                    ShowMessage(
                        Resources.Congress.YouCanNotUseDiscoutOverThanMax + Tag.NewLine +
                        Resources.Congress.DiscountCount + config.DisscountCount, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Warning);
                    return Redirect("~/Congress/UserWorkShopPanel/EditWorkShop?WorkShopId=" + WorkShopId);
                }
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.MessaageTitle,
                       messageIcon: MessageIcon.Warning);
                    return Redirect("~/Congress/UserWorkShopPanel/EditWorkShop?WorkShopId=" + WorkShopId );

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
                    CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.WorkShopUserInsert(this.Homa.Id,WorkShopId,
                        SessionParameters.CongressUser, transactionDiscountAttaches,
                        "/Congress/UserWorkShopPanel/UpdateStatusAfterTransaction?tempId=", postFormData, users);
                if (list.Key)
                {
                    this.ClearFormGeneratorData(postFormData.Id);
                    if (list.Value != Guid.Empty)
                        return Redirect("~" + Extentions.PrepaymenyUrl(list.Value));
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/UserWorkShopPanel/IndexWorkShop");

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserWorkShopPanel/EditWorkShop?WorkShopId=" + WorkShopId);

               
                
            }
            catch (Exception exception)
            {

                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserWorkShopPanel/EditWorkShop?WorkShopId=" + WorkShopId);

            }
        }
        [CongressUserAuthorize]
        public ActionResult DetailsWorkShop(Guid WorkShopId)
        {
            ViewBag.Id = WorkShopId;
            return View();
        }
        [CongressUserAuthorize]
        public ActionResult DeleteWorkShop(Guid WorkShopId)
        {
            ViewBag.Id = WorkShopId;
            return View();
        }
        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult DeleteWorkShop(Guid WorkShopId, FormCollection collection)
        {
            
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.Delete(SessionParameters.CongressUser.Id, WorkShopId))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/UserWorkShopPanel/IndexWorkShop");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserWorkShopPanel/IndexWorkShop");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = WorkShopId;
                return View();
            }
        }

    }
}
