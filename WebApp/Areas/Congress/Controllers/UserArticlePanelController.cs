using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
using Enums = Radyn.Congress.Tools.Enums;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class UserArticlePanelController : CongressBaseController
    {
        [CongressUserAuthorize]
        public ActionResult IndexArticle()
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.ArticleFacade.GetByUserId(this.Homa.Configuration, SessionParameters.CongressUser.Id);
           
            ViewBag.config = this.Homa.Configuration;
            Session.Remove("ArticleAuthors");
            return View(list);
        }



        [CongressUserAuthorize]
        public ActionResult SentArticle(Guid Id)
        {
            var article =
                CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(Id);
            if (article.Status == (byte)Enums.ArticleState.Denial)
            {
                ShowMessage(string.Format(Resources.Congress.YouCanNotSentOrginalArticle,this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserArticlePanel/IndexArticle");


            }
            return View(article);
        }

        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult SentArticle(Guid Id, FormCollection collection)
        {
            var article =
                CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(Id);
            try
            {
                if (this.Homa.Configuration.GetArticleOrginalFile)
                {
                    if (Session["OrginalFileId"] == null && article.OrginalFileId == null)
                    {
                        ShowMessage(Resources.Congress.PleaseUpLoadOrgianlFile, Resources.Common.MessaageTitle,
                                    messageIcon: MessageIcon.Error);
                        return View(article);
                    }
                }
                else
                {
                    this.RadynTryUpdateModel(article, collection);
                    if (string.IsNullOrEmpty(article.ArticleOrginalText))
                    {
                        ShowMessage(string.Format(Resources.Congress.PleaseEnterArticleOrginalText,this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                                   messageIcon: MessageIcon.Error);
                        return View(article);
                    }
                   
                }
                   
                
                HttpPostedFileBase contentFileId = null;
                if (Session["OrginalFileId"] != null)
                {
                    contentFileId = (HttpPostedFileBase)Session["OrginalFileId"];
                    Session.Remove("OrginalFileId");
                }
                if (!this.Homa.Configuration.AllowSentOrginalWhileAbstractDeny)
                {

                    if (article.FinalState != (byte)Enums.FinalState.AbstractConfirm ||
                            (this.Homa.Configuration.GetAbstractFile &&
                             article.AbstractFileId == null) ||
                            (!this.Homa.Configuration.GetAbstractFile &&
                             string.IsNullOrEmpty(article.Abstract)))
                    {
                        ShowMessage(string.Format(Resources.Congress.YoucannotSendthearticlebecauseyouhavebeenrefusedAbstracts,this.Homa.Configuration.ArticleTitle,this.Homa.Configuration.ArticleTitle),
                            Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                        return View(article);
                    }



                }
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return View(article);

                }
                if (CongressComponent.Instance.BaseInfoComponents.ArticleFacade.SentArticle(article, contentFileId, postFormData))
                {
                    ShowMessage(string.Format(Resources.Congress.Article_Succes_Sent,this.Homa
                        .Configuration.ArticleTitle), Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/UserArticlePanel/IndexArticle");

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserArticlePanel/IndexArticle");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(article);
            }

        }
        public ActionResult GetUserArticlePayment()
        {
            ViewBag.paymentTypes = new SelectList(CongressComponent.Instance.BaseInfoComponents.ArticlePaymentTypeFacade.GetValidList(this.Homa.Id), "Id", "DescriptionField");
            return PartialView("PartialViewArticlePayment");
        }
        [CongressUserAuthorize]
        public ActionResult UserArticlePayment(Guid articleId)
        {

            var article = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);
            if (article == null) return View();
            return View(article);
        }

        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult UserArticlePayment(Guid articleId, FormCollection collection)
        {
            var article = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);
            try
            {

                this.RadynTryUpdateModel(article, collection);
                if (string.IsNullOrEmpty(collection["PaymentTypeId"]))
                {
                    ShowMessage(string.Format(Resources.Congress.PleaseEnterArticlePaymentType,this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                    return Redirect("~/Congress/UserArticlePanel/IndexArticle");
                }
                var config = this.Homa.Configuration;
                var transactionDiscountAttaches = Payment.Tools.AppExtentions.FillTransactionDiscount(collection);
                if (transactionDiscountAttaches != null)
                {
                    if (transactionDiscountAttaches.Count > config.DisscountCount)
                    {
                        ShowMessage(
                            Resources.Congress.YouCanNotUseDiscoutOverThanMax + Tag.NewLine +
                            Resources.Congress.DiscountCount + config.DisscountCount, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Warning);
                        return Redirect("~/Congress/UserArticlePanel/UserArticlePayment?articleId=" + articleId);

                    }
                    var paymnet =
                        CongressComponent.Instance.BaseInfoComponents.ArticleFacade.ArticlePaymnet(
                            article, transactionDiscountAttaches, "/Congress/UserArticlePanel/UpdateStatusAfterTransaction?Id=" + article.Id);
                    if (paymnet)
                    {
                        if (article.TempId.HasValue)
                        {

                            return Redirect("~" + Extentions.PrepaymenyUrl(article.TempId.Value));
                        }
                        ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Succeed);
                        return Redirect("~/Congress/UserArticlePanel/UserArticlePayment?articleId=" + articleId);

                    }
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserArticlePanel/UserArticlePayment?articleId=" + articleId);

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserArticlePanel/UserArticlePayment?articleId=" + articleId);

            }
        }

        public ActionResult UpdateStatusAfterTransaction(Guid id)
        {
            try
            {
                var tr = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.UpdateStatusAfterTransaction(this.Homa.Id,id);
                return tr != Guid.Empty
               ? Redirect("~/Payment/Transaction/TransactionResult?Id=" + tr +
                          "&callbackurl=/Congress/UserArticlePanel/IndexArticle")
                          : Redirect("~/Congress/UserArticlePanel/IndexArticle");


            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserArticlePanel/IndexArticle");
            }
        }
        [CongressUserAuthorize]
        public ActionResult CreateArticle()
        {
            return ValidateCreate();
        }

        private ActionResult ValidateCreate()
        {
            var configuration = this.Homa.Configuration;
            var userarticleCount =
                CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Count(x => x.Id, article => article.UserId == SessionParameters.CongressUser.Id);

            if (!(userarticleCount < configuration.MaxArticlePerUser))
            {
                ShowMessage(string.Format(Resources.Congress.YouCanNotAddArticle,this.Homa.Configuration.ArticleTitle) + Tag.NewLine + string.Format(Resources.Congress.YouCanAddarticleCountOnly,this.Homa.Configuration.ArticleTitle) + " " + configuration.MaxArticlePerUser, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");
            }
            var getAbsrtact = (!string.IsNullOrEmpty(configuration.AbstractStartDate) &&
                               configuration.AbstractStartDate.CompareTo(DateTime.Now.ShamsiDate()) <= 0) &&
                              (!string.IsNullOrEmpty(configuration.AbstractFinishDate) &&
                               configuration.AbstractFinishDate.CompareTo(DateTime.Now.ShamsiDate()) >= 0) &&
                              configuration.GetAbsrtact;
            var getOrginal = (!string.IsNullOrEmpty(configuration.OrginalStartDate) &&
                              configuration.OrginalStartDate.CompareTo(DateTime.Now.ShamsiDate()) <= 0) &&
                             (!string.IsNullOrEmpty(configuration.OrginalFinishDate) &&
                              configuration.OrginalFinishDate.CompareTo(DateTime.Now.ShamsiDate()) >= 0) &&
                             configuration.GetOrginal;

            if (!getAbsrtact && !getOrginal)
            {

                ShowMessage(string.Format(Resources.Congress.Youarenotallowedtosubmitanarticleonthisdate, this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                              messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserPanel/Home");
            }

            ViewBag.AllowPayment = configuration.HasArticlePayment && configuration.ArticlePaymentStep != null && configuration.ArticlePaymentStep == (byte)Enums.ArticlePaymentSteps.BeforSendArticle;
            return View(new Article());
        }

        [CongressUserAuthorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateArticle(FormCollection collection)
        {

            var article = new Article();
            try
            {
                this.RadynTryUpdateModel(article, collection);
                article.UserId = SessionParameters.CongressUser.Id;
                HttpPostedFileBase orginalFileId = null;
                if (Session["OrginalFileId"] != null)
                {
                    orginalFileId = (HttpPostedFileBase)Session["OrginalFileId"];
                    Session.Remove("OrginalFileId");
                }

                HttpPostedFileBase abstractFileId = null;
                if (Session["AbstractFileId"] != null)
                {
                    abstractFileId = (HttpPostedFileBase)Session["AbstractFileId"];
                    Session.Remove("AbstractFileId");
                }
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return Json(new { Result = false, Url = "" }, JsonRequestBehavior.AllowGet);

                }
                article.CongressId = this.Homa.Id;
                article.CurrentUICultureName = collection["LanguageId"];
                var config = this.Homa.Configuration;
                if (config.HasArticlePayment == false || (config.HasArticlePayment && config.ArticlePaymentStep == (byte)Enums.ArticlePaymentSteps.AfterFinalState))
                {
                    if (CongressComponent.Instance.BaseInfoComponents.ArticleFacade.UserInsert(article, (List<ArticleAuthors>)Session["ArticleAuthors"], abstractFileId, orginalFileId, postFormData))
                    {
                        Session.Remove("ArticleAuthors");
                        this.ClearFormGeneratorData(postFormData.Id);
                        ShowMessage(string.Format(Resources.Congress.ArticleInsertSuccessMessage,this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = true, Url = "/Congress/UserArticlePanel/IndexArticle" }, JsonRequestBehavior.AllowGet);
                    }
                    ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                       messageIcon: MessageIcon.Error);
                    return Json(new { Result = false, Url = "" }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(collection["PaymentTypeId"]))
                {
                    ShowMessage(string.Format(Resources.Congress.PleaseEnterArticlePaymentType, this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                    return Json(new { Result = true, Url = "/Congress/UserArticlePanel/CreateArticle" }, JsonRequestBehavior.AllowGet);
                }
                var transactionDiscountAttaches = Payment.Tools.AppExtentions.FillTransactionDiscount(collection);
                if (transactionDiscountAttaches.Count > config.DisscountCount)
                {
                    ShowMessage(
                        Resources.Congress.YouCanNotUseDiscoutOverThanMax + Tag.NewLine +
                        Resources.Congress.DiscountCount + config.DisscountCount, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Warning);
                    return Json(new { Result = false, Url = "" }, JsonRequestBehavior.AllowGet);
                }
                var insert = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.UserInsert(ref article, (List<ArticleAuthors>)Session["ArticleAuthors"], abstractFileId,
                    orginalFileId, transactionDiscountAttaches, "/Congress/UserArticlePanel/UpdateStatusAfterTransaction?Id=", postFormData);
                if (insert)
                {

                    this.ClearFormGeneratorData(postFormData.Id);
                    if (article.TempId.HasValue)
                    {
                        ShowMessage(string.Format(Resources.Congress.ArticleInsertSuccessMessage,this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = true, Url = Extentions.PrepaymenyUrl(article.TempId.Value) }, JsonRequestBehavior.AllowGet);
                    }
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Error);
                    return Json(new { Result = true, Url = "/Congress/UserArticlePanel/IndexArticle" }, JsonRequestBehavior.AllowGet);


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

        [CongressUserAuthorize]
        public ActionResult EditArticle(Guid articleId)
        {
            return ValidateEdit(articleId);
        }

        private ActionResult ValidateEdit(Guid articleId)
        {
            var configuration = this.Homa.Configuration;
            var article = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);

            if (configuration.GetAbsrtact&&configuration.AbstractFinishDate.CompareTo(DateTime.Now.ShamsiDate()) < 0 && article.Status != (int)Enums.FinalState.ConfirmandEdit)
            {
                ShowMessage(string.Format(Resources.Congress.AbstractSentDateIsEnd,this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserArticlePanel/IndexArticle");

            }

            if (article.Status != (byte)Enums.ArticleState.AbstractSended &&
                article.Status != (byte)Enums.ArticleState.ConfirmandEdit &&
                configuration.OrginalFinishDate.CompareTo(DateTime.Now.ShamsiDate()) < 0)
            {
                ShowMessage(string.Format(Resources.Congress.YouCanNotEditArticle,this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserArticlePanel/IndexArticle");
            }
            if (article.Status != (byte)Enums.ArticleState.AbstractSended &&
                article.Status != (byte)Enums.ArticleState.OrginalSended &&
                article.Status != (byte)Enums.ArticleState.ConfirmandEdit)
            {
                ShowMessage(string.Format(Resources.Congress.YouCanNotEditArticle, this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserArticlePanel/IndexArticle");

            }
            return View(article);
        }

        [CongressUserAuthorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult EditArticle(FormCollection collection)
        {

            var Id = collection["Id"].ToGuid();
            var article = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(article);
                HttpPostedFileBase orginalFileId = null;
                if (Session["OrginalFileId"] != null)
                {
                    orginalFileId = (HttpPostedFileBase)Session["OrginalFileId"];
                    Session.Remove("OrginalFileId");
                }
                HttpPostedFileBase abstractFileId = null;
                if (Session["AbstractFileId"] != null)
                {
                    abstractFileId = (HttpPostedFileBase)Session["AbstractFileId"];
                    Session.Remove("AbstractFileId");
                }
                article.CurrentUICultureName = collection["LanguageId"];
                var articleAuthorses = (List<ArticleAuthors>)Session["ArticleAuthors"];
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return Content("false");

                }

                if (CongressComponent.Instance.BaseInfoComponents.ArticleFacade.UserUpdate(article, articleAuthorses, abstractFileId, orginalFileId, postFormData))
                {

                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, new[] { Resources.Common.Ok, "window.location='" + Web.Mvc.UI.Application.CurrentApplicationPath + "/Congress/UserArticlePanel/IndexArticle';" },
                                messageIcon: MessageIcon.Succeed);
                    Session.Remove("ArticleAuthors");
                    this.ClearFormGeneratorData(postFormData.Id);
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

        [CongressUserAuthorize]
        public ActionResult DetailsArticle(Guid articleId)
        {
            var congressArticle = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);
            return View(congressArticle);
        }

        [CongressUserAuthorize]
        public ActionResult DeleteArticle(Guid articleId)
        {
            var article = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);
            return View(article);
        }

        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult DeleteArticle(Guid articleId, FormCollection collection)
        {
            var congressArticle = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Delete(articleId))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/UserArticlePanel/IndexArticle");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserArticlePanel/IndexArticle");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(congressArticle);
            }
        }

        [CongressUserAuthorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult ValidateArticle(FormCollection collection)
        {

            try
            {
                var userId = SessionParameters.CongressUser.Id;
                var messageStack = new List<string>();
                var abstractSend = true;
                var orginalSend = true;
                var articleId = collection["Id"].ToGuid();
                var article = new Article();
                if (articleId != Guid.Empty) article = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);
                this.RadynTryUpdateModel(article, collection);
                var configuration = this.Homa.Configuration;
                if (configuration.AllowGetArticleAbstract())
                {
                    if (!configuration.GetAbstractFile)
                    {
                        if (string.IsNullOrEmpty(article.Abstract))
                            messageStack.Add(string.Format(Resources.Congress.PleaseEnterAbstract,this.Homa.Configuration.ArticleTitle));
                        else
                        {
                            if (!configuration.CheckAbstractMaxWordCount(article.Abstract))
                                messageStack.Add(Resources.Congress.abstractWordCountNotAllowed);
                            if (!configuration.CheckAbstractMinWordCount(article.Abstract))
                                messageStack.Add(Resources.Congress.Abstractwordcountislessthantheminimumallowed);
                        }
                    }
                    else
                    {
                        if (Session["AbstractFileId"] == null && article.AbstractFileId == null)
                            messageStack.Add(string.Format(Resources.Congress.PleaseEnterAbstractFile,this.Homa.Configuration.ArticleTitle));
                        if (Session["AbstractFileId"] != null &&
                            !configuration.CheckAbstarctFileLenght((HttpPostedFileBase)Session["AbstractFileId"]))
                            messageStack.Add(Resources.Congress.AbstractFileSizeNotAllowed);
                    }
                }
                else
                    abstractSend = false;
                if (configuration.AllowGetArticleOrginal())
                {
                    if (!configuration.GetArticleOrginalFile)
                    {

                        var orginalisnull = (article.ArticleOrginalText == null || string.IsNullOrEmpty(article.ArticleOrginalText.Trim()));
                        if (!configuration.AllowSentOrginalWhileAbstractDeny)
                        {
                            if (!orginalisnull && (article.FinalState != (byte)Enums.FinalState.AbstractConfirm ||
                                                   (this.Homa.Configuration.GetAbstractFile &&
                                                    article.AbstractFileId == null && Session["AbstractFileId"] == null) ||
                                                   (!this.Homa.Configuration.GetAbstractFile &&
                                                    string.IsNullOrEmpty(article.Abstract))))
                                messageStack.Add(
                                    string.Format(string.Format(Resources.Congress.YoucannotSendthearticlebecauseyouhavebeenrefusedAbstracts, this.Homa.Configuration.ArticleTitle, this.Homa.Configuration.ArticleTitle)));
                            else
                            {
                                if (orginalisnull)
                                {
                                    if (article.FinalState == (byte)Enums.FinalState.AbstractConfirm)
                                        messageStack.Add(string.Format(Resources.Congress.PleaseEnterTheTextOfTheArticle,this.Homa.Configuration.ArticleTitle));
                                }
                                else
                                {

                                    if (!configuration.CheckOrginalMaxWordCount(article.ArticleOrginalText))
                                        messageStack.Add(string.Format(Resources.Congress.MaximumNumberAllowed,this.Homa.Configuration.ArticleTitle));
                                    if (!configuration.CheckOrginalMinWordCount(article.ArticleOrginalText))
                                        messageStack.Add(string.Format(this.Homa.Configuration.ArticleTitle));

                                }

                            }
                        }
                        else
                        {
                            if (orginalisnull)
                                messageStack.Add(string.Format(Resources.Congress.PleaseEnterTheTextOfTheArticle, this.Homa.Configuration.ArticleTitle));
                            else
                            {
                                if (!string.IsNullOrEmpty(article.ArticleOrginalText))
                                {
                                    if (!configuration.CheckOrginalMaxWordCount(article.ArticleOrginalText))
                                        messageStack.Add(string.Format(Resources.Congress.MaximumNumberAllowed, this.Homa.Configuration.ArticleTitle));
                                    if (!configuration.CheckOrginalMinWordCount(article.ArticleOrginalText))
                                        messageStack.Add(string.Format(this.Homa.Configuration.ArticleTitle));
                                }
                            }
                        }


                    }
                    else
                    {
                        var orginalisnull = Session["OrginalFileId"] == null && article.OrginalFileId == null;
                        if (!configuration.AllowSentOrginalWhileAbstractDeny)
                        {
                            if (!orginalisnull && (article.FinalState != (byte)Enums.FinalState.AbstractConfirm ||
                                                   (this.Homa.Configuration.GetAbstractFile &&
                                                    article.AbstractFileId == null && Session["AbstractFileId"] == null) ||
                                                   (!this.Homa.Configuration.GetAbstractFile &&
                                                    string.IsNullOrEmpty(article.Abstract))))
                                messageStack.Add(
                                    string.Format(string.Format(Resources.Congress.YoucannotSendthearticlebecauseyouhavebeenrefusedAbstracts, this.Homa.Configuration.ArticleTitle, this.Homa.Configuration.ArticleTitle)));
                            else
                            {
                                if (article.FinalState == (byte)Enums.FinalState.AbstractConfirm && orginalisnull)
                                    messageStack.Add(Resources.Congress.PleaseUpLoadOrgianlFile);
                            }
                        }
                        else
                        {
                            if (orginalisnull)
                                messageStack.Add(Resources.Congress.PleaseUpLoadOrgianlFile);



                            if (Session["OrginalFileId"] != null &&
                                !configuration.CheckArticleFileLenght((HttpPostedFileBase)Session["OrginalFileId"]))
                                messageStack.Add(string.Format(string.Format(Resources.Congress.ArticleFileSizeNotAllowed,this.Homa.Configuration.ArticleTitle),this.Homa.Configuration.ArticleTitle));
                        }
                    }

                }
                else
                    orginalSend = false;
                if (!abstractSend && !orginalSend)
                    messageStack.Add(string.Format(Resources.Congress.YouCanNotAddArticle, this.Homa.Configuration.ArticleTitle));
                if (this.Homa.Configuration.AllowUserAddAuthor)
                {
                    var articleAuthorses = (List<ArticleAuthors>)Session["ArticleAuthors"];
                    if (articleId == Guid.Empty)
                    {
                        if (articleAuthorses == null || articleAuthorses.Count == 0)
                            messageStack.Add(string.Format(Resources.Congress.Pleaseenteraminimumorauthorforthearticle, this.Homa.Configuration.ArticleTitle));
                    }
                    if (articleAuthorses != null && !articleAuthorses.Any(x => x.IsDirector))
                        messageStack.Add(Resources.Congress.ThisArticleNotHaveDirectorAuthor);
                }
               

                if (article.PivotId == Guid.Empty)
                    messageStack.Add(string.Format(Resources.Congress.PleaseEnterArticlePivot,this.Homa.Configuration.ArticleTitle));
                if (Session["OrginalFileId"] != null || article.OrginalFileId != null)
                {
                    if (configuration.OrginalStartDate.CompareTo(DateTime.Now.ShamsiDate()) > 0)
                        messageStack.Add(string.Format(Resources.Congress.OrginalStartDateNoStart,this.Homa.Configuration.ArticleTitle));
                    else if (configuration.OrginalFinishDate.CompareTo(DateTime.Now.ShamsiDate()) < 0)
                        messageStack.Add(string.Format(Resources.Congress.OrginalSentDateIsEnd,this.Homa.Configuration.ArticleTitle));
                }
                article.CurrentUICultureName = collection["LanguageId"];
                if (string.IsNullOrEmpty(article.Title))
                    messageStack.Add(string.Format(Resources.Congress.PleaseEnterArticleTitle,this.Homa.Configuration.ArticleTitle));


                var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                if (messageBody != "")
                {
                    ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return Content("false");
                }
                return Content("true");
            }
            catch (Exception exception)
            {

                ShowExceptionMessage(exception);
                return Content("false");
            }

        }


    }
}
