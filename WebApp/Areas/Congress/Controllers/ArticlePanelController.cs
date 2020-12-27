using Radyn.Common.Component;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Framework;
using Radyn.WebApp.Areas.FormGenerator.Tools;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class ArticlePanelController : CongressBaseController
    {

        public ActionResult GenerateArticle(Guid Id, string Status)
        {
            Article article;
            if (Id != Guid.Empty)
                article = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(Id);
            else article = new Article();
            var congressConfiguration = this.Homa.Configuration;
            switch (Status)
            {
                case "UserModify":
                    {

                        ViewBag.Category = new SelectList(CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.SelectKeyValuePair(x => x.Id, x => x.Title,x=>x.CongressId==this.Homa.Id, new OrderByModel<PivotCategory>() { Expression = x => x.Order }), "Key", "Value");
                        ViewBag.ViewGetOrginalFile = (congressConfiguration.OrginalStartDate.CompareTo(DateTime.Now.ShamsiDate()) <= 0 && congressConfiguration.OrginalFinishDate.CompareTo(DateTime.Now.ShamsiDate()) >= 0 && congressConfiguration.GetOrginal);
                        ViewBag.ViewGetAbstractFile = (congressConfiguration.AbstractStartDate.CompareTo(DateTime.Now.ShamsiDate()) <= 0 && congressConfiguration.AbstractFinishDate.CompareTo(DateTime.Now.ShamsiDate()) >= 0 && congressConfiguration.GetAbsrtact);
                        Session["ArticleAuthors"] =
                            CongressComponent.Instance.BaseInfoComponents.ArticleAuthorsFacade.Where(
                                authors => authors.ArticleId == Id);
                        return PartialView("PartialViewModifyForUser", article);
                    }
                case "AdminModify":
                    {


                        Session["ArticleAuthors"] =
                            CongressComponent.Instance.BaseInfoComponents.ArticleAuthorsFacade.Where(
                                authors => authors.ArticleId == Id);
                        GetArticleAdminModifyViewBags(congressConfiguration);

                        return PartialView("PartialViewModify", article);
                    }
                case "Details":
                    {

                        if (article == null) return null;
                        if (article.Abstract != null)
                            article.Abstract = article.Abstract.Replace("\r\n", "<br />");
                        Session["ArticleAuthors"] =
                           CongressComponent.Instance.BaseInfoComponents.ArticleAuthorsFacade.Where(
                               authors => authors.ArticleId == Id);
                        return PartialView("PartialViewDetails", article);
                    }
                case "UserSent":
                    {
                        if (article == null) return null;
                        if (article.Abstract != null)
                            article.Abstract = article.Abstract.Replace("\r\n", "<br />");
                        Session["ArticleAuthors"] =
                           CongressComponent.Instance.BaseInfoComponents.ArticleAuthorsFacade.Where(
                               authors => authors.ArticleId == Id);
                        return PartialView("PartialViewDetailsForUser", article);
                    }
                case "Referee":
                    {
                        if (article.Abstract != null)
                            article.Abstract = article.Abstract.Replace("\r\n", "<br />");
                        return PartialView("PartialViewDetailsForReferee", article);
                    }


            }
            return null;
        }

       

        [HttpGet]
        public ActionResult DeleteOrginalId(Guid orginalId)
        {

            try
            {
                var obj = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Where(c => c.OrginalFileId == orginalId);
                if (obj != null)
                {
                    obj[0].OrginalFileId = null;

                    var data = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Update(obj[0]);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ShowMessage("موردي براي حذف يافت نشد");
                    return Content("false");
                }

            }
            catch (Exception)


            {
                ShowMessage("خطا در حذف اطلاعات!");
                return Content("false");

            }

        }




        public void GetArticleAdminModifyViewBags(Configuration congressConfiguration)
        {
            ViewBag.Category = new SelectList(CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.SelectKeyValuePair(x => x.Id, x => x.Title,x=>x.CongressId==this.Homa.Id,new OrderByModel<PivotCategory>(){Expression = x=>x.Order}), "Key", "Value");
            ViewBag.ArticleType = new SelectList(CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.SelectKeyValuePair(x => x.Id, x => x.Title, x => x.CongressId == this.Homa.Id), "Key", "Value");
            ViewBag.Config = congressConfiguration;
            if (this.Homa.Configuration.HasArticlePayment)
            {
                ViewBag.paystatuslist =
               EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.ArticlepayState>().Select(
                   keyValuePair =>
                       new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.ArticlepayState>(),
                           keyValuePair.Value));
            }
            ViewBag.finalstatus =
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.FinalState>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.FinalState>(),
                            keyValuePair.Value));

        }



        public ActionResult Archive()
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.ArticleFacade.OrderBy(x => x.PublishDate,
                    article => article.CongressId == this.Homa.Id);
            return View(list);
        }
        public ActionResult GetAuthor(Guid? Id, PageMode status)
        {

            try
            {
                if (Session["ArticleAuthors"] == null) Session["ArticleAuthors"] = new List<ArticleAuthors>();
                ArticleAuthors articleAuthors = null;
                switch (status)
                {
                    case PageMode.Create:
                        articleAuthors = new ArticleAuthors(){Id = Guid.NewGuid()};
                        break;
                    case PageMode.Edit:
                        var list = (List<ArticleAuthors>)Session["ArticleAuthors"];
                        articleAuthors = list.FirstOrDefault(organization => organization.Id.Equals(Id));
                        break;
                }
                this.PrepareViewBags(articleAuthors, status);
                return PartialView("PartialViewArticleAuthor", articleAuthors);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        [HttpPost]
        public ActionResult GetAuthor(FormCollection collection)
        {

            try
            {
                ArticleAuthors articleAuthors = null;
                PageMode pageMode = base.GetPageMode<ArticleAuthors>(collection);
                var id = base.GetModelKey<ArticleAuthors>(collection);
                var list = (List<ArticleAuthors>)Session["ArticleAuthors"];
                var guid = id[0].ToString().ToGuid();
                if (string.IsNullOrEmpty(collection["Name"]))
                {
                    ShowMessage(Resources.Congress.PleaseEnterAuthorName, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Warning);
                    return Content("false");
                }
                else
                {
                    if (list.Any(x => x.Name == collection["Name"] && x.Id != guid))
                    {
                        ShowMessage($"نویسنده {collection["Name"]} قبلا ثبت شده است", Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Warning);
                        return Content("false");
                    }
                }
                if (!string.IsNullOrEmpty(collection["Percentage"]))
                {
                    if (collection["Percentage"].ToByte() <= 0)
                    {
                        ShowMessage(Resources.Congress.PleaseEnterTheAuthorContributionPercentage, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Warning);
                        return Content("false");
                    }
                    var sumPercent = list.Sum(x => x.Percentage);
                    if (sumPercent + collection["Percentage"].ToByte() > 100)
                    {
                        ShowMessage(Resources.Congress.TotalPercentage, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Warning);
                        return Content("false");
                    }
                }

                switch (pageMode)
                {
                    case PageMode.Edit:
                        articleAuthors = list.FirstOrDefault(organizationIp => organizationIp.Id.Equals(guid)); 
                        if(articleAuthors==null) return Content("false");
                        RadynTryUpdateModel(articleAuthors, collection);
                        if (articleAuthors.IsDirector)
                        {
                            if (!list.Any(x => x.IsDirector & x.Id != articleAuthors.Id)) return Content("true");
                            ShowMessage(string.Format(Resources.Congress.AuthorDirectorAddedThisArticle, this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Error);
                            return Content("false");
                        }
                        return Content("true");
                    case PageMode.Create:
                        articleAuthors = new ArticleAuthors();
                        RadynTryUpdateModel(articleAuthors, collection);
                        if (articleAuthors.IsDirector)
                        {
                            if (list.Any(x => x.IsDirector))
                            {
                                ShowMessage(string.Format(Resources.Congress.AuthorDirectorAddedThisArticle, this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                                    messageIcon: MessageIcon.Error);
                                return Content("false");
                            }
                        }
                        articleAuthors.Order = list.Count == 0 ? 1 : list.Max(x => x.Order) + 1;
                        list.Add(articleAuthors);
                        return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return Content("false");
            }

           
        }
        public ActionResult DeleteAuthor(Guid Id)
        {
            var list = (List<ArticleAuthors>)Session["ArticleAuthors"];
            var item = list.FirstOrDefault(ip => ip.Id.Equals(Id));
            if (item == null) return Content("false");
            list.Remove(item);
            return Content("true");
        }
        public ActionResult GetAuthorList(bool? hiddenEdit = false)
        {
            ViewBag.hiddenEdit = hiddenEdit;
            var list = (List<ArticleAuthors>)Session["ArticleAuthors"] ?? new List<ArticleAuthors>();
            return PartialView("PartialViewArticleAuthors", list.OrderBy(authors => authors.Order));
        }



    }
}
