using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.News;
using Radyn.News.DataStructure;
using Radyn.News.Tools;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Filter;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressNewsController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.CongressNewsFacade.Select(x=>x.News,news => news.CongressId == this.Homa.Id);
            return View(list);
        }
        [RadynAuthorize]
        public ActionResult Details(Int32 Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [RadynAuthorize]
        [SourceCodeFile("ImageBrowser Controller", "~/Controllers/ImageBrowserController.cs")]
        public ActionResult Create()
        {
            return View();
        }
        [RadynAuthorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var news = new Radyn.News.DataStructure.News();
            try
            {
                var newsproperty = new NewsProperty();
                var newsContent = new NewsContent();
                RadynTryUpdateModel(news, collection);
                RadynTryUpdateModel(newsproperty, collection);
                RadynTryUpdateModel(newsContent, collection);
                news.SaveDate = DateTime.Now;
                news.Visible = true;
                var file = Session["NewsImage"];
                Session.Remove("NewsImage");
                if (SessionParameters.User != null) news.CreatorId = SessionParameters.User.Id;
                if (CongressComponent.Instance.BaseInfoComponents.CongressNewsFacade.Insert(this.Homa.Id, news, newsContent, newsproperty, (HttpPostedFileBase)file))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = news.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressNews/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(news);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Int32 id)
        {
            ViewBag.Id = id;
            return View();
        }

        [RadynAuthorize]
        [HttpPost, ValidateInput(false)]
        [SourceCodeFile("ImageBrowser Controller", "~/Controllers/ImageBrowserController.cs")]
        public ActionResult Edit(Int32 id, FormCollection collection)
        {
            var news = NewsComponent.Instance.NewsFacade.Get(id);
            try
            {
                var newscontent = NewsComponent.Instance.NewsContentFacade.Get(id, collection["LanguageId"]) ??
                                  new NewsContent();
                var newsProperty = NewsComponent.Instance.NewsPropertyFacade.Get(id);
                this.RadynTryUpdateModel(news, collection);
                this.RadynTryUpdateModel(newsProperty, collection);
                this.RadynTryUpdateModel(newscontent, collection);
                var file = Session["NewsImage"];
                Session.Remove("NewsImage");
                if (SessionParameters.User != null) news.EditorId = SessionParameters.User.Id;
                if (CongressComponent.Instance.BaseInfoComponents.CongressNewsFacade.Update(this.Homa.Id, news, newscontent, newsProperty, (HttpPostedFileBase)file))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = id });
                }
               

                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressNews/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = id;

                return View(news);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Int32 id)
        {

            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        [RadynAuthorize]
        public ActionResult Delete(Int32 id, FormCollection collection)
        {
            var news = NewsComponent.Instance.NewsFacade.Get(id);
            try
            {
               
                var newscontent = NewsComponent.Instance.NewsContentFacade.Get(id, "fa-IR") ??
                                  (NewsComponent.Instance.NewsContentFacade.Get(id, "en-US") ?? new NewsContent());
                var newsProperty = NewsComponent.Instance.NewsPropertyFacade.Get(id);
                this.RadynTryUpdateModel(news, collection);
                this.RadynTryUpdateModel(newsProperty, collection);
                this.RadynTryUpdateModel(newscontent, collection);
                var file = Session["NewsImage"];
                news.Visible = false;
                news.Enabled = false;
                newscontent.Lead = "'";
                newscontent.Body = "'";
                Session.Remove("NewsImage");
                if (SessionParameters.User != null) news.EditorId = SessionParameters.User.Id;
                if (CongressComponent.Instance.BaseInfoComponents.CongressNewsFacade.Update(this.Homa.Id, news, newscontent, newsProperty, (HttpPostedFileBase)file))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = id });
                }
              

                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressNews/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = id;
                return View(news);
            }
        }

    }
}
