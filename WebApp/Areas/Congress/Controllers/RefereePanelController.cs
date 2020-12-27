using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Resources;
using Radyn.Congress.Tools;
using Radyn.FileManager;
using Radyn.FormGenerator;
using Radyn.FormGenerator.DataStructure;
using Radyn.Security.Tools;
using Radyn.Web.Html;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security;
using Radyn.Utility;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.Web.Mvc.UI.Captcha;
using Radyn.WebApp.Areas.Congress.Tools;
using Radyn.WebApp.Areas.FormGenerator.Tools;


namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class RefereePanelController : CongressBaseController
    {
        [CongressRefereeAuthorize]
        public ActionResult RefereeCartablIndex()

        {
            var refereeId = SessionParameters.CongressReferee.Id;
            var list = CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.OrderByDescending(x => x.InsertDate,
                    x => x.RefereeId == refereeId && x.IsActive);
            GetValues();

            return View(list);

        }

        private void GetValues()
        {
            ViewBag.status =
               EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.FinalState>().Select(
                   keyValuePair =>
                       new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.FinalState>(),
                           keyValuePair.Value));
            var value = this.Homa.Configuration;
            var configurationContent = value.GetConfigurationContent();
            if (configurationContent != null)
            {
                if (configurationContent.AttachRefereeFileId.HasValue)
                    ViewBag.File = configurationContent.AttachRefereeFile;
            }
            ViewBag.HasRefereeopinion = HasopinionForm();

        }

        private static bool HasopinionForm()
        {
            var byUrl = FormGeneratorComponent.Instance.FormAssigmentFacade.GetByUrl(
                AppExtention.CongressMoudelName + "-/Congress/RefereePanel/Refereeopinion");
            return byUrl != null;
        }

        public ActionResult ViewRefereeopinion(Guid refereeId, Guid articleId)
        {
            var refereeCartable = CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.FirstOrDefault(x => x.RefereeId == SessionParameters.CongressReferee.Id && x.ArticleId == articleId);

            return View(refereeCartable);
        }
        [CongressRefereeAuthorize]
        public ActionResult Refereeopinion(Guid articleId)
        {

            var refereeCartable = CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.FirstOrDefault(x => x.RefereeId == SessionParameters.CongressReferee.Id && x.ArticleId == articleId);
            return View(refereeCartable);
        }
        [HttpPost]
        public ActionResult Refereeopinion(FormCollection collection)
        {
            try
            {
                var articleId = collection["articleId"].ToGuid();
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Warning);
                    return Json(new { Result = false, Score = "" });
                }
                var refereeCartable = CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.FirstOrDefault(x => x.RefereeId == SessionParameters.CongressReferee.Id && x.ArticleId == articleId);
                if (CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.Refereeopinion(refereeCartable, postFormData))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                               messageIcon: MessageIcon.Succeed);
                    this.ClearFormGeneratorData(postFormData.Id);
                    return Json(new { Result = true, Score = refereeCartable.Score });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Json(new { Result = false, Score = "" });
            }
            catch (Exception exception)
            {

                ShowExceptionMessage(exception);
                return Json(new { Result = false, Score = "" });

            }
        }
        [CongressRefereeAuthorize]
        [HttpPost]
        public ActionResult RefereeCartablIndex(FormCollection collection)
        {

            var refereeId = SessionParameters.CongressReferee.Id;
            var state = 0;
            var list = new List<RefereeCartable>();
            if (!string.IsNullOrEmpty(collection["StateId"]))
            {
                state = collection["StateId"].ToByte();
                list =
                    CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.OrderBy(x => x.InsertDate,
                        x => x.RefereeId == refereeId && x.Status == state && x.IsActive);
            }
            else
            {
                list = CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.OrderBy(x => x.InsertDate,
                        x => x.RefereeId == refereeId && x.IsActive);
            }



            GetValues();
            ViewBag.SelectedStatus = string.IsNullOrEmpty(collection["StateId"]) ? (byte?)null : collection["StateId"].ToByte();
            return View(list);

        }


        public ActionResult Login()
        {

            if (this.Homa.Status != Enums.CongressStatus.NoProblem)
                return Redirect("/Home/Index");
            if (SessionParameters.CongressReferee != null)
                return Redirect("~/Congress/RefereePanel/RefereeCartablIndex");

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
                if (!WebUtility.IsLocal() && SessionParameters.HasLoginPasswordError)
                {
                    var service = new CaptchaService();
                    if (!service.IsValidCaptcha(model.Captch))
                        throw new Exception(Resources.CommonComponent.Enterthesecuritycodeisnotvalid);

                }
                var userName = model.Username;
                var password = model.Password;
                var referee = await CongressComponent.Instance.BaseInfoComponents.RefereeFacade.LoginAsync(userName, password, this.Homa.Id);
                if (referee == null)
                    throw new Exception(Resources.Security.Please_enter_the_correct_values);
                if (!referee.Enabled)
                    throw new Exception("اطلاعات کابری شما غیر فعال است");
                SessionParameters.HasLoginPasswordError = false;
                SessionParameters.CongressReferee = referee;
                return !string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                    ? this.RadynRedirect(Request.QueryString["returnUrl"])
                    : this.Redirect("~/Congress/RefereePanel/RefereeCartablIndex");

                

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


        [CongressRefereeAuthorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection collection)
        {
            var newPassword = collection["NewPassword"];
            if (CongressComponent.Instance.BaseInfoComponents.RefereeFacade.ChangePassword(SessionParameters.CongressReferee.Id, newPassword))
            {
                ShowMessage(Resources.Congress.PasswordSuccedChanged, "",
                               new[] { Resources.Common.Ok, " window.location='" + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/RefereePanel/RefereeCartablIndex'; " });
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
                if (!CongressComponent.Instance.BaseInfoComponents.RefereeFacade.CheckOldPassword(SessionParameters.CongressReferee.Id, oldPassword))
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
                Session.Remove("CongressReferee");
                return Redirect("~/Congress/RefereePanel/Login");
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
            return Redirect("~/Congress/RefereePanel/Login");
        }

        public ActionResult GetArticleReferees(Guid articleId)
        {
            var referees = CongressComponent.Instance.BaseInfoComponents.RefereeFacade.GetAllForArticle(this.Homa.Id, articleId);
            ViewBag.HasValue = this.Homa.Configuration.SentArticleSpecialReferee && SessionParameters.CongressReferee.IsSpecial;
            ViewBag.articleId = articleId;
            return PartialView("PVArticleReferees", referees);
        }

        [CongressRefereeAuthorize]
        public ActionResult RefereeCartabl(Guid refreeId, Guid articleId)
        {
            var cartable = CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.Where(x => x.RefereeId == refreeId && x.ArticleId == articleId).OrderByDescending(x => x.InsertDate).FirstOrDefault();
            if (!CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.ModifyCartable(articleId, refreeId, cartable.Status, true, true)) return View();
            ViewBag.status = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.FinalStateSpecialRefree>();
            if (SessionParameters.CongressReferee.IsSpecial &&
                this.Homa.Configuration.SentArticleSpecialReferee)
                ViewBag.status = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.FinalStateSpecialRefree>();
            else
            {
                List<KeyValuePair<string, string>> keyValuePairs;
                var status = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.FinalStateReferee>();


                if (!string.IsNullOrEmpty(cartable.Article.ArticleOrginalText) || cartable.Article.OrginalFileId != null)
                    keyValuePairs = status.Where(c => c.Key != Enums.FinalStateReferee.DenialAbstarct.ToString()).ToList();
                else
                {

                    if (string.IsNullOrEmpty(cartable.Article.Abstract) && cartable.Article.AbstractFileId == null)
                        keyValuePairs = status.Where(c =>
                            c.Key != Enums.FinalStateReferee.DenialAbstarct.ToString() &&
                            c.Key != Enums.FinalStateReferee.ConfirmandEdit.ToString() &&
                            c.Key != Enums.FinalStateReferee.Confirm.ToString() &&
                            c.Key != Enums.FinalStateReferee.Denial.ToString()).ToList();
                    else
                        keyValuePairs = status.Where(c =>
                            c.Key != Enums.FinalStateReferee.Confirm.ToString() &&
                            c.Key != Enums.FinalStateReferee.Denial.ToString()).ToList();

                }



                ViewBag.status = keyValuePairs;
            }

            ViewBag.HasRefereeopinion = HasopinionForm();
            return View(cartable);
        }
        [CongressRefereeAuthorize]
        public ActionResult AssignToReferee(FormCollection collection)
        {

            try
            {
                var refereeId = collection["refereeId"].ToGuid();
                var articleId = collection["articleId"].ToGuid();
                var refereeCartable = CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.FirstOrDefault(x => x.RefereeId == refereeId && x.ArticleId == articleId && x.IsActive);
                this.RadynTryUpdateModel(refereeCartable, collection);
                refereeCartable.Status = (byte)collection["State"].ToEnum<Enums.FinalState>();
                var value = collection["Comments"];
                HttpPostedFileBase attachment = null;
                if (Session["FlowAttachment"] != null)
                {
                    attachment = (HttpPostedFileBase)Session["FlowAttachment"];
                    Session.Remove("FlowAttachment");
                }
                var refereesId = new List<Guid>();
                var enumerable = collection.AllKeys.FirstOrDefault(s => s.Equals("SelectedReferee"));
                if (enumerable != null)
                {
                    var strings = collection[enumerable].Split(',');
                    foreach (var variable in strings)
                    {
                        refereesId.Add(variable.ToGuid());
                    }
                }
                if (CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.SpecialRefereeAssignArticle(this.Homa.Id, refereeCartable, SessionParameters.CongressReferee.Id, value, attachment, refereesId))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
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


        [CongressRefereeAuthorize]

        public ActionResult AnswerArticle(FormCollection collection)
        {
            try
            {
                var refereeId = collection["refereeId"].ToGuid();
                var articleId = collection["articleId"].ToGuid();
                var refereeCartable = CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.FirstOrDefault(x => x.RefereeId == refereeId && x.ArticleId == articleId && x.IsActive);
                this.RadynTryUpdateModel(refereeCartable, collection);
                refereeCartable.Status = (byte)collection["State"].ToEnum<Enums.FinalState>();
                var value = collection["Comments"];
                HttpPostedFileBase attachment = null;
                if (Session["FlowAttachment"] != null)
                {
                    attachment = (HttpPostedFileBase)Session["FlowAttachment"];
                    Session.Remove("FlowAttachment");
                }
                if (CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.AnswerArticle(this.Homa.Id, refereeCartable, SessionParameters.CongressReferee.Id, value, attachment))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                               messageIcon: MessageIcon.Succeed);
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



    }
}
