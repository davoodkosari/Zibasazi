using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Radyn.Chat;
using Radyn.Congress;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.Security;
using Radyn.Security.Facade;
using Radyn.Security.Tools;
using Radyn.Utility;
using Radyn.Web.Html;
using Radyn.Web.Mvc.UI.Captcha;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using User = Radyn.Security.DataStructure.User;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class SecurityUserController : CongressBaseController
    {


        [RadynAuthorize]
        public ActionResult Index()
        {
            var users = CongressComponent.Instance.BaseInfoComponents.SecurityUserFacade.Select(x => x.User, x => x.CongressId == Homa.Id);

            return View(users);
        }

        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            ViewBag.Id = Id;
            ViewBag.CongressList =
                CongressComponent.Instance.BaseInfoComponents.SecurityUserFacade.GetUserCongressList(Id);
            return View();
        }


        [RadynAuthorize]
        public ActionResult Create()
        {
            ViewBag.CongressList =
             CongressComponent.Instance.BaseInfoComponents.SecurityUserFacade.GetUserCongressList(null);
            return View(new User());
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {

            try
            {
                var user = new User()
                {
                    EnterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode
                    {
                        RealEnterpriseNode = new RealEnterpriseNode()
                    }
                };


                this.RadynTryUpdateModel(user);
                this.RadynTryUpdateModel(user.EnterpriseNode);
                this.RadynTryUpdateModel(user.EnterpriseNode.RealEnterpriseNode);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }

                var list = new List<Guid>();
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("SelectedCongress"));
                if (!string.IsNullOrEmpty(firstOrDefault))
                {
                    var strings = collection[firstOrDefault].Split(',');
                    foreach (var s in strings)
                    {
                        if (string.IsNullOrEmpty(s)) continue;
                        list.Add(s.ToGuid());
                    }
                }
                if (CongressComponent.Instance.BaseInfoComponents.SecurityUserFacade.Insert(user, file, list))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, new[] { Resources.Common.Ok, " window.location='" + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/SecurityUser/Index';" }, messageIcon: MessageIcon.Succeed);
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

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            ViewBag.CongressList =
            CongressComponent.Instance.BaseInfoComponents.SecurityUserFacade.GetUserCongressList(Id);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {

            var Id = collection["Id"].ToGuid();
            var user = SecurityComponent.Instance.UserFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(user);
                this.RadynTryUpdateModel(user.EnterpriseNode);
                this.RadynTryUpdateModel(user.EnterpriseNode.RealEnterpriseNode);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                var list = new List<Guid>();
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("SelectedCongress"));
                if (!string.IsNullOrEmpty(firstOrDefault))
                {
                    var strings = collection[firstOrDefault].Split(',');
                    foreach (var s in strings)
                    {
                        if (string.IsNullOrEmpty(s)) continue;
                        list.Add(s.ToGuid());
                    }
                }
                if (CongressComponent.Instance.BaseInfoComponents.SecurityUserFacade.Update(user, file, list))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, new[] { Resources.Common.Ok, "window.location='" + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/SecurityUser/Index';" }, messageIcon: MessageIcon.Succeed);
                    return Content("true");

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            ViewBag.Id = Id;
            ViewBag.CongressList =
            CongressComponent.Instance.BaseInfoComponents.SecurityUserFacade.GetUserCongressList(Id);
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.SecurityUserFacade.DeleteByUserId(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                ViewBag.CongressList =
            CongressComponent.Instance.BaseInfoComponents.SecurityUserFacade.GetUserCongressList(Id);
                return View();
            }
        }


        [HttpPost]
        public ActionResult Validate(FormCollection collection)
        {

            var messageStack = new List<string>();
            var Id = collection["Id"].ToGuid();
            var user = new User();
            var enterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode
            {
                RealEnterpriseNode = new RealEnterpriseNode()
            };
            this.RadynTryUpdateModel(user);
            this.RadynTryUpdateModel(enterpriseNode);
            this.RadynTryUpdateModel(enterpriseNode.RealEnterpriseNode);
            if (string.IsNullOrEmpty(user.Username))
                messageStack.Add(Resources.Congress.PleaseInsertUserName);
            if (Id == Guid.Empty)
            {
                if (string.IsNullOrEmpty(user.Password))
                    messageStack.Add(Resources.Congress.Please_Enter_Password);
                if (string.IsNullOrEmpty(collection["RepeatPassword"]))
                    messageStack.Add(Resources.Congress.Please_Enter_Password_Repeat);
                else if (!string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(collection["RepeatPassword"]))
                    if (user.Password != collection["RepeatPassword"])
                        messageStack.Add(Resources.Congress.Password_and_Repeat_Not_Equal);
            }
            else
            {
                if (!string.IsNullOrEmpty(user.Password))
                {
                    if (string.IsNullOrEmpty(collection["RepeatPassword"]))
                        messageStack.Add(Resources.Congress.Please_Enter_Password_Repeat);
                    else if (!string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(collection["RepeatPassword"]))
                        if (user.Password != collection["RepeatPassword"])
                            messageStack.Add(Resources.Congress.Password_and_Repeat_Not_Equal);
                }
            }
            if (user.Password != null && !string.IsNullOrEmpty(user.Password) && user.Password.Length < 6)
                messageStack.Add(Resources.Congress.MinimumPasswordCharacter);

            var list = new List<Guid>();
            var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("SelectedCongress"));
            if (!string.IsNullOrEmpty(firstOrDefault))
            {
                var strings = collection[firstOrDefault].Split(',');
                foreach (var s in strings)
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    list.Add(s.ToGuid());
                }
            }
            if (!list.Any())
                messageStack.Add("لطفا همایشی را انتخاب کنید");

            var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
            if (messageBody != "")
            {
                ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                return Content("false");
            }
            return Content("true");

        }


        public ActionResult Login()
        {


            return View();
        }

        [HttpPost]
        [RadynAuthorize(DoAuthorize = false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("نام کاربری یا رمز عبور را وارد کنید");
                if (!WebUtility.IsLocal() && SessionParameters.HasLoginPasswordError)
                {
                    var service = new CaptchaService();
                    if (!service.IsValidCaptcha(model.Captch))
                        throw new Exception(Resources.CommonComponent.Enterthesecuritycodeisnotvalid);
                }
                var userName = model.Username;
                var password = model.Password;
                if (FormsAuthentication.Authenticate(userName, StringUtils.Encrypt(password)))
                {

                    FormsAuthentication.SetAuthCookie(userName, false);
                    switch (userName.ToLower())
                    {
                        case "host":
                            SessionParameters.UserType = UserType.Host;
                            SessionParameters.UserOperation = await SecurityComponent.Instance.OperationFacade.GetAllAsync();
                            break;

                    }
                    SessionParameters.Culture = "fa-IR";
                    SessionParameters.HasLoginPasswordError = false;
                    return !string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                        ? this.Redirect(Request.QueryString["returnUrl"])
                        : this.Redirect("~/Account/Index");
                }
                if (!await CongressComponent.Instance.BaseInfoComponents.HomaFacade.HasLicenseAsync())
                    throw new Exception(Radyn.Congress.Resources.Congress.YouCanNotAddNewCongress);
                var user = await CongressComponent.Instance.BaseInfoComponents.SecurityUserFacade.LoginAsync(userName, password, this.Homa.Id);
                if (user == null)
                    throw new Exception(Resources.Security.Please_enter_the_correct_values);
               
                FormsAuthentication.SetAuthCookie(user.Username, false);
                SessionParameters.User = user;
                SessionParameters.UserOperation =
                    await SecurityComponent.Instance.OperationFacade.GetAllByUserIdAsync(user.Id);
                SessionParameters.UserType = UserType.User;
                ChatComponent.Instance.ChatManager.AddUser(user);
                SessionParameters.HasLoginPasswordError = false;
                SessionParameters.Culture = "fa-IR";
                return !string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                    ? this.Redirect(Request.QueryString["returnUrl"])
                    : this.Redirect("~/Account/Index");



            }
            catch (Exception ex)
            {
                SessionParameters.HasLoginPasswordError = true;
                ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                ViewBag.Message = ex.Message;
                model = new LoginViewModel() { Username = model.Username };
                return View(model);
            }

        }





    }
}