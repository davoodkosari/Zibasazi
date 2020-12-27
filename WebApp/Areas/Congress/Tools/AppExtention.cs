using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Radyn.Common.Component;
using Radyn.Common.DataStructure;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Payment;
using Radyn.Utility;
using Radyn.WebApp.AppCode.Security;
using Resources;
using CommonComponent = Radyn.Common.CommonComponent;

namespace Radyn.WebApp.Areas.Congress.Tools
{
    public static class AppExtention
    {
        public static KeyValuePair<string, string>? HomaError()
        {

            if (SessionParameters.CurrentCongress == null || SessionParameters.CurrentCongress.Status == Enums.CongressStatus.NotRegistered)
                return new KeyValuePair<string, string>(Resources.Common.NotFound, "/Content/Images/conference-alert/not-found.png");
            var status = SessionParameters.CurrentCongress.Status;
            switch (status)
            {
                case Enums.CongressStatus.NotConfiged:

                    return new KeyValuePair<string, string>(Resources.Common.NotConfig, "/Content/Images/conference-alert/setting.png");
                    break;
                case Enums.CongressStatus.NotStatrted:
                    return new KeyValuePair<string, string>(Resources.Common.NotStatrted, "/Content/Images/conference-alert/time.png");
                    break;
                case Enums.CongressStatus.Disabled:
                    return new KeyValuePair<string, string>(Resources.Common.Disabled, "/Content/Images/conference-alert/disable.png");
                    break;
                case Enums.CongressStatus.Ended:
                    return new KeyValuePair<string, string>(Resources.Common.Ended, "/Content/Images/conference-alert/time.png");
                    break;
                case Enums.CongressStatus.NotRegistered:
                    return new KeyValuePair<string, string>(Resources.Common.NotRegistered, "/Content/Images/conference-alert/not-found.png");
                    break;

            }
            return null;
        }
        public static int GetUserTempCount(Guid userId)
        {
            return PaymentComponenets.Instance.TransactionFacade.GetUserTempCount(userId);
        }
        public static string CongressMoudelName
        {
            get { return "Congress-(" + SessionParameters.CurrentCongress.Id + ")"; }
        }
        public static Article PrepareArticleSearch(FormCollection collection)
        {
            var article = new Article()
            {
                FinalStateNullable = (string.IsNullOrEmpty(collection["SearchFinalState"])
                    ? (byte?)null
                    : collection["SearchFinalState"].ToByte()),

                StateNullable = (string.IsNullOrEmpty(collection["SearchStatus"])
                    ? (byte?)null
                    : collection["SearchStatus"].ToByte()),
                PayStatus =
                    (string.IsNullOrEmpty(collection["SearchPaymentStatus"])
                        ? (byte?)null
                        : collection["SearchPaymentStatus"].ToByte()),
                PivotId =
                    (string.IsNullOrEmpty(collection["Search_PivotId"]) ? Guid.Empty : collection["Search_PivotId"].ToGuid()),
                TypeId =
                    (string.IsNullOrEmpty(collection["Search_ArticleTypeId"])
                        ? (Guid?)null
                        : collection["Search_ArticleTypeId"].ToGuid()),
            };
            if (!string.IsNullOrEmpty(collection["Search_PivotCategoryId"]))
            {
                if (article.Pivot == null) article.Pivot = new Pivot();
                article.Pivot.PivotCategoryId = collection["Search_PivotCategoryId"].ToGuid();
            }

            return article;
        }
        public static List<KeyValuePair<string, string>> GetFormList()
        {
            var liste = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(
                    CongressMoudelName + "-/Congress/ArticlePanel/Article",string.Format(Resources.Congress.Article,SessionParameters.CurrentCongress.Configuration.ArticleTitle)),
                      new KeyValuePair<string, string>(
                    CongressMoudelName + "-/Congress/WorkShop/WorkShop",Resources.Congress.WorkShop),
                      new KeyValuePair<string, string>(
                    CongressMoudelName + "-/Congress/Hotel/Hotel",Resources.Congress.Hotel),
                new KeyValuePair<string, string>(
                    CongressMoudelName + "-/home/ContactUs",Resources.Congress.ContactUsForm),
                new KeyValuePair<string, string>(
                    CongressMoudelName + "-/Congress/Userpanel/Complete",Resources.Congress.UserInFormationForm),
                new KeyValuePair<string, string>(
                    CongressMoudelName + "-/Congress/RefereePanel/Refereeopinion",Resources.Congress.RefereeForm),
                new KeyValuePair<string, string>(
                    CongressMoudelName + "-/Congress/Booth/Booth",Resources.Congress.BoothRezerv),
                new KeyValuePair<string, string>(
                    CongressMoudelName + "-/Congress/ArticlePanel/Article",string.Format(Resources.Congress.Article,SessionParameters.CurrentCongress.Configuration.ArticleTitle))

            };
            if (SessionParameters.CurrentCongress != null)
            {
                var a = CongressComponent.Instance.BaseInfoComponents.PivotFacade.Where(x =>
                    x.CongressId == SessionParameters.CurrentCongress.Id);
                foreach (var pivot in a)
                {
                    liste.Add(new KeyValuePair<string, string>(CongressMoudelName + "-/Congress/ArticlePanel/Article?PivotId=" + pivot.Id, pivot.Title));
                }
            }


            return liste;
        }
        public static List<KeyValuePair<string, string>> SlideList()
        {
            var liste = new List<KeyValuePair<string, string>>
            {
                 new KeyValuePair<string, string>(
                    "1",Resources.Congress.BigSlideId),
                new KeyValuePair<string, string>(
                    "0",Resources.Congress.AverageSlideId),
                new KeyValuePair<string, string>(
                    "-1",Resources.Congress.MiniSlideId)
            };

            return liste;
        }
        public static string GetCongressResources(Enums.UseLayout layout = Enums.UseLayout.None)
        {
            if (SessionParameters.CurrentCongress == null) return string.Empty;
            return CongressComponent.Instance.BaseInfoComponents.ResourceFacade.GetCongressResourceHtml(
                SessionParameters.CurrentCongress.Id, SessionParameters.Culture, layout);
        }


        private static System.Resources.ResourceManager FindResourceInWebApp(string moudelname)
        {
            switch (moudelname)
            {
                case "Congress":
                    return Resources.Congress.ResourceManager;
                case "Comment":
                    return Comment.ResourceManager;
                case "Calander":
                    return Calander.ResourceManager;
                case "ContentManager":
                    return Resources.ContentManager.ResourceManager;

                case "EnterpriseNode":
                    return EnterPriseNode.ResourceManager;
                case "FAQ":
                    return Resources.FAQ.ResourceManager;
                case "FormGenerator":
                    return Resources.FormGenerator.ResourceManager;
                case "Gallery":
                    return Gallery1.ResourceManager;
                case "Help":
                    return Resources.Help.ResourceManager;
                case "Message":
                    return Resources.Message.ResourceManager;
                case "News":
                    return Resources.News.ResourceManager;
                case "Payment":
                    return Resources.Payment.ResourceManager;
                case "Reservation":
                    return Resources.Reservation.ResourceManager;
                case "Security":
                    return Resources.Security.ResourceManager;
                case "Slider":
                    return Resources.Slider.ResourceManager;
                case "Statistics":
                    return Resources.Statistics.ResourceManager;

            }
            return null;
        }

        public static bool HasUserFile
        {
            get
            {
                return CongressComponent.Instance.BaseInfoComponents.UserFileFacade.Any(x => x.CongressId == SessionParameters.CurrentCongress.Id);
            }
        }

        public static void GetHoma(System.Web.HttpContextBase context)
        {


            bool renew = false;
            if (context == null) return;
            var homaFacade = CongressComponent.Instance.BaseInfoComponents.HomaFacade;
            var query = context.Request.QueryString["hid"];
            if (!string.IsNullOrEmpty(query))
            {
                SessionParameters.CurrentCongress = homaFacade.Get(query);
                renew = SessionParameters.CurrentCongress != null;
            }
            else
            {
                if (context.Request.Url != null)
                {
                    var authority = context.Request.Url.Authority;
                    if (authority.ToLower().StartsWith("www."))
                        authority = authority.Replace("www.", "");
                    if (SessionParameters.CurrentCongress == null)
                        SessionParameters.CurrentCongress = homaFacade.GetUrlHomaId(authority, context.Request.Url.AbsolutePath);
                    

                }
            }

            SessionParameters.Error = HomaError();
            if (!renew && (SessionParameters.UserOperation != null || SessionParameters.CurrentCongress == null || SessionParameters.CongressSessionStarted || SessionParameters.CurrentCongress.Status != Enums.CongressStatus.NoProblem)) return;
            var list = CongressComponent.Instance.BaseInfoComponents.CongressLanguageFacade.Select(x => x.LanguageId, x => x.CongressId == SessionParameters.CurrentCongress.Id && x.Language.Enabled);
            if (list != null && list.Any())
            {
                if (list.Count == 1)
                    SessionParameters.Culture = list.First();
                else
                {
                    var defaultCulture = CommonComponent.GetDefaultCulture;
                    if (!string.IsNullOrEmpty(defaultCulture))
                    {
                        var firstOrDefault = list.FirstOrDefault(x => x == defaultCulture);
                        if (firstOrDefault != null) SessionParameters.Culture = firstOrDefault;
                    }
                }
            }
            SessionParameters.CongressSessionStarted = true;



        }
    }
}