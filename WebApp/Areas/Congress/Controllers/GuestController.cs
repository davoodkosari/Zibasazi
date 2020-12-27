using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.Payment.Tools;
using Radyn.Web.Html;
using Radyn.Web.Mvc.UI.Message;
using Radyn.Utility;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.FormGenerator.Tools;
using Enums = Radyn.Congress.Tools.Enums;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class GuestController : CongressBaseController
    {

        public ActionResult ReservDetail(Guid Id)
        {

            ViewBag.Id = Id;
            var configuration = this.Homa.Configuration;
            ViewBag.Booths =
                CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Where(
                    x => x.UserId == Id);
            var configurationContent = configuration.GetConfigurationContent();
            if (configurationContent != null)
                ViewBag.BoothMapId = configurationContent.BoothMapAttachmentId;
            return View();

        }

        public ActionResult RegisterGuest()
        {
            var config = this.Homa.Configuration;
            if (!this.Homa.Configuration.HasBooth)
            {
                ShowMessage(Resources.Congress.BoothReservNotEnable, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                return RedirectToAction("GetConfiguration", "Configuration");

            }
            if (config.BoothRezervStartDate.CompareTo(DateTime.Now.ShamsiDate()) > 0)
                ShowMessage(Resources.Congress.BoothRezervDateNotStart + Tag.NewLine + Resources.Congress.PlesaetryIn + config.BoothRezervStartDate + Resources.Congress.Refer, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
            if (config.BoothRezervEndDate.CompareTo(DateTime.Now.ShamsiDate()) < 0)
                ShowMessage(Resources.Congress.BoothRezervDateIsEnd, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
            if (!this.Homa.Configuration.RegisterForReservBooth) return View();
            ShowMessage(Resources.Congress.ForBoothreservationsyoumustregister, Resources.Common.Attantion,
                messageIcon: MessageIcon.Warning);
            return View();
        }


        [HttpPost]
        public ActionResult RegisterGuest(FormCollection collection)
        {

            try
            {
                var messageStack = new List<string>();
                var enterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode();
                this.RadynTryUpdateModel(enterpriseNode);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                switch (enterpriseNode.EnterpriseNodeTypeId)
                {
                    case 1:
                        enterpriseNode.RealEnterpriseNode = new RealEnterpriseNode();
                        this.RadynTryUpdateModel(enterpriseNode.RealEnterpriseNode);
                        if (string.IsNullOrEmpty(enterpriseNode.RealEnterpriseNode.FirstName))
                            messageStack.Add(Resources.Congress.Please_Enter_YourName);
                        if (string.IsNullOrEmpty(enterpriseNode.RealEnterpriseNode.LastName))
                            messageStack.Add(Resources.Congress.Please_Enter_YourLastName);
                        if (string.IsNullOrEmpty(enterpriseNode.Cellphone))
                            messageStack.Add(Resources.Congress.Please_Enter_YourMobile);
                        if (enterpriseNode.RealEnterpriseNode.Gender == null)
                            messageStack.Add(Resources.Congress.Please_Enter_YourGender);
                        if (string.IsNullOrEmpty(enterpriseNode.Email))
                            messageStack.Add(Resources.Congress.PleaseEnterYourEmail);
                        else
                        {
                            if (!Utils.IsEmail(enterpriseNode.Email))
                                messageStack.Add(Resources.Congress.UnValid_Enter_Email);
                        }
                        break;
                    case 2:
                        enterpriseNode.LegalEnterpriseNode = new LegalEnterpriseNode();
                        this.RadynTryUpdateModel(enterpriseNode.LegalEnterpriseNode);
                        if (string.IsNullOrEmpty(enterpriseNode.Cellphone))
                            messageStack.Add(Resources.Congress.Please_Enter_YourMobile);
                        if (string.IsNullOrEmpty(enterpriseNode.Email))
                            messageStack.Add(Resources.Congress.PleaseEnterYourEmail);
                        else
                        {
                            if (!Utils.IsEmail(enterpriseNode.Email))
                                messageStack.Add(Resources.Congress.UnValid_Enter_Email);
                        }
                        break;
                }
                var listboothId = new List<Guid>();
                foreach (var key in collection.AllKeys.Where(s => s.StartsWith("CheckSelect-")))
                {
                    if (string.IsNullOrEmpty(collection[key])) continue;
                    listboothId.Add(collection[key].ToGuid());
                }
                var config = this.Homa.Configuration;
                if (listboothId.Count == 0)
                    messageStack.Add(Resources.Congress.NoSelectedBoothItem);
                var transactionDiscountAttaches = Payment.Tools.AppExtentions.FillTransactionDiscount(collection);
                if (transactionDiscountAttaches.Count > config.DisscountCount)
                    messageStack.Add(Resources.Congress.YouCanNotUseDiscoutOverThanMax + Tag.NewLine + Resources.Congress.DiscountCount + config.DisscountCount);
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                    messageStack.Add(postFormData.FillErrors);
                var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                if (messageBody != "")
                {
                    ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return View();
                }

                var insertGuest = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.InsertGuest(enterpriseNode, listboothId, file, transactionDiscountAttaches,
                    "/Congress/Guest/UpdateStatusAfterTransaction?Id=", postFormData, this.Homa.Id);
                if (insertGuest.Key)
                {
                    Session.Remove("BoothOfficers");
                    if (insertGuest.Value != Guid.Empty)
                    {

                        return Redirect("~" + Extentions.PrepaymenyUrl(insertGuest.Value));
                    }
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                   messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/Guest/ReservDetail?Id=" + enterpriseNode.Id);

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                   messageIcon: MessageIcon.Error);
                return View();

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View();
            }
        }

        public ActionResult UpdateStatusAfterTransaction(Guid Id)
        {
            try
            {
                CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.UpdateStatusAfterTransactionGuest(this.Homa.Id,Id);
                return Redirect("~/Congress/Guest/ReservDetail?Id=" + Id);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/Guest/ReservDetail?Id=" + Id);
            }
        }



        public ActionResult Getbooths()
        {

            var configuration = this.Homa.Configuration;
            var booths =
                CongressComponent.Instance.BaseInfoComponents.BoothFacade.GerReservableBooth(this.Homa.Id);
            var configurationContent = configuration.GetConfigurationContent();
            if (configurationContent != null)
                ViewBag.BoothMapId = configurationContent.BoothMapAttachmentId;
            return PartialView("PartialViewBooths", booths);


        }


        [RadynAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            try
            {

                var user = new User { RegisterDate = collection["RegisterDate"], StatusNullable = (byte?)Enums.UserStatus.Guest };
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.Search(this.Homa.Id, string.Empty, user, Enums.AscendingDescending.Ascending, Enums.SortAccordingToUser.RegisterDate, Radyn.EnterpriseNode.Tools.Enums.Gender.None, postFormData);
                return PartialView("PVGuest", list);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        [HttpPost]
        public ActionResult GenerateUser(FormCollection collection)
        {

            try
            {
                if (string.IsNullOrEmpty(collection["CountUser"]))
                {
                    ShowMessage(Resources.Congress.PleaseEnterCountOfGuest, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var count = collection["CountUser"].ToInt();
                var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.GenerateGuest(this.Homa.Id, count);
                return PartialView("PVGuest", list);

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
    }
}
