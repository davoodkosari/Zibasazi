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
    public class UserBoothPanelController : CongressBaseController
    {

        [CongressUserAuthorize]
        public ActionResult IndexBooth()
        {
            Session.Remove("BoothOfficers");
            var list =
                CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.OrderByDescending(x=>x.RegisterDate,x=>x.UserId==SessionParameters.CongressUser.Id);
           
            return View(list);
        }

        [CongressUserAuthorize]
        public ActionResult CreateBooth()
        {
            return ValidateCreate();

        }

        private ActionResult ValidateCreate()
        {


            var congressConfiguration = this.Homa.Configuration;
            if (congressConfiguration.BoothRezervStartDate.CompareTo(DateTime.Now.ShamsiDate()) > 0)
            {
                ShowMessage(Resources.Congress.BoothRezervDateNotStart + Tag.NewLine + Resources.Congress.PlesaetryIn + congressConfiguration.HotelRezervStartDate + Resources.Congress.Refer, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");

            }
            if (congressConfiguration.BoothRezervEndDate.CompareTo(DateTime.Now.ShamsiDate()) < 0)
            {
                ShowMessage(Resources.Congress.BoothRezervDateIsEnd, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");
            }
            var countByUserId = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Count(x=>x.UserId==SessionParameters.CongressUser.Id);
            if (countByUserId >= congressConfiguration.MaxBoothPerUser)
            {
                ShowMessage(Resources.Congress.BoothRezervCountIsFull, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                {

                    return Redirect("~/Congress/UserPanel/Home");
                }
            }
            var getunusedByUserId = CongressComponent.Instance.BaseInfoComponents.BoothFacade.GetunusedByUserId(SessionParameters.CongressUser.Id, this.Homa.Id);
            if (!getunusedByUserId.Any())
            {
                ShowMessage(Resources.Congress.BoothForReservNotFount, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");

            }
            ViewBag.Booths = new SelectList(getunusedByUserId, "Id", "DescriptionField");
            var configurationContent = congressConfiguration.GetConfigurationContent();
            if (configurationContent != null)
                ViewBag.BoothMapId = configurationContent.BoothMapAttachmentId;
            return View(new UserBooth());
        }
        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult CreateBooth(FormCollection collection)
        {
            var userBooth = new UserBooth();
            try
            {
                var messageStack = new List<string>();
                this.RadynTryUpdateModel(userBooth);
                userBooth.UserId = SessionParameters.CongressUser.Id;
                var transactionDiscountAttaches = Payment.Tools.AppExtentions.FillTransactionDiscount(collection);
                var config = this.Homa.Configuration;
                if (transactionDiscountAttaches.Count > config.DisscountCount)
                    messageStack.Add(Resources.Congress.YouCanNotUseDiscoutOverThanMax + Tag.NewLine +
                                     Resources.Congress.DiscountCount + config.DisscountCount);
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                    messageStack.Add(postFormData.FillErrors);
                var boothOfficers = (List<BoothOfficer>)Session["BoothOfficers"];
                if (boothOfficers == null || !boothOfficers.Any())
                    messageStack.Add(Resources.Congress.PleaseEnterBoothOfficer);
                var booth = CongressComponent.Instance.BaseInfoComponents.BoothFacade.Get(userBooth.BoothId);
                if (booth != null && booth.MaxBoothOfficerCount != null && boothOfficers != null &&
                  boothOfficers.Count > booth.MaxBoothOfficerCount)
                    messageStack.Add(Resources.Congress.BoothOfficerCountismorethantheallowednumber + " " +
                                    Resources.Congress.Maximumallowed + ":" + booth.MaxBoothOfficerCount);
                var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                if (messageBody != "")
                {
                    ShowMessage(messageBody, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                    return Redirect("~/Congress/UserBoothPanel/CreateBooth");
                }
                var userBoothInsert =
                    CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.UserBoothInsert(this.Homa.Id,userBooth,
                        transactionDiscountAttaches,
                        "/Congress/UserBoothPanel/UpdateStatusAfterTransaction?Id=" + userBooth.BoothId, postFormData, boothOfficers);
                if (userBoothInsert)
                {
                    this.ClearFormGeneratorData(postFormData.Id);
                    Session.Remove("BoothOfficers");
                    if (userBooth.TempId.HasValue)
                    {

                        return Redirect("~" + Extentions.PrepaymenyUrl(userBooth.TempId.Value));
                    }
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Succeed);

                    return Redirect("~/Congress/UserBoothPanel/IndexBooth");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                      messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserBoothPanel/CreateBooth");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserBoothPanel/CreateBooth");
            }
        }

        [CongressUserAuthorize]
        public ActionResult EditBooth(Guid UserId, Guid BoothId)
        {
            return ValidateEdit(UserId, BoothId);
        }
        private ActionResult ValidateEdit(Guid UserId, Guid BoothId)
        {
            var congressConfiguration = this.Homa.Configuration;
            if (congressConfiguration.BoothRezervStartDate.CompareTo(DateTime.Now.ShamsiDate()) > 0)
            {
                ShowMessage(Resources.Congress.BoothRezervDateNotStart + Tag.NewLine + Resources.Congress.PlesaetryIn + congressConfiguration.HotelRezervStartDate + Resources.Congress.Refer, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");

            }
            if (congressConfiguration.BoothRezervEndDate.CompareTo(DateTime.Now.ShamsiDate()) < 0)
            {
                ShowMessage(Resources.Congress.BoothRezervDateIsEnd, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");
            }

            var model = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Get(UserId, BoothId);
            var configurationContent = congressConfiguration.GetConfigurationContent();
            if (configurationContent != null)
                ViewBag.BoothMapId = configurationContent.BoothMapAttachmentId;
            Session["BoothOfficers"] = CongressComponent.Instance.BaseInfoComponents.BoothOfficerFacade.Where(
            x =>

                    x.UserId == UserId &&
                    x.BoothId == BoothId);
            return View(model);
        }
        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult EditBooth(Guid UserId, Guid BoothId, FormCollection collection)
        {
            var userBooth = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Get(UserId, BoothId);
            try
            {
                var messageStack = new List<string>();
                this.RadynTryUpdateModel(userBooth);
                var config = this.Homa.Configuration;
                var transactionDiscountAttaches = Payment.Tools.AppExtentions.FillTransactionDiscount(collection);
                if (transactionDiscountAttaches.Count > config.DisscountCount)
                    messageStack.Add(Resources.Congress.YouCanNotUseDiscoutOverThanMax + Tag.NewLine +Resources.Congress.DiscountCount + config.DisscountCount);
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                    messageStack.Add(postFormData.FillErrors);
                var boothOfficers = (List<BoothOfficer>)Session["BoothOfficers"];
                if (boothOfficers == null || !boothOfficers.Any())
                    messageStack.Add(Resources.Congress.PleaseEnterBoothOfficer);
                var booth = CongressComponent.Instance.BaseInfoComponents.BoothFacade.Get(BoothId);
                if (booth != null && booth.MaxBoothOfficerCount != null && boothOfficers != null &&
                   boothOfficers.Count > booth.MaxBoothOfficerCount)
                    messageStack.Add(Resources.Congress.BoothOfficerCountismorethantheallowednumber+" "+
                                    Resources.Congress.Maximumallowed + ":" + booth.MaxBoothOfficerCount);
                var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                if (messageBody != "")
                {
                    ShowMessage(messageBody, Resources.Common.MessaageTitle,messageIcon: MessageIcon.Error);
                    return Redirect("~/Congress/UserBoothPanel/EditBooth?UserId=" + UserId + "&BoothId=" + BoothId);
                }
                if (CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.UserBoothUpdate(this.Homa.Id,userBooth, transactionDiscountAttaches, "/Congress/UserBoothPanel/UpdateStatusAfterTransaction?Id=" + userBooth.BoothId, postFormData, boothOfficers))
                {
                    this.ClearFormGeneratorData(postFormData.Id);
                    Session.Remove("BoothOfficers");
                    if (userBooth.TempId != null)
                    {
                        return Redirect("~" + Extentions.PrepaymenyUrl(userBooth.TempId.Value));
                    }
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                   messageIcon: MessageIcon.Succeed);
                    return this.Redirect("~/Congress/UserBoothPanel/EditBooth?UserId=" + UserId + "&BoothId=" + BoothId);

                }

                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserBoothPanel/EditBooth?UserId=" + UserId + "&BoothId=" + BoothId);

            }
            catch (Exception exception)
            {

                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserBoothPanel/EditBooth?UserId=" + UserId + "&BoothId=" + BoothId);

            }
        }
        public ActionResult UpdateStatusAfterTransaction(Guid Id)
        {
            try
            {
                var tr = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.UpdateStatusAfterTransaction(this.Homa.Id,SessionParameters.CongressUser.Id, Id);
                return tr != Guid.Empty
                 ? Redirect("~/Payment/Transaction/TransactionResult?Id=" + tr +
                            "&callbackurl=/Congress/UserBoothPanel/IndexBooth")
                            : Redirect("~/Congress/UserBoothPanel/IndexBooth");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserBoothPanel/IndexBooth");

            }
        }
        [CongressUserAuthorize]
        public ActionResult DetailsBooth(Guid UserId, Guid BoothId)
        {
            var hotelUser = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Get(UserId, BoothId);
            return View(hotelUser);
        }
        [CongressUserAuthorize]
        public ActionResult DeleteBooth(Guid UserId, Guid BoothId)
        {
            var hotelUser = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Get(UserId, BoothId);
            return View(hotelUser);
        }

        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult DeleteBooth(Guid UserId, Guid BoothId, FormCollection collection)
        {
            var userBooth = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Get(UserId, BoothId);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Delete(UserId, BoothId))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/UserBoothPanel/IndexBooth");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserBoothPanel/IndexBooth");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(userBooth);
            }
        }

    }
}
