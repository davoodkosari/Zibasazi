using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.FormGenerator;
using Radyn.FormGenerator.Tools;
using Radyn.Message;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.Utility;
using Radyn.Web.Html;
using Radyn.Web.Mvc.UI.Captcha;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.FormGenerator.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using Radyn.Security.Tools;
using Enums = Radyn.Congress.Tools.Enums;
using Exception = System.Exception;
using Extentions = Radyn.Payment.Tools.Extentions;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class UserPanelController : CongressBaseController
    {

        public ActionResult Login()
        {
            if (this.Homa.Status != Enums.CongressStatus.NoProblem)
                return Redirect("/Home/Index");
            if (SessionParameters.CongressUser != null)
                return Redirect("~/Congress/UserPanel/Home");
            Session["Ipaddress"] = Request.ServerVariables["REMOTE_ADDR"];
            Session["RequestCount"] = 0;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("نام کاربری یا رمز عبور را وارد کنید");
                if (!WebUtility.IsLocal()&& SessionParameters.HasLoginPasswordError)
                {
                    var service = new CaptchaService();
                    if (!service.IsValidCaptcha(model.Captch))
                        throw new Exception(Resources.Security.CapchaIsNotCorrect);
                }
                var userName = model.Username;
                var password = model.Password;
                if (Session["Ipaddress"] != null && Session["Ipaddress"].Equals(Request.ServerVariables["REMOTE_ADDR"]) &&
                    Session["RequestCount"].ToString().ToInt() >= 4)
                    throw new Exception(Resources.Security.Please_enter_the_correct_values);
                var user = await CongressComponent.Instance.BaseInfoComponents.UserFacade.LoginAsync(userName, password, this.Homa.Id);
                if (user == null || user.Status == (byte) Enums.UserStatus.PreRegister)
                    throw new Exception(Resources.Security.Please_enter_the_correct_values);
                SessionParameters.CongressUser = user;
                SessionParameters.HasLoginPasswordError = false;
                Session.Remove("Ipaddress");
                Session.Remove("RequestCount");
                return !string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                    ? this.RadynRedirect(Request.QueryString["returnUrl"])
                    : this.RadynRedirect("~/Congress/UserPanel/Home");

            }
            catch (Exception ex)
            {
                SessionParameters.HasLoginPasswordError = true;
                ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                ViewBag.Message = ex.Message;
                model=new LoginViewModel(){Username = model.Username};
                return View(model);
            }
          
        }

        [CongressUserAuthorize]
        public ActionResult EditInfoUser()
        {

            return View();
        }

        [CongressUserAuthorize]
        public ActionResult UserTemps()
        {


            return
                Redirect("~/Payment/UserTransaction/UserTransaction?userId=" + SessionParameters.CongressUser.Id +
                         "&callbackurl=/Congress/UserPanel/Home" + "&postdataUrl=/Congress/UserPanel/AddUserTemp");


        }
        public ActionResult TrackingOrders()
        {


            return
                Redirect("~/Payment/Transaction/SearchTransaction");


        }
        public ActionResult AddUserTemp(FormCollection collection)
        {


            if (!string.IsNullOrEmpty(collection["PayGroup"]) && collection["PayGroup"].ToBool())
            {
                try
                {
                    var temp = new Temp { Id = Guid.NewGuid() };
                    temp.AdditionalData = CongressComponent.Instance.BaseInfoComponents.CongressDiscountTypeFacade.FillTempAdditionalData(this.Homa.Id);
                    var model = new List<Guid>();
                    var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("CheckSelect"));
                    if (string.IsNullOrEmpty(collection[firstOrDefault])) return Json(new { Result = false, Url = "" }, JsonRequestBehavior.AllowGet);
                    var strings = collection[firstOrDefault].Split(',');
                    foreach (var vale in strings)
                    {
                        if (string.IsNullOrEmpty(vale)) continue;
                        model.Add(vale.ToGuid());
                    }
                    if (model.Count == 0)
                    {
                        ShowMessage(Resources.Payment.PleaseSelectTempforPayment, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                        return Json(new { Result = false, Url = "" }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(collection["C-UserId"]))
                    {
                        ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                             messageIcon: MessageIcon.Error);
                        return Json(new { Result = false, Url = "" }, JsonRequestBehavior.AllowGet);
                    }
                    if (temp.Id == Guid.Empty) temp.Id = Guid.NewGuid();
                    temp.PayerId = collection["C-UserId"].ToGuid();
                    temp.PayerTitle = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get((Guid)temp.PayerId).DescriptionField;
                    temp.CallBackUrl = "/Congress/UserPanel/UpdateStatusAfterTransactionGroupTemp?Id=" + temp.Id;

                    if (PaymentComponenets.Instance.TempFacade.GroupPayTemp(temp, model))
                    {
                        return Json(new { Result = true, Url = Extentions.PrepaymenyUrl(temp.Id) }, JsonRequestBehavior.AllowGet);
                    }
                    ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                              messageIcon: MessageIcon.Error);
                    return Json(new { Result = false, Url = "" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception exception)
                {

                    ShowExceptionMessage(exception);
                    return Json(new { Result = false, Url = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            try
            {
                var temp = new Temp
                {
                    Id = Guid.NewGuid(),
                    AdditionalData =
                        CongressComponent.Instance.BaseInfoComponents.CongressDiscountTypeFacade.FillTempAdditionalData(
                            this.Homa.Id)
                };
                var dirty = !string.IsNullOrEmpty(collection["Id"]) && collection["Id"].ToGuid() != Guid.Empty;
                if (dirty)
                    temp = PaymentComponenets.Instance.TempFacade.Get(collection["Id"].ToGuid());
                this.RadynTryUpdateModel(temp, collection);
                var messageStack = new List<string>();

                if (temp.Amount == 0)
                    messageStack.Add(Resources.Payment.Please_Enter_Amount);
                if (string.IsNullOrEmpty(temp.Description))
                    messageStack.Add(Resources.Payment.PleaseEnterDescription);
                var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                if (messageBody != "")
                {
                    ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return Content("false");
                }
                temp.AdditionalData = CongressComponent.Instance.BaseInfoComponents.CongressDiscountTypeFacade.FillTempAdditionalData(this.Homa.Id);
                if (!dirty)
                {
                    if (string.IsNullOrEmpty(collection["UserId"]) || string.IsNullOrEmpty(collection["callbackurl"]))
                    {
                        ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                        return Content("false");
                    }
                    if (temp.Id == Guid.Empty) temp.Id = Guid.NewGuid();
                    temp.PayerId = collection["UserId"].ToGuid();
                    temp.PayerTitle = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get((Guid)temp.PayerId).DescriptionField;
                    temp.CallBackUrl = "/Congress/UserPanel/UpdateStatusAfterTransactionGroupTemp?Id=" + temp.Id;
                    if (PaymentComponenets.Instance.TempFacade.Insert(temp))
                    {
                        ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Succeed);
                        return Content("true");
                    }
                }
                else
                {
                    if (PaymentComponenets.Instance.TempFacade.Update(temp))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Succeed);
                        return Content("true");
                    }
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {

                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        [CongressUserAuthorize]
        public ActionResult UpdateStatusAfterTransactionGroupTemp(Guid id)
        {
            try
            {
                var tr = CongressComponent.Instance.BaseInfoComponents.CongressDiscountTypeFacade.UpdateStatusAfterTransactionGroupTemp(SessionParameters.CongressUser.Id, id);
                return tr != Guid.Empty
                   ? Redirect("~/Payment/Transaction/TransactionResult?Id=" + tr +
                              "&callbackurl=/Congress/UserPanel/Home")
                   : Redirect("~/Congress/UserPanel/Home");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserPanel/Home");
            }
        }

        [CongressUserAuthorize]
        public ActionResult UserFiles()
        {

            return View(CongressComponent.Instance.BaseInfoComponents.UserFileFacade.Select(x => x.File, x => x.CongressId == this.Homa.Id));
        }
        [CongressUserAuthorize]
        public ActionResult UserPayment()
        {
            ViewBag.paymentTypes = new SelectList(CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.GetValidListUser(this.Homa.Id), "Id", "DescriptionField");
            var enumerable = CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetParents(this.Homa.Id, true);
            ViewBag.CongressHalls = new SelectList(enumerable, "Id", "Name");
            ViewBag.HasHall = enumerable != null && enumerable.Any();
            return View(SessionParameters.CongressUser);
        }
        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult UserPayment(FormCollection collection)
        {

            try
            {

                var user = SessionParameters.CongressUser;
                if (string.IsNullOrEmpty(collection["PaymentTypeId"]))
                {
                    ShowMessage(Resources.Congress.PleaseEnterUserPaymentType, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                    return Redirect("~/Congress/UserPanel/Home");
                }
                var guids = collection["SelectedChairId"];
                if (!string.IsNullOrEmpty(guids))
                {
                    var value = guids.Split(',');
                    if (!string.IsNullOrEmpty(value[0]))
                        user.ChairId = value[0].ToGuid();
                }
                user.PaymentTypeId = collection["PaymentTypeId"].ToGuid();
                var config =
                    this.Homa.Configuration;
                var transactionDiscountAttaches = Payment.Tools.AppExtentions.FillTransactionDiscount(collection);
                if (transactionDiscountAttaches.Count > config.DisscountCount)
                {
                    ShowMessage(
                        Resources.Congress.YouCanNotUseDiscoutOverThanMax + Tag.NewLine +
                        Resources.Congress.DiscountCount + config.DisscountCount, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Warning);
                    return this.Redirect("~/Congress/UserPanel/UserPayment");
                }
                var dictionary = new Dictionary<int, decimal>();
                const string selectregistertype = "SelectRegisterType-";
                foreach (var variable in collection.AllKeys.Where(x => x.StartsWith(selectregistertype)))
                {
                    var key = variable.Substring(selectregistertype.Length, variable.Length - selectregistertype.Length);
                    dictionary.Add(key.ToInt(), collection[variable].ToDecimal());
                }
                if (dictionary.Count == 0)
                {
                    ShowMessage(Resources.Congress.PleaseEnterUserPaymentType, Resources.Common.MessaageTitle,
                     messageIcon: MessageIcon.Warning);
                    return this.Redirect("~/Congress/UserPanel/UserPayment");
                }
                var paymnet =
                    CongressComponent.Instance.BaseInfoComponents.UserFacade.Paymnet(
                        SessionParameters.CongressUser, transactionDiscountAttaches, "/Congress/UserPanel/UpdateStatusAfterTransaction?Id=" + user.Id, dictionary);
                if (paymnet)
                {
                    SessionParameters.CongressUser = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(user.Id);
                    if (SessionParameters.CongressUser.TempId.HasValue)
                    {
                        return Redirect("~" + Extentions.PrepaymenyUrl((Guid)SessionParameters.CongressUser.TempId));
                    }
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/UserPanel/Home");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
               messageIcon: MessageIcon.Error);
                return this.Redirect("~/Congress/UserPanel/UserPayment");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return this.Redirect("~/Congress/UserPanel/UserPayment");
            }
        }
        public ActionResult UserChair()
        {
            var enumerable = CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetParents(this.Homa.Id, true);
            ViewBag.CongressHalls = new SelectList(enumerable, "Id", "Name");
            ViewBag.HasHall = enumerable != null && enumerable.Any();
            return View(SessionParameters.CongressUser);
        }
        [CongressUserAuthorize]
        public ActionResult SetUserChair(string chairId)
        {

            try
            {
                var user = SessionParameters.CongressUser;
                if (!string.IsNullOrEmpty(chairId))
                {
                    var value = chairId.Split(',');
                    user.ChairId = string.IsNullOrEmpty(value[0]) ? (Guid?)null : value[0].ToGuid();
                }
                else user.ChairId = null;
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.SelectChair(user))
                {
                    SessionParameters.CongressUser = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(user.Id);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
               messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");

            }
        }

        public ActionResult UpdateStatusAfterTransaction(Guid id)
        {
            try
            {
                var tr = CongressComponent.Instance.BaseInfoComponents.UserFacade.UpdateStatusAfterTransaction(this.Homa.Id, id);
                SessionParameters.CongressUser = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(id);
                return tr != Guid.Empty
                    ? Redirect("~/Payment/Transaction/TransactionResult?Id=" + tr +
                               "&callbackurl=/Congress/UserPanel/Home")
                    : Redirect("~/Congress/UserPanel/Home");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserPanel/Home");
            }
        }

        [HttpPost]
        public ActionResult ModifyUser(FormCollection collection)
        {

            try
            {
                var messageStack = new List<string>();
                var id = collection["Id"].ToGuid();
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                var user = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(id);
                this.RadynTryUpdateModel(user);
                this.RadynTryUpdateModel(user.EnterpriseNode);
                this.RadynTryUpdateModel(user.EnterpriseNode.RealEnterpriseNode);
                if (SessionParameters.CongressUser.Username != user.Username)
                {
                    if (string.IsNullOrEmpty(user.Username))
                        messageStack.Add(Resources.Congress.PleaseInsertUserName);

                }
                if (string.IsNullOrEmpty(user.EnterpriseNode.RealEnterpriseNode.FirstName))
                    messageStack.Add(Resources.Congress.Please_Enter_YourName);
                if (string.IsNullOrEmpty(user.EnterpriseNode.RealEnterpriseNode.LastName))
                    messageStack.Add(Resources.Congress.Please_Enter_YourLastName);
                if (string.IsNullOrEmpty(user.EnterpriseNode.Cellphone))
                    messageStack.Add(Resources.Congress.Please_Enter_YourMobile);
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    messageStack.Add(postFormData.FillErrors);
                }
                var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                if (messageBody != "")
                {
                    ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return Content("false");
                }
                user.Password = String.Empty;
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.Update(user, postFormData, file))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    SessionParameters.CongressUser = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(user.Id);
                    this.ClearFormGeneratorData(postFormData.Id);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        [CongressUserAuthorize]
        public ActionResult Home()
        {
            ViewBag.tempcount = PaymentComponenets.Instance.TransactionFacade.GetUserTempCount(SessionParameters.CongressUser.Id);
            ViewBag.HasFile = CongressComponent.Instance.BaseInfoComponents.UserFileFacade.Any(x => x.CongressId == this.Homa.Id);
            ViewBag.UnreadInboxCount = MessageComponenet.SentInternalMessageInstance.MailBoxFacade.GetUnReadInboxCount(SessionParameters.CongressUser.Id);
            return View();
        }



        [CongressUserAuthorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection collection)
        {
            var newPassword = collection["NewPassword"];
            if (CongressComponent.Instance.BaseInfoComponents.UserFacade.ChangePassword(SessionParameters.CongressUser.Id, newPassword))
            {
                ShowMessage(Resources.Congress.PasswordSuccedChanged, "",
                               new[] { Resources.Common.Ok, " window.location='" + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/UserPanel/Home'; " });
                return Content("true");
            }
            ShowMessage(Resources.Common.No_results_found);
            return Content("false");
        }
        [HttpPost]
        public ActionResult Validate(FormCollection collection)
        {

            var messageStack = new List<string>();
            var newPassword = collection["NewPassword"];
            var newPasswordRepeat = collection["RepeatNewPassword"];
            var oldPassword = collection["OldPassword"];
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(newPasswordRepeat) || string.IsNullOrEmpty(oldPassword))
                messageStack.Add(Resources.Common.Please_complete_Form_Data);
            else
            {
                if (!CongressComponent.Instance.BaseInfoComponents.UserFacade.CheckOldPassword(SessionParameters.CongressUser.Id, oldPassword))
                    messageStack.Add(Resources.Congress.OldPasswordIsWrong);
                if (newPassword != newPasswordRepeat)
                    messageStack.Add(Resources.Congress.Password_and_Repeat_Not_Equal);
            }
            var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
            if (messageBody != "")
            {
                ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                return Content("false");
            }
            return Content("true");

        }
        public ActionResult Logout()
        {
            if (SessionParameters.User != null)
            {
                Session.Remove("CongressUser");
                return Redirect("~/Congress/UserPanel/Login");
            }

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            string[] allDomainCookes = Request.Cookies.AllKeys;
            foreach (string domainCookie in allDomainCookes)
            {
                if (Request.Cookies[domainCookie] == null) continue;
                Request.Cookies[domainCookie].Expires = DateTime.Now.AddDays(-1d);
            }
         
            return Redirect("~/Congress/UserPanel/Login");
        }

        public ActionResult UploadPaymentPhoto(IEnumerable<HttpPostedFileBase> fileBase)
        {
            var file = Request.Files["UploadPaymentPhoto"];
            if (file != null)
            {
                if (file.InputStream != null)
                {

                    Session["ImagePayment"] = file;
                }
            }
            return Content("");
        }
        public ActionResult Subscribe()
        {
            if (SessionParameters.CongressUser != null)
                return Redirect("~/Congress/UserPanel/Home");
            return View();
        }

        [HttpPost]
        [RadynAuthorize(DoAuthorize = false)]

        public ActionResult Subscribe(FormCollection collection)
        {
            try
            {

                var messageStack = new List<string>();

                var service = new CaptchaService();
                if (!service.IsValidCaptcha(collection["captch"]))
                    messageStack.Add(Resources.CommonComponent.Enterthesecuritycodeisnotvalid);

                if (string.IsNullOrEmpty(collection["name"]))
                    messageStack.Add(Resources.Congress.Please_Enter_YourName);
                if (string.IsNullOrEmpty(collection["lastname"]))
                    messageStack.Add(Resources.Congress.Please_Enter_YourLastName);
                if (string.IsNullOrEmpty(collection["mail"]))
                    messageStack.Add(Resources.Congress.PleaseEnterYourEmail);
                else
                {

                    if (!Utility.Utils.IsEmail(collection["mail"]))
                        messageStack.Add(Resources.Congress.UnValid_Enter_Email);
                }
                var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                if (messageBody != "")
                {
                    ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    ViewBag.Name = collection["name"];
                    ViewBag.LastName = collection["lastname"];
                    ViewBag.Email = collection["mail"];
                    return View();
                }
                var config =
                    this.Homa.Configuration;
                if (config.RegisterEmailConfirm)
                {
                    if (Request.UrlReferrer != null)
                    {
                        var status = CongressComponent.Instance.BaseInfoComponents.UserFacade.Register(this.Homa.Id, collection["mail"],
                            collection["name"], collection["lastname"], "http://" + Request.Url.Authority + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/UserPanel/Complete", SessionParameters.Culture);
                        switch (status)
                        {
                            case Enums.SubscribeStatus.None:
                                break;
                            case Enums.SubscribeStatus.MailSent:
                                ShowMessage(
                                    Resources.Congress.MailSentMessage,
                                     Resources.Congress.EmailRegisterInCongress, messageIcon: MessageIcon.Succeed);
                                break;
                            case Enums.SubscribeStatus.NotConfirmed:
                                ShowMessage(
                                   Resources.Congress.NotConfirmedMessage + "<br/><br/>" +
                                   " <div style='float:left;'><a href='javascript:;' onclick=sendMail('" + collection["mail"] +
                                   "')>" + Resources.Congress.Anotherlinkinactivation + "</a></div>",
                                   Resources.Congress.EmailRegisterInCongress, messageIcon: MessageIcon.Warning);
                                break;
                            case Enums.SubscribeStatus.Confirmed:
                                break;
                            case Enums.SubscribeStatus.Registered:
                                ShowMessage(Resources.Congress.YouhavealreadyregisteredCongress, Resources.Congress.EmailRegisterInCongress, messageIcon: MessageIcon.Warning);
                                break;
                            case Enums.SubscribeStatus.Deactived:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    return Redirect("~/Congress/UserPanel/Subscribe");

                }
                var model = CongressComponent.Instance.BaseInfoComponents.UserFacade.GetByEmail(collection["mail"], this.Homa.Id);
                if (model == null)
                {
                    if (CongressComponent.Instance.BaseInfoComponents.UserFacade.RegisterWithOutSendMail(this.Homa.Id, collection["mail"], collection["name"], collection["lastname"]))
                        return Redirect("~/Congress/UserPanel/CompleteWithOutEmail?email=" + collection["mail"]);
                }
                else
                {
                    if (model.Status == (byte)Enums.UserStatus.PreRegister)
                        return Redirect("~/Congress/UserPanel/CompleteWithOutEmail?email=" + collection["mail"]);
                    ShowMessage(Resources.Congress.YouhavealreadyregisteredCongress, Resources.Congress.EmailRegisterInCongress, messageIcon: MessageIcon.Warning);
                    return Redirect("~/Congress/UserPanel/Subscribe");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ShowMessage(ex.Message + " " + ex.InnerException.Message, "", messageIcon: MessageIcon.Security);
                else
                    ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                ViewBag.Message = ex.Message;
            }
            return Redirect("~/Congress/UserPanel/Subscribe");
        }





        public ActionResult SendConfirmLink(string mail)
        {
            var model = CongressComponent.Instance.BaseInfoComponents.UserFacade.GetByEmail(mail,
                 this.Homa.Id);
            if (model == null)
            {
                return Content("false");
            }
            if (Request.UrlReferrer != null && CongressComponent.Instance.BaseInfoComponents.UserFacade.SendConfirmLink(mail, "http://" + Request.Url.Authority + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/UserPanel/Complete", this.Homa.Id, model.FirstNameAndLastName))
            {
                ShowMessage(Resources.Congress.MailSentMessage,
                                  Resources.Congress.EmailRegisterInCongress, messageIcon: MessageIcon.Succeed);
            }
            return Content("");
        }
        public ActionResult CompleteWithOutEmail(string email)

        {
            return PartialView("PVCompleteWithOutEmail");
        }

        public ActionResult CompleteWithEmail(string email)

        {
            return PartialView("PVCompleteWithEmail");
        }
        public ActionResult InsertWithOutEmail(FormCollection collection)
        {
            try
            {
                var userFacade = CongressComponent.Instance.BaseInfoComponents.UserFacade;
                var messageStack = new List<string>();
                var user = new User
                {
                    EnterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode
                    {
                        RealEnterpriseNode = new RealEnterpriseNode()
                    }
                };
                this.RadynTryUpdateModel(user);
                this.RadynTryUpdateModel(user.EnterpriseNode);
                this.RadynTryUpdateModel(user.EnterpriseNode.RealEnterpriseNode);
                user.CongressId = this.Homa.Id;
                user.Status = (byte)Enums.UserStatus.Register;
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];

                }
                var service = new CaptchaService();
                if (!service.IsValidCaptcha(collection["captch"]))
                    messageStack.Add(Resources.CommonComponent.Enterthesecuritycodeisnotvalid);
                if (string.IsNullOrEmpty(collection["Password"]))
                    messageStack.Add(Resources.Congress.Please_Enter_Password);
                if (string.IsNullOrEmpty(collection["RepeatPassword"]))
                    messageStack.Add(Resources.Congress.Please_Enter_Password_Repeat);
                if (user.Password != null && !string.IsNullOrEmpty(user.Password) && user.Password.Length < 6)
                    messageStack.Add(Resources.Congress.MinimumPasswordCharacter);
                if (string.IsNullOrEmpty(user.EnterpriseNode.RealEnterpriseNode.FirstName))
                    messageStack.Add(Resources.Congress.Please_Enter_YourName);
                if (string.IsNullOrEmpty(user.EnterpriseNode.RealEnterpriseNode.LastName))
                    messageStack.Add(Resources.Congress.Please_Enter_YourLastName);
                if (string.IsNullOrEmpty(user.EnterpriseNode.Cellphone))
                    messageStack.Add(Resources.Congress.Please_Enter_YourMobile);
                else if (!string.IsNullOrEmpty(user.EnterpriseNode.Cellphone) && ((!user.EnterpriseNode.Cellphone.StartsWith("09") && !user.EnterpriseNode.Cellphone.StartsWith("+") && !user.EnterpriseNode.Cellphone.StartsWith("00")) || ((user.EnterpriseNode.Cellphone.Length < 11) && (user.EnterpriseNode.Cellphone.Length > 15)) || user.EnterpriseNode.Cellphone.ToLong() == 0))
                    messageStack.Add(Resources.Congress.MobileNumberIsNotValid);
                if (user.EnterpriseNode.RealEnterpriseNode.Gender == null)
                    messageStack.Add(Resources.Congress.Please_Enter_YourGender);
                if (collection["Password"] != collection["RepeatPassword"])
                    messageStack.Add(Resources.Congress.Password_and_Repeat_Not_Equal);
                var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                if (messageBody != "")
                {
                    ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return Content("false");
                }

                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Warning);
                    this.ClearFormGeneratorData(postFormData.Id);
                    return Content("false");
                }
                if (userFacade.Insert(user, postFormData, file))
                {
                    Session.Remove("Image");
                    var login = userFacade.Login(user.Username, collection["Password"], this.Homa.Id);
                    if (login != null)
                    {
                        FormsAuthentication.SetAuthCookie(collection["UserName"], false);
                        SessionParameters.CongressUser = login;
                        ShowMessage(Resources.Congress.CompleteRegisterMessage, "",
                            new[] { Resources.Common.Ok, " window.location='" + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/UserPanel/Home'; " });
                        return Content("true");

                    }
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                ViewBag.Message = ex.Message;
                return Content("false");
            }

        }

        public ActionResult InsertWithEmail(FormCollection collection)
        {
            try
            {

                var userFacade = CongressComponent.Instance.BaseInfoComponents.UserFacade;
                var messageStack = new List<string>();
                var user = new User
                {
                    EnterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode
                    {
                        RealEnterpriseNode = new RealEnterpriseNode()
                    }
                };
                this.RadynTryUpdateModel(user);
                this.RadynTryUpdateModel(user.EnterpriseNode);
                this.RadynTryUpdateModel(user.EnterpriseNode.RealEnterpriseNode);
                user.CongressId = this.Homa.Id;
                user.Status = (byte)Enums.UserStatus.Register;
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                    file = (HttpPostedFileBase)Session["Image"];
                var service = new CaptchaService();
                if (!service.IsValidCaptcha(collection["captch"]))
                    messageStack.Add(Resources.CommonComponent.Enterthesecuritycodeisnotvalid);
                if (string.IsNullOrEmpty(collection["Password"]))
                    messageStack.Add(Resources.Congress.Please_Enter_Password);
                if (string.IsNullOrEmpty(collection["RepeatPassword"]))
                    messageStack.Add(Resources.Congress.Please_Enter_Password_Repeat);
                if (user.Password != null && !string.IsNullOrEmpty(user.Password) && user.Password.Length < 6)
                    messageStack.Add(Resources.Congress.MinimumPasswordCharacter);
                if (string.IsNullOrEmpty(user.EnterpriseNode.RealEnterpriseNode.FirstName))
                    messageStack.Add(Resources.Congress.Please_Enter_YourName);
                if (string.IsNullOrEmpty(user.EnterpriseNode.RealEnterpriseNode.LastName))
                    messageStack.Add(Resources.Congress.Please_Enter_YourLastName);
                if (string.IsNullOrEmpty(user.EnterpriseNode.Cellphone))
                    messageStack.Add(Resources.Congress.Please_Enter_YourMobile);
                else if (!string.IsNullOrEmpty(user.EnterpriseNode.Cellphone) && ((!user.EnterpriseNode.Cellphone.StartsWith("09") && !user.EnterpriseNode.Cellphone.StartsWith("+") && !user.EnterpriseNode.Cellphone.StartsWith("00")) || ((user.EnterpriseNode.Cellphone.Length < 11) && (user.EnterpriseNode.Cellphone.Length > 15)) || user.EnterpriseNode.Cellphone.ToLong() == 0))
                    messageStack.Add(Resources.Congress.MobileNumberIsNotValid);
                if (user.EnterpriseNode.RealEnterpriseNode.Gender == null)
                    messageStack.Add(Resources.Congress.Please_Enter_YourGender);
                if (collection["Password"] != collection["RepeatPassword"])
                    messageStack.Add(Resources.Congress.Password_and_Repeat_Not_Equal);
                if (string.IsNullOrEmpty(collection["Email"]))
                    messageStack.Add(Resources.Congress.PleaseEnterYourEmail);
                else
                {

                    if (!Utility.Utils.IsEmail(collection["Email"]))
                        messageStack.Add(Resources.Congress.UnValid_Enter_Email);
                }


                var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                if (messageBody != "")
                {
                    ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return Content("false");
                }

                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Warning);
                    this.ClearFormGeneratorData(postFormData.Id);
                    return Content("false");
                }
                if (userFacade.Insert(user, postFormData, file))
                {
                    Session.Remove("Image");
                    var login = userFacade.Login(user.Username, collection["Password"], this.Homa.Id);
                    if (login != null)
                    {
                        FormsAuthentication.SetAuthCookie(collection["UserName"], false);
                        SessionParameters.CongressUser = login;
                        ShowMessage(Resources.Congress.CompleteRegisterMessage, "",
                            new[] { Resources.Common.Ok, " window.location='" + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/UserPanel/Home'; " });
                        return Content("true");

                    }
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                ViewBag.Message = ex.Message;
                return Content("false");
            }

        }
        public ActionResult Complete(Guid Id)
        {

            var model = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(Id);

            if (model == null)
            {
                ShowMessage(Resources.Congress.YourUserNameDeletedPleaseRegisterAgain, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                return Redirect("~/Congress/UserPanel/Subscribe");
            }
            if (!string.IsNullOrEmpty(model.Culture))
            {
                SessionParameters.Culture = model.Culture;
            }

            if (model.Status != (byte)Enums.UserStatus.PreRegister)
                return Redirect("~/Congress/UserPanel/Login");
            ViewBag.Id = model.Id;
            return View();
        }

        [HttpPost]
        public ActionResult Complete(FormCollection collection)
        {
            try
            {
                var userFacade = CongressComponent.Instance.BaseInfoComponents.UserFacade;
                var messageStack = new List<string>();
                var user = userFacade.Get(collection["Id"].ToGuid());
                this.RadynTryUpdateModel(user);
                if (user != null)
                {

                    this.RadynTryUpdateModel(user.EnterpriseNode);
                    this.RadynTryUpdateModel(user.EnterpriseNode.RealEnterpriseNode);
                    HttpPostedFileBase file = null;
                    if (Session["Image"] != null)
                        file = (HttpPostedFileBase)Session["Image"];

                    var service = new CaptchaService();
                    if (!service.IsValidCaptcha(collection["captch"]))
                        messageStack.Add(Resources.CommonComponent.Enterthesecuritycodeisnotvalid);
                    if (string.IsNullOrEmpty(collection["Password"]))
                        messageStack.Add(Resources.Congress.Please_Enter_Password);
                    if (string.IsNullOrEmpty(collection["RepeatPassword"]))
                        messageStack.Add(Resources.Congress.Please_Enter_Password_Repeat);
                    if (user.Password != null && !string.IsNullOrEmpty(user.Password) && user.Password.Length < 6)
                        messageStack.Add(Resources.Congress.MinimumPasswordCharacter);
                    if (string.IsNullOrEmpty(user.EnterpriseNode.RealEnterpriseNode.FirstName))
                        messageStack.Add(Resources.Congress.Please_Enter_YourName);
                    if (string.IsNullOrEmpty(user.EnterpriseNode.RealEnterpriseNode.LastName))
                        messageStack.Add(Resources.Congress.Please_Enter_YourLastName);
                    if (string.IsNullOrEmpty(user.EnterpriseNode.Cellphone))
                        messageStack.Add(Resources.Congress.Please_Enter_YourMobile);
                    else if (!string.IsNullOrEmpty(user.EnterpriseNode.Cellphone) && ((!user.EnterpriseNode.Cellphone.StartsWith("09") && !user.EnterpriseNode.Cellphone.StartsWith("+") && !user.EnterpriseNode.Cellphone.StartsWith("00")) || ((user.EnterpriseNode.Cellphone.Length < 11) && (user.EnterpriseNode.Cellphone.Length > 15)) || user.EnterpriseNode.Cellphone.ToLong() == 0))
                        messageStack.Add(Resources.Congress.MobileNumberIsNotValid);
                    if (user.EnterpriseNode.RealEnterpriseNode.Gender == null)
                        messageStack.Add(Resources.Congress.Please_Enter_YourGender);
                    if (collection["Password"] != collection["RepeatPassword"])
                        messageStack.Add(Resources.Congress.Password_and_Repeat_Not_Equal);
                    var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                    if (messageBody != "")
                    {
                        ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                        return Content("false");
                    }

                    var postFormData = this.PostForFormGenerator(collection);
                    if (!string.IsNullOrEmpty(postFormData.FillErrors))
                    {
                        ShowMessage(postFormData.FillErrors, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Warning);
                        this.ClearFormGeneratorData(postFormData.Id);
                        return Content("false");
                    }
                    if (userFacade.CompleteRegister(user, postFormData, file))
                    {
                        Session.Remove("Image");
                        var login = userFacade.Login(user.Username, collection["Password"], this.Homa.Id);
                        if (login != null)
                        {
                            FormsAuthentication.SetAuthCookie(collection["UserName"], false);
                            SessionParameters.CongressUser = login;
                            ShowMessage(Resources.Congress.CompleteRegisterMessage, "",
                                new[] { Resources.Common.Ok, " window.location='" + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/UserPanel/Home'; " });
                            return Content("true");


                        }


                    }
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                ViewBag.Message = ex.Message;
                return Content("false");
            }

        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [RadynAuthorize(DoAuthorize = false)]
        public ActionResult ForgotPassword(FormCollection collection)
        {
            try
            {
                if (!Request.Url.Authority.Contains("localhost"))
                {
                    var service = new CaptchaService();
                    if (!service.IsValidCaptcha(collection["captch"]))
                        throw new Exception(Resources.CommonComponent.Enterthesecuritycodeisnotvalid);
                }
                var userName = collection["username"];
                if (Request.UrlReferrer != null && CongressComponent.Instance.BaseInfoComponents.UserFacade.ForgotPassword(userName, "http://" + Request.UrlReferrer.Authority + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/UserPanel/NewPassword", this.Homa.Id))
                {
                    ShowMessage(Resources.Security.Your_New_Password_Sent_Email);
                }
                ViewBag.Username = userName;
                return View();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                ViewBag.Message = ex.Message;
            }
            return View();
        }
        public ActionResult NewPassword(Guid Id)
        {

            var user = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(Id);
            if (user == null)
            {
                ShowMessage(Resources.Congress.YourUserNameDeletedPleaseRegisterAgain, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                return Redirect("~/Congress/UserPanel/Subscribe");
            }
            return View(user);
        }

        [HttpPost]
        [RadynAuthorize(DoAuthorize = false)]
        // [ValidateAntiForgeryToken(Salt = "radynAntiFor")]
        public ActionResult NewPassword(Guid Id, FormCollection collection)
        {
            User user = null;
            try
            {
                if (!Request.Url.Authority.Contains("localhost"))
                {
                    var service = new CaptchaService();
                    if (!service.IsValidCaptcha(collection["captch"]))
                        throw new Exception(Resources.CommonComponent.Enterthesecuritycodeisnotvalid);
                }

                user = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(Id);
                if (user == null)
                {
                    ShowMessage(Resources.Congress.YourUserNameDeletedPleaseRegisterAgain, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return Redirect("~/Congress/UserPanel/Subscribe");
                }
                var messageStack = new List<string>();
                if (string.IsNullOrEmpty(collection["Password"]))
                    messageStack.Add(Resources.Congress.Please_Enter_Password);
                if (string.IsNullOrEmpty(collection["RepeatPassword"]))
                    messageStack.Add(Resources.Congress.Please_Enter_Password_Repeat);
                else if (collection["Password"] != collection["RepeatPassword"])
                    messageStack.Add(Resources.Congress.Password_and_Repeat_Not_Equal);
                var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                if (messageBody != "")
                {
                    ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return View(user);
                }
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.ChangePassword(user.Id, collection["password"]))
                {
                    ShowMessage(Resources.Congress.PasswordSuccedChanged, messageIcon: MessageIcon.Succeed);
                    var role = CongressComponent.Instance.BaseInfoComponents.UserFacade.Login(user.Username, collection["password"], this.Homa.Id);
                    if (role != null)
                    {

                        FormsAuthentication.SetAuthCookie(user.Username, false);
                        SessionParameters.CongressUser = role;
                        return Redirect("~/Congress/UserPanel/Home");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                ViewBag.Message = ex.Message;
            }
            return View(user);
        }
        public ActionResult UserAttendanceList(Guid? userId, bool isdirty = false)
        {
            IEnumerable<User> list;
            if (userId.HasValue)
            {
                if (userId == Guid.Empty)
                    list = null;
                else
                {
                    var user = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get((Guid)userId);
                    list = user != null ? new List<User> { user } : null;
                }

            }
            else
                list = CongressComponent.Instance.BaseInfoComponents.UserFacade.Where(x =>
               x.CongressId == this.Homa.Id && x.Status == (byte)Enums.UserStatus.ConfirmPresentInHoma);
            ViewBag.isdirty = isdirty;
            return PartialView("PartialViewAttendanceUser", list);
        }
        [RadynAuthorize]
        public ActionResult UserAttendance()
        {

            return View();
        }

        [HttpPost]
        public ActionResult UserAttendance(FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["Number"]))
                {
                    ShowMessage(Resources.Congress.PleaseEnterUserId, Resources.Common.MessaageTitle, new[] { Resources.Common.Ok, "$(\"#Number\").focus();" },
                           messageIcon: MessageIcon.Warning);

                    return View();
                }

                var userAttendance = CongressComponent.Instance.BaseInfoComponents.UserFacade.UserAttendance(this.Homa.Id, collection["Number"].ToLong());
                if (userAttendance != null)
                {
                    ViewBag.UserId = userAttendance.Id;
                    ViewBag.Isdirty = userAttendance.State != Framework.ObjectState.New;

                }
                return View();
            }
            catch
            {
                ViewBag.UserId = Guid.Empty;
                return View();
            }
        }



        public ActionResult UserForm()

        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.UserFormsFacade.SelectKeyValuePair(
                    c => c.FormStructure.Id, c => c.FormStructure.Name, x => x.FormStructure.Enable);
            ViewBag.FormList = new SelectList(list, "Key", "Value");
            return View();
        }

        [HttpPost]
        public ActionResult UserForm(FormCollection collection)
        {
            try
            {
                var list =
                    CongressComponent.Instance.BaseInfoComponents.UserFormsFacade.SelectKeyValuePair(
                        c => c.FormStructure.Id, c => c.FormStructure.Name);

                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Warning);
                    this.ClearFormGeneratorData(postFormData.Id);
                    return View();
                }
                postFormData.RefId = SessionParameters.CongressUser.Id.ToString();
                if (FormGeneratorComponent.Instance.FormDataFacade.ModifyFormData(postFormData))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    ViewBag.FormList = new SelectList(list, "Key", "Value", postFormData.Id);
                    return View();
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                ViewBag.FormList = new SelectList(list, "Key", "Value", postFormData.Id);
                return View();

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                var list =
                    CongressComponent.Instance.BaseInfoComponents.UserFormsFacade.SelectKeyValuePair(
                        c => c.FormStructure.Id, c => c.FormStructure.Name);
                ViewBag.FormList = new SelectList(list, "Key", "Value");
                return View();
            }
        }

        public ActionResult GetUserFormInfo(Guid id, string refId, FormState formState = FormState.DataEntryMode)
        {
            var user = CongressComponent.Instance.BaseInfoComponents.UserFormsFacade.Get(this.Homa.Id, id);
            ViewBag.RefId = refId;
            ViewBag.formState = formState;
            return PartialView("PVUserFormInfo", user);
        }
    }
}
