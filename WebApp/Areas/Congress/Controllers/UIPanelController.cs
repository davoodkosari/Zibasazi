using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.ContentManager.DataStructure;
using Radyn.News;
using Radyn.Slider;
using Radyn.Utility;
using System.Linq;
using System.Linq.Expressions;
using Radyn.ContentManager;
using Radyn.Framework;
using Radyn.News.DataStructure;
using Radyn.Web.Html;
using Radyn.Web.Mvc.UI.Captcha;
using Radyn.Web.Mvc.UI.Message;
using Radyn.Web.Mvc.UI.Theme;
using Radyn.Web.Mvc.Utility;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.ContentManager.Tools;
using Radyn.News.Tools;
using Radyn.Security;
using Radyn.Security.Tools;
using Radyn.WebApp.Tools;
using Stimulsoft.Base.Serializing;
using Enums = Radyn.Congress.Tools.Enums;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class UIPanelController : CongressBaseController
    {

        public ActionResult Search()
        {
            
            var txtvalue = Request.QueryString["SearchValue"];
            var SerachType = Request.QueryString["SerachType"];
            var resultvalues = CongressComponent.Instance.BaseInfoComponents.HomaFacade.SearchInHoma(this.Homa.Id,
                SerachType.ToEnum<Enums.SearchType>(), txtvalue);
         
         
           

            return View(resultvalues);
        }
        public ActionResult ArticleApproveList()
        {
            var @select = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Select(
                new Expression<Func<Article, object>>[] { x => x.Title, x => x.OrginalFileId },
                x => x.CongressId == this.Homa.Id && x.OrginalFileId.HasValue && (x.FinalState == (int)Enums.FinalState.Confirm || x.FinalState == (int)Enums.FinalState.AbstractConfirm),
                new OrderByModel<Article>() { Expression = x => x.PublishDate, OrderType = OrderType.DESC });
          
            return View(@select);
        }

        public ActionResult ArticleList()
        {
            var @select = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Select(
                new Expression<Func<Article, object>>[] { x => x.Title, x => x.OrginalFileId },
                x => x.CongressId == this.Homa.Id && x.OrginalFileId.HasValue,
                new OrderByModel<Article>() { Expression = x => x.PublishDate, OrderType = OrderType.DESC });
            return View(@select);
        }
        public ActionResult CongressNewsView(Int32 Id)
        {
            var congressNews = CongressComponent.Instance.BaseInfoComponents.CongressNewsFacade.Get(this.Homa.Id, Id);
            if (congressNews == null) return View(new Radyn.News.DataStructure.News());
            var model = congressNews.News;
            model.NewsContent = model.GetNewsContent(SessionParameters.Culture);
            return View(model);
        }

      
        public ActionResult CongressNewsList()
        {
            var news = CongressComponent.Instance.BaseInfoComponents.CongressNewsFacade.TopCount(this.Homa.Id, null);
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
        public ActionResult GetCongressNewsList()
        {
            var news = CongressComponent.Instance.BaseInfoComponents.CongressNewsFacade.TopCount(this.Homa.Id, null);
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
                return PartialView("PVCongressNewsList",newses.ToList());
            }

            ShowMessage(Resources.News.This_Category_Not_have_News);
            return null;

        }
        public ActionResult GetPivots()
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.PivotFacade.Select(x => x.Title,
                    x => x.CongressId == this.Homa.Id);
            return PartialView("PartialViewpivot", list);
        }

        public ActionResult GetSupporters(short Id)
        {

            var type = CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.GetSupportWithSupporters((short)Id);
            if (type == null) return null;
            ViewBag.AllowScroll = this.Homa.Configuration.IsScrollSupporter;
            ViewBag.ImagesCount = type.Supporters.Count(x => x.Image != null);

            return PartialView("PartialViewSupporter", type);

        }
        public ActionResult GetSlideShow()
        {

            if (this.Homa.Configuration.BigSlideId == null) return null;
            var list = SliderComponent.Instance.SlideFacade.GetSlideWithSliders((short)this.Homa.Configuration.BigSlideId);
            if (list == null) return null;
            ViewBag.Id = "CongressBigerSlideShow";
            return PartialView("PartialViewSlideShow", list);
        }
        public ActionResult GetMinSlideShow()
        {

            if (this.Homa.Configuration.MiniSlideId == null) return null;
            var list = SliderComponent.Instance.SlideFacade.GetSlideWithSliders((short)this.Homa.Configuration.MiniSlideId);
            if (list == null) return null;
            ViewBag.Id = "CongressMinSlideShow";
            return PartialView("PartialViewSlideShow", list);
        }
        public ActionResult GetAverageSlideShow()
        {

            if (this.Homa.Configuration.AverageSlideId == null) return null;
            var list = SliderComponent.Instance.SlideFacade.GetSlideWithSliders((short)this.Homa.Configuration.AverageSlideId);
            if (list == null) return null;
            ViewBag.Id = "CongressAverageSlideShow";
            return PartialView("PartialViewSlideShow", list);


        }
        public ActionResult GetMarquee()
        {
            return PartialView("PartialViewMarquee ");
        }
        public ActionResult GetLogo()
        {
            return PartialView("PartialViewLogo", this.Homa.Configuration.GetConfigurationContent());

        }
        public ActionResult GetPoster()
        {

            return PartialView("PartialViewPoster", this.Homa.Configuration.GetConfigurationContent());
        }

        public ActionResult GetAbout()
        {
            return PartialView("PartialViewAbout", null);
        }

        public ActionResult GetFoter()
        {

            return PartialView("PartialViewFooter", this.Homa.Configuration.GetConfigurationContent());
        }
        public ActionResult GetHeader()
        {
            return PartialView("PartialViewHeader", this.Homa.Configuration.GetConfigurationContent());
        }
        public ActionResult GetGallery()
        {

            return PartialView("PartialViewGallery");
        }
        public ActionResult GetNews()
        {

            var maxNewsCountShow = this.Homa.Configuration.MaxNewsCountShow;
            var enumerable = CongressComponent.Instance.BaseInfoComponents.CongressNewsFacade.TopCount(this.Homa.Id, maxNewsCountShow);
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
        public string ShowNews()
        {

            var maxNewsCountShow = this.Homa.Configuration.MaxNewsCountShow;
            var enumerable = NewsComponent.Instance.NewsFacade.GetTopNews(maxNewsCountShow ?? 0, false);
            return this.RenderPartialToString("PartialViewNews", enumerable);

        }

        public ActionResult GetRegister()
        {
            Session["Ipaddress"] = Request != null ? Request.ServerVariables["REMOTE_ADDR"] : null;
            Session["RequestCount"] = 0;
            return PartialView("PartialViewRegister",new LoginViewModel());
        }
        public ActionResult GetMenuHorizontal()
        {
            var model = CongressComponent.Instance.BaseInfoComponents.CongressMenuFacade.MenuTree(this.Homa.Id, Guid.Empty).Where(x => !x.IsVertical).ToList();
            if (SessionParameters.CongressUser != null)
                model.Add(new Menu { Text = Resources.Congress.UserPanel, Link = "/Congress/UserPanel/Home" });
            if (SessionParameters.CongressReferee != null)
                model.Add(new Menu { Text = Resources.Congress.RefereePanel, Link = "/Congress/RefereePanel/RefereeCartablIndex" });
            if (this.Homa.Configuration.MenuHtml != null)
                ContentManagerComponent.Instance.MenuHtmlFacade.GetLanuageContent(SessionParameters.Culture, this.Homa.Configuration.MenuHtml);
            ViewBag.MenuHtml = this.Homa.Configuration.MenuHtml;
            return PartialView("PartialViewMenuShowHorizontal", model);
        }
        public ActionResult GetMenuVetical()
        {
            var model = CongressComponent.Instance.BaseInfoComponents.CongressMenuFacade.MenuTree(this.Homa.Id, Guid.Empty).Where(x => x.IsVertical);

            return PartialView("PartialViewMenuShowVertical", model);
        }
        public ActionResult GetSearch()
        {
            ViewBag.SerachTypes = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.SearchType>(), "Key", "Value");
            return PartialView("PartialViewSearch");
        }

        public ActionResult GetVip()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.VipFacade.OrderBy(x => x.Sort,
                    x => x.CongressId == this.Homa.Id);
            if (list.Count == 0)
            {
                return null;
            }
            ViewBag.AllowScroll = this.Homa.Configuration.IsScrollVIP;
            return PartialView("PartialViewVIP", list);
        }
        public ActionResult Getarticles()
        {

            var maxArticleCountShow =
                    this.Homa.Configuration.MaxArticleCountShow;
            var list = maxArticleCountShow > 0 ?
                CongressComponent.Instance.BaseInfoComponents.ArticleFacade.SelectTop((int)maxArticleCountShow,
                    article => article.CongressId == this.Homa.Id) :
                    CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Where(
                    article => article.CongressId == this.Homa.Id);
            return PartialView("PartialViewArticles", list);

        }
       
        public ActionResult Index(string culture)
        {

            if (!string.IsNullOrEmpty(culture))
            {
                SessionParameters.Culture = culture;
                return RedirectToAction("Index");
            }

            return View(this.Homa);
        }
        public ActionResult GetIndex()
        {


            if (this.Homa.Configuration.HtmlDesgin == null) return PartialView("PVIndex");
            ContentManagerComponent.Instance.HtmlDesginFacade.GetLanuageContent(SessionParameters.Culture, this.Homa.Configuration.HtmlDesgin);
            var themeName = String.IsNullOrEmpty(Request.QueryString["theme"])
                ? this.Homa.Configuration.Theme
                : Request.QueryString["theme"];
            var resourcehtml = string.IsNullOrEmpty(themeName) ? "" :
                ThemeManager.HtmlOutput(themeName);
            if (this.Homa != null && this.Homa.Configuration != null && !string.IsNullOrEmpty(this.Homa.Configuration.ThemeColorURL))
                resourcehtml += "<link href = " + @SessionParameters.CurrentCongress.Configuration.ThemeColorURL + " rel = \"stylesheet\" />";
            ViewBag.Html = this.GetHtml(this.Homa.Configuration.HtmlDesgin, this.Homa.CongressTitle, resourcehtml, this.Homa.Configuration.DefaultContrainerId);
            return PartialView("PVIndex", this.Homa);
        }
        public ActionResult GetRegisterNewsLetter()
        {

            return PartialView("PVRegisterNewsLetter");
        }
        public ActionResult RegisterNewsLetter(FormCollection collection)
        {
            try
            {
                var messageStack = new List<string>();
                if (!Request.Url.Authority.Contains("localhost"))
                {
                    var service = new CaptchaService();
                    if (!service.IsValidCaptcha(collection["captch"]))
                        messageStack.Add(Resources.CommonComponent.Enterthesecuritycodeisnotvalid);
                }
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
                    return Json(new { Result = false, Message = messageBody }, JsonRequestBehavior.AllowGet);
                }
                var model = CongressComponent.Instance.BaseInfoComponents.NewsLetterFacade.Get(this.Homa.Id, collection["mail"]);
                if (model == null)
                {
                    var newsLetter = new NewsLetter { CongressId = this.Homa.Id, Email = collection["mail"] };
                    return
                        Json(
                            CongressComponent.Instance.BaseInfoComponents.NewsLetterFacade.Insert(newsLetter)
                                ? new { Result = true, Message = Resources.Congress.YourEmailSuccedRegisterInNewsLetter }
                                : new { Result = false, Message = Resources.Common.ErrorInInsert },
                            JsonRequestBehavior.AllowGet);
                }
                return Json(new { Result = false, Message = Resources.Congress.YourEmailRegisteredInCongressNewsLetter }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ShowMessage(ex.Message + " " + ex.InnerException.Message, "", messageIcon: MessageIcon.Security);
                else
                    ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                ViewBag.Message = ex.Message;
                return Json(new { Result = false, Message = ViewBag.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult RemoveFromNewsLetter(FormCollection collection)
        {
            try
            {
                var messageStack = new List<string>();
                if (!Request.Url.Authority.Contains("localhost"))
                {
                    var service = new CaptchaService();
                    if (!service.IsValidCaptcha(collection["captch"]))
                        messageStack.Add(Resources.CommonComponent.Enterthesecuritycodeisnotvalid);
                }
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
                    return Json(new { Result = false, Message = messageBody }, JsonRequestBehavior.AllowGet);
                }
                return
                    Json(
                        CongressComponent.Instance.BaseInfoComponents.NewsLetterFacade.Delete(this.Homa.Id,
                            collection["mail"])
                            ? new { Result = true, Message = Resources.Congress.YourEmailSuccedDeleteInNewsLetter }
                            : new { Result = false, Message = Resources.Common.ErrorInDelete },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ShowMessage(ex.Message + " " + ex.InnerException.Message, "", messageIcon: MessageIcon.Security);
                else
                    ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                return Json(new { Result = false, Message = ViewBag.Message }, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult PrintUserCardWithNationalCode()
        {
            return PartialView("PVPrintUserCardWithNationalCode");
        }

        public ActionResult GetUserCardWithNationalCode(string nationalCode,string date)
        {
            if (nationalCode==null||string.IsNullOrEmpty(nationalCode.Trim()))
            {
                return Json(new { ResponseStatus = false, Message = Resources.Congress.PleaseEnterRightNationalCode }, JsonRequestBehavior.AllowGet);
            }
            if (!Radyn.Utility.Utils.ValidNationalID(nationalCode))
            {
                return Json(new { ResponseStatus = false, Message = Resources.Congress.Please_Enter_NationalCode }, JsonRequestBehavior.AllowGet);
            }
            var user = CongressComponent.Instance.BaseInfoComponents.UserFacade.FirstOrDefault(x =>
                x.EnterpriseNode.RealEnterpriseNode.NationalCode == nationalCode);
            if (user == null)
            {
                
                return Json(new { ResponseStatus = false, Message = Resources.Congress.UserNotFound }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ResponseStatus = true , UserId = user.Id , Message =""}, JsonRequestBehavior.AllowGet);
        }
    }

}
