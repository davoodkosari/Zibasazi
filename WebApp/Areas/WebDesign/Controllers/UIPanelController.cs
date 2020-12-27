using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Radyn.Slider;
using System.Linq;
using Radyn.Congress;
using Radyn.ContentManager;
using Radyn.News;
using Radyn.News.Tools;
using Radyn.Utility;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.ContentManager.Tools;
using Radyn.WebApp.Areas.WebDesign.Security.Filter;
using Radyn.WebDesign;

namespace Radyn.WebApp.Areas.WebDesign.Controllers
{
    public class UIPanelController : WebDesignBaseController
    {

        public ActionResult Archives(Guid? groupId)
        {

            var news = NewsComponent.Instance.NewsFacade.Where(x => x.NewsCategoryId == groupId && x.Enabled);
            if (news.Count == 0)
            {
                ShowMessage(Resources.News.This_Category_Not_have_News);
                return null;
            }

            var groupe = NewsComponent.Instance.NewsCategoryFacade.Get(groupId);
            TempData["GroupeName"] = groupe.Title;
            ViewBag.groupId = groupId;
            return View("WebSiteNewsList", news.OrderByDescending(x => x.PublishDate).ToList());
        }
        public ActionResult CongressList(int id)
        {
            var congressType = CongressComponent.Instance.BaseInfoComponents.CongressTypeFacade.Get(id);
            if (congressType == null)
            {
                ShowMessage("نوع رویداد یافت نشد");
                return null;
            }
            ViewBag.ModelTitle = congressType.Title;
            var homas = CongressComponent.Instance.BaseInfoComponents.HomaFacade.OrderBy(x => x.Order,
                x => x.CongressTypeId == id);


            return View(homas);

        }
        


        public ActionResult WebSiteNewsList()
        {
            var news =WebDesignComponent.Instance.NewsFacade.TopCount(this.WebSite.Id, null);
            var newses = news as IList<Radyn.News.DataStructure.News> ?? news.ToList();
            if (newses.Any())
            {
                var @where =
                    NewsComponent.Instance.NewsContentFacade.Where(
                        x => x.Id.In(newses.Select(i => i.Id)) && x.LanguageId == SessionParameters.Culture);
                foreach (var newse in newses)
                {
                    newse.NewsContent = where.FirstOrDefault(x => x.Id == newse.Id);
                }
                return View(newses.ToList());
            }

            ShowMessage(Resources.News.This_Category_Not_have_News);
            return null;

        }
        public ActionResult GetWebSiteNewsList()
        {
            var news = WebDesignComponent.Instance.NewsFacade.TopCount(this.WebSite.Id, null);
            var newses = news as IList<Radyn.News.DataStructure.News> ?? news.ToList();
            if (newses.Any())
            {
                var @where =
                    NewsComponent.Instance.NewsContentFacade.Where(
                        x => x.Id.In(newses.Select(i => i.Id)) && x.LanguageId == SessionParameters.Culture);
                foreach (var newse in newses)
                {
                    newse.NewsContent = where.FirstOrDefault(x => x.Id == newse.Id);
                }
                return PartialView("PVWebDesignNewsList", newses.ToList());
            }

            ShowMessage(Resources.News.This_Category_Not_have_News);
            return null;

        }


        public ActionResult GetCertificates()
        {
            if (this.WebSite.Configuration.CertificateSlideId == null) return null;
            var list = SliderComponent.Instance.SlideFacade.GetSlideWithSliders((short)WebSite.Configuration.CertificateSlideId);
            if (list == null) return null;

            ViewBag.Id = "WebSiteCertificatesSlideShow";
            return PartialView("PartialViewCertificates", list.SlideItems);
        }

        public ActionResult GetEvents()
        {
            if (this.WebSite.Configuration.EventsSlideId == null) return null;
            var list = SliderComponent.Instance.SlideFacade.GetSlideWithSliders((short)WebSite.Configuration.EventsSlideId);
            if (list == null) return null;

            ViewBag.Id = "WebSiteEventsSlideShow";
            return PartialView("PartialViewEvents", list.SlideItems);
        }





        public ActionResult GetMinSlideShow()
        {
            if (this.WebSite.Configuration.MiniSlideId == null) return null;
            var list = SliderComponent.Instance.SlideFacade.GetSlideWithSliders((short)this.WebSite.Configuration.MiniSlideId);
            if (list == null) return null;
            ViewBag.Id = "WebSiteMinSlideShow";
            return PartialView("PartialViewSlideShow", list);
        }
        public ActionResult GetAverageSlideShow()
        {
            if (this.WebSite.Configuration.AverageSlideId == null) return null;
            var list = SliderComponent.Instance.SlideFacade.GetSlideWithSliders((short)this.WebSite.Configuration.AverageSlideId);
            if (list == null) return null;
            ViewBag.Id = "WebSiteAverageSlideShow";
            return PartialView("PartialViewSlideShow", list);
        }
        public ActionResult GetBigSlideShow()
        {
            if (this.WebSite.Configuration.BigSlideId == null) return null;
            var list = SliderComponent.Instance.SlideFacade.GetSlideWithSliders((short)this.WebSite.Configuration.BigSlideId);
            if (list == null) return null;
            ViewBag.Id = "WebSiteBigSlideShowSlideShow";
            return PartialView("PartialViewSlideShow", list);
        }
        public ActionResult GetMarquee()
        {
            return PartialView("PartialViewMarquee ");
        }

        public ActionResult GetAbout()
        {
            return PartialView("PartialViewAbout", null);
        }

        public ActionResult GetFooter()
        {
            ViewBag.Id = this.WebSite.Configuration.FooterId;
            return PartialView("PartialViewFooter", this.WebSite.Configuration);
        }
        public ActionResult GetHeader()
        {
            ViewBag.Id = this.WebSite.Configuration.HeaderId;
            return PartialView("PartialViewHeader", this.WebSite.Configuration);
        }
        public ActionResult GetNews()
        {
            var maxNewsCountShow = this.WebSite.Configuration.MaxNewsShow;
            var enumerable = WebDesignComponent.Instance.NewsFacade.TopCount(this.WebSite.Id, maxNewsCountShow);
            if (enumerable.Any())
            {
                var @where =
                    NewsComponent.Instance.NewsContentFacade.Where(
                        x => x.Id.In(enumerable.Select(i => i.Id)) && x.LanguageId == SessionParameters.Culture);
                foreach (var newse in enumerable)
                {
                    newse.NewsContent = where.FirstOrDefault(x => x.Id == newse.Id);
                }
               
            }

            return PartialView("PartialViewNews", enumerable);
        }
        public ActionResult Index(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                SessionParameters.Culture = culture;
                return RedirectToAction("Index");
            }
            return View(this.WebSite);
        }
        public ActionResult WebDesignNewsView(Int32 Id)
        {
            var WebDesignNews = WebDesignComponent.Instance.NewsFacade.Get(this.WebSite.Id, Id);
            if (WebDesignNews == null) return View(new Radyn.News.DataStructure.News());
            var model =WebDesignNews.WebSiteNews;
            model.NewsContent = model.GetNewsContent(SessionParameters.Culture);
            return View(model);
        }
        public ActionResult GetMenu()
        {
            var tree =
                WebDesignComponent.Instance.MenuFacade.MenuTree(this.WebSite.Id); 
            if (this.WebSite.Configuration.MenuHtml != null)
                ContentManagerComponent.Instance.MenuHtmlFacade.GetLanuageContent(SessionParameters.Culture, this.WebSite.Configuration.MenuHtml);
            ViewBag.MenuHtml = this.WebSite.Configuration.MenuHtml;
            return PartialView("PartialViewMenuShow", tree);
        }

        public ActionResult GetCongressCurrent()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetCurrentCongress();
            ViewBag.ContainerTitle = "رویدادهای جاری";
            return PartialView("PartialViewCongressList", list);
        }
        public ActionResult GetCongressLast()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetLastCongress();
            ViewBag.ContainerTitle = "رویدادهای برگذار شده";
            return PartialView("PartialViewCongressList", list);
        }
        public ActionResult GetCongressNext()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetNextCongress();
            ViewBag.ContainerTitle = "رویدادهای آتی";
            return PartialView("PartialViewCongressList", list);
        }

        public ActionResult GetIndex()
        {

            if (this.WebSite.Configuration.DefaultHTMLID == null) return PartialView("PVIndex");
            var htmlDesign = WebDesignComponent.Instance.HtmlFacade.SelectFirstOrDefault(c => c.HtmlDesgin,
                 c => c.HtmlDesginId == this.WebSite.Configuration.DefaultHTMLID);

            ViewBag.Html = this.GetHtml(htmlDesign, this.WebSite.Title, DefaultContrainerId: this.WebSite.Configuration.DefaultContainerID);
            return PartialView("PVIndex", this.WebSite);
        }
      


    }

}
