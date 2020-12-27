using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade;
using Radyn.Framework.DbHelper;
using Radyn.Utility;
using Radyn.Web.Parser;

namespace Radyn.Congress.Tools
{

    public static class Extention
    {
        public static bool CheckAbstarctFileLenght(this Configuration config, HttpPostedFileBase abstractFileId)
        {


            var i = abstractFileId.ContentLength / 1024;
            return !(i >= config.AbstractFileSize);


        }
        public static bool CheckArticleFileLenght(this Configuration config, HttpPostedFileBase articleFileId)
        {


            var i = articleFileId.ContentLength / 1024;
            return !(i >= config.ArticleOrginalFileSize);



        }
        public static string PartialUrl(this SupportType obj)
        {
            return "/Congress/UIPanel/GetSupporters/" + obj.Id;

        }
        public static bool CheckAbstractMaxWordCount(this Configuration config, string text)
        {

            if (config.AbstractWordCount == null || config.AbstractWordCount <= 0) return true;
            text = text.Trim().RemoveHtml();
            var numtext = text.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Count();
            return numtext <= config.AbstractWordCount;


        }
        public static bool CheckAbstractMinWordCount(this Configuration configuration, string text)
        {


            if (configuration.MinAbstractWordCount == null || configuration.MinAbstractWordCount <= 0) return true;
            return text.Split().Count() >= configuration.MinAbstractWordCount;


        }

        public static bool CheckOrginalMaxWordCount(this Configuration config, string text)
        {

            if (config.ArticleOrginalWordCount == null || config.ArticleOrginalWordCount <= 0) return true;
            text = text.Trim().RemoveHtml();
            var numtext = text.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Count();
            return numtext <= config.ArticleOrginalWordCount;


        }
        public static bool CheckOrginalMinWordCount(this Configuration configuration, string text)
        {


            if (configuration.MinArticleOrginalWordCount == null || configuration.MinArticleOrginalWordCount <= 0) return true;
            return text.Split().Count() >= configuration.MinArticleOrginalWordCount;


        }
        public static string GetHomaCompleteUrl(this Homa homa)
        {
            return System.Web.HttpContext.Current != null ? (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Url.Authority) ? "Http://" + System.Web.HttpContext.Current.Request.Url.Authority : "") : "";
        }
        public static string GetHomaRefereePanelUrl(this Homa homa)
        {
            string urlPanel = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Url.Authority) ? "Http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/Congress/RefereePanel/Login?hid=" + homa.Id : "";
            return "<a href=\"" + urlPanel + "\">" + urlPanel + "</a>";
        }

        public static string GetHomaArticleRefereePanelUrl(this Homa homa,Guid articleId,Guid RefereeId)
        {
            string urlPanel = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Url.Authority) ? "Http://" + System.Web.HttpContext.Current.Request.Url.Authority + $"/Congress/RefereePanel/RefereeCartabl?refreeId={RefereeId}&articleId={articleId}"  : "";
            return "<a href=\"" + urlPanel + "\">" + urlPanel + "</a>";
        }


     
        public static string GetHtmlText(this string strHtml)
        {
            return Radyn.Web.Html.Utility.GetHtmlText(strHtml,ParseMethod.TextOnly);
        }

      


      
        public static bool AllowGetArticleAbstract(this Configuration configuration)
        {
            return (!string.IsNullOrEmpty(configuration.AbstractStartDate) &&
                    configuration.AbstractStartDate.CompareTo(DateTime.Now.ShamsiDate()) <= 0) &&
                   (!string.IsNullOrEmpty(configuration.AbstractFinishDate) &&
                    configuration.AbstractFinishDate.CompareTo(DateTime.Now.ShamsiDate()) >= 0) &&
                   configuration.GetAbsrtact;
        }
        public static bool AllowGetArticleOrginal(this Configuration configuration)
        {
            return (!string.IsNullOrEmpty(configuration.OrginalStartDate) &&
                    configuration.OrginalStartDate.CompareTo(DateTime.Now.ShamsiDate()) <= 0) &&
                   (!string.IsNullOrEmpty(configuration.OrginalFinishDate) &&
                    configuration.OrginalFinishDate.CompareTo(DateTime.Now.ShamsiDate()) >= 0) &&
                   configuration.GetOrginal;
        }

        public static List<string> GetCongressYear(this Homa homa)
        {
            var list = new List<string>();
            if (homa == null || string.IsNullOrEmpty(homa.StartDate) || string.IsNullOrEmpty(homa.EndDate)) return list;
            var shamsiDateToGregorianDate = homa.StartDate.Substring(0, 4).ToInt();
            var gregorianDate = homa.EndDate.Substring(0, 4).ToInt();
            for (int i = shamsiDateToGregorianDate; i <= gregorianDate; i++)
            {
                list.Add(i.ToString());
            }
            return list;
        }
        public static ConfigurationContent GetConfigurationContent(this Configuration configuration)
        {
            if (configuration == null)
                return new ConfigurationContent();
            var culture = System.Globalization.CultureInfo.CurrentUICulture.ToString();
            if (configuration.ConfigurationContent.ContainsKey(culture))
                return configuration.ConfigurationContent[culture];
            var configurationContent =
               new ConfigurationContentFacade().Get(configuration.CongressId,
                    System.Globalization.CultureInfo.CurrentUICulture.ToString());
            var getConfigurationContent = configurationContent ?? new ConfigurationContent();
            configuration.ConfigurationContent.Add(culture, getConfigurationContent);
            return getConfigurationContent;
        }

        public static string GetAtricleTitle(this Guid homaId)
        {
            if (homaId != Guid.Empty)
                return new ConfigurationFacade().SelectFirstOrDefault(x => x.ArticleTitle);
            return string.Empty;
        }

        public static string GetAtricleTitle(Homa homa)
        {
            if (homa != null)
                return homa.Configuration.ArticleTitle;
            return string.Empty;
        }

    }
}
