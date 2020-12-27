using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Web.Mvc.UI.Captcha;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class ArticleUserCommentController : CongressBaseController
    {
        // GET: Congress/ArticleUserComment
        public ActionResult Index(Guid articleId)

        {
            var article = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);
            var comment = new ArticleUserComment() { ArticleId = articleId, Article = article };
            return View(comment);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            try
            {
                var comment = new ArticleUserComment();
                RadynTryUpdateModel(comment, collection);
                PredicateBuilder<ArticleUserComment> query = new PredicateBuilder<ArticleUserComment>();
                query.And(x => x.ArticleId == comment.ArticleId);
                if (!string.IsNullOrEmpty(collection["NameOfUser"]))
                {
                    query.And(x => x.Name.Contains(collection["Name"]) || x.Name.Contains(collection["Name"]));
                }
                if (!string.IsNullOrEmpty(comment.SaveDate))
                {
                    query.And(x => x.SaveDate == comment.SaveDate);
                }
                if (!string.IsNullOrEmpty(comment.SaveTime))
                {
                    query.And(x => x.SaveTime == comment.SaveTime);
                }
                if (collection["ConfirmAdmin"] != "All")
                {
                    query.And(x => x.ConfirmAdmin == comment.ConfirmAdmin);
                }
                var result = CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.OrderByDescending(x => x.SaveDate+" "+x.SaveTime, query.GetExpression());
                return PartialView("PVGetIndex", result);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return Content("false");
            }
        }

        [HttpPost]
        public ActionResult SaveAllComments(FormCollection collection)
        {
            try
            {
                var confirmList = collection["ConfirmList"].Split(',').Select(i => i.ToGuid()).ToList();
                CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.UpdateConfirmAdmin(collection["ArticleId"].ToGuid(), confirmList);
                ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                return Content("true");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return Content("false");
            }
        }

        public ActionResult Details(string commentId, PageMode status)
        {
            ViewBag.Status = status;
            var comment = CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.Get(commentId.ToGuid());
            return PartialView("PVDetails", comment);
        }

        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            try
            {
                var commentId = collection["Id"].ToGuid();
                if (!CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.Delete(commentId))
                {
                    ShowMessage(Resources.Congress.ErrorInDetele, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                ShowMessage(Resources.Congress.SuccesfullyDetele, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Succeed);
                return Content("true");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return Content("false");
            }

        }

        [HttpPost]
        public ActionResult UpdateConfirm(Guid id, bool? confirm)
        {
            try
            {
                var comment = CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.Get(id);

                if (confirm.HasValue)
                {
                    comment.ConfirmAdmin = confirm.Value;
                }
                else
                {
                    comment.ConfirmAdmin = !comment.ConfirmAdmin;
                }
                if (!CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.Update(comment))
                {
                    ShowMessage(Resources.Congress.ErrorInUpdateComments, Resources.Common.MessaageTitle,
                         messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                ShowMessage(Resources.Congress.SuccesfullyUpdate, Resources.Common.MessaageTitle,
                         messageIcon: MessageIcon.Succeed);
                return Content("true");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return Content("false");
            }
        }

        public ActionResult Comments()
        {
            var result = new List<Article>();
            if (this.Homa.Configuration.EnableArticleComment)
                result = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.OrderBy(x => x.Code, x => x.IsShare & x.CongressId == this.Homa.Id);
            return View(result);
        }
        public ActionResult GetComments(Guid articleId)
        {
            var result = CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.OrderByDescending(x => x.SaveDate + "" + x.SaveTime, x => x.ArticleId == articleId && x.ConfirmAdmin && !string.IsNullOrEmpty(x.Description));
            return PartialView("PVGetComments", result);
        }
        public ActionResult SetLike(Guid articleId)
        {
            try
            {
                var ip = WebUtility.GetClientIp();
                var comment = CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.FirstOrDefault(x => x.ArticleId == articleId && x.IP == ip);
                if (comment != null)
                {
                    comment.IsLike = !comment.IsLike;

                    if (!CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.Update(comment))
                    {
                        ShowMessage(Resources.Congress.ErrorInUpdateComments, Resources.Common.MessaageTitle,
                           messageIcon: MessageIcon.Error);
                        return Content("false");
                    }
                }
                else
                {
                    comment = new ArticleUserComment() { IsLike = true, ArticleId = articleId, IP = ip };
                    if (!CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.Insert(comment))
                    {
                        ShowMessage(Resources.Congress.ErrorInUpdateComments, Resources.Common.MessaageTitle,
                           messageIcon: MessageIcon.Error);
                        return Content("false");
                    }
                }

                return Content("true");
            }
            catch (Exception ex)
            {

                ShowExceptionMessage(ex);
                return Content("false");
            }

        }

   
        public JsonResult GetLikes(Guid articleId)
        {
            try
            {
                var obj = new Object();
                var count = CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.Count(x => x.ArticleId == articleId && x.IsLike);
                var ip = WebUtility.GetClientIp();
                var comment = CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.FirstOrDefault(x => x.ArticleId == articleId && x.IP == ip);
                obj = new
                {
                    Count = count,
                    Like = comment.IsLike
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

               ShowExceptionMessage(ex);
               return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SetComment(Guid articleId)
        {
            var comment = new ArticleUserComment() { ArticleId = articleId };
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetComment(FormCollection collection)
        {
            try
            {
                if (!Request.Url.Authority.Contains("localhost"))
                {
                    var service = new CaptchaService();
                    if (!service.IsValidCaptcha(collection["captch"]))
                    {
                        SessionParameters.HasLoginPasswordError = true;
                        throw new Exception(Resources.CommonComponent.Enterthesecuritycodeisnotvalid);
                    }
                }
                var ip = WebUtility.GetClientIp();
                if (string.IsNullOrEmpty(collection["Name"]))
                {
                    ShowMessage(Resources.Congress.PleaseEnterYourName, Resources.Common.MessaageTitle,
                          messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                if (string.IsNullOrEmpty(collection["Description"]))
                {
                    ShowMessage(Resources.Congress.PleaseEnterYourComment, Resources.Common.MessaageTitle,
                          messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var comment = CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.FirstOrDefault(x => x.ArticleId == collection["ArticleId"].ToGuid() && x.IP == ip);
                if (comment == null || !string.IsNullOrEmpty(comment.Description))
                {
                    comment = new ArticleUserComment() { IP = ip };
                    RadynTryUpdateModel(comment, collection);
                    if (!CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.Insert(comment))
                    {
                        ShowMessage(Resources.Congress.ErrorInInsertComment, Resources.Common.MessaageTitle,
                         messageIcon: MessageIcon.Error);
                        return Content("false");
                    }
                }
                else
                {
                    RadynTryUpdateModel(comment, collection);
                    if (!CongressComponent.Instance.BaseInfoComponents.ArticleUserCommentFacade.Update(comment))
                    {
                        ShowMessage(Resources.Congress.ErrorInInsertComment, Resources.Common.MessaageTitle,
                         messageIcon: MessageIcon.Error);
                        return Content("false");
                    }
                }
                return Content("true");

            }
            catch (Exception ex)
            {

                ShowExceptionMessage(ex);
                return Content("false");
            }
        }

       

    }
}