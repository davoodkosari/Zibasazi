using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Common;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.Congress.Tools;


namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class ConfigurationController : CongressBaseController
    {


        public ActionResult GetContent(Guid? Id, string langid)
        {

            var contentContent = new ConfigurationContent { LanguageId = SessionParameters.Culture };
            if (Id != null && Id != Guid.Empty)
            {
                var content = CongressComponent.Instance.BaseInfoComponents.ConfigurationContentFacade.Get((Guid)Id, string.IsNullOrEmpty(langid) ? SessionParameters.Culture : langid);
                if (content != null) contentContent = content;
            }
            ViewBag.Lanuages = new SelectList(CommonComponent.Instance.LanguageFacade.SelectKeyValuePair(x => x.Id, x => x.DisplayName), "Key", "Value");
            ViewBag.CongressContents = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongressContentFacade.SelectKeyValuePair(x => x.ContentId, x => x.Content.Title, x => x.CongressId == this.Homa.Id,new OrderByModel<CongressContent>(){Expression = x=>x.Content.Title}), "Key", "Value");
            return PartialView("ConfigurationContent", contentContent);
        }


        public ActionResult GetModify(Guid? Id, string culture)
        {


            GetValue();
            if (string.IsNullOrEmpty(culture))
                culture = SessionParameters.Culture;
            Configuration configuration;
            if (!Id.HasValue) configuration = new Configuration { CongressId = Homa.Id, HasArticle = true, HasHotel = true, HasWorkShop = true };
            else
            {
                configuration =
                    CongressComponent.Instance.BaseInfoComponents.ConfigurationFacade.GetLanuageContent(culture, Id);
                configuration.TerminalPassword = string.Empty;
                configuration.SMSAccountPassword = string.Empty;
                configuration.MailPassword = string.Empty;
                configuration.CertificatePassword = string.Empty;
                configuration.MerchantPrivateKey = string.Empty;
                configuration.MerchantPublicKey = string.Empty;
            }
            return PartialView("PVModify", configuration);
        }



        private void GetValue()
        {
            ViewBag.ThemeUrl = CongressComponent.Instance.BaseInfoComponents.ConfigurationFacade.SelectFirstOrDefault(c => c.ThemeColorURL, c => c.CongressId == this.Homa.Id);
            ViewBag.IntroPages = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongressContentFacade.SelectKeyValuePair(x => x.ContentId, x => x.Content.Title, x => x.CongressId == this.Homa.Id), "Key", "Value");
            ViewBag.SupportType = new SelectList(CongressComponent.Instance.BaseInfoComponents.SupportTypeFacade.SelectKeyValuePair(x => x.Id, x => x.Title), "Key", "Value");
            ViewBag.Themes = new SelectList(Web.Mvc.UI.Theme.ThemeManager.ThemeList(false));
            ViewBag.Slides =
                new SelectList(
                    CongressComponent.Instance.BaseInfoComponents.CongessSlideFacade.SelectKeyValuePair(x => x.SlideId,
                        x => x.Slide.Title, news => news.CongressId == Homa.Id), "Key", "Value");
            ViewBag.Languages = new SelectList(CommonComponent.Instance.LanguageFacade.SelectKeyValuePair(x => x.Id, x => x.DisplayName), "Key", "Value");
            ViewBag.ArticlePaymentSteps = new SelectList(
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.ArticlePaymentSteps>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.ArticlePaymentSteps>(),
                            keyValuePair.Value)), "Key", "Value");
            ViewBag.Banks = new SelectList(
                EnumUtils.ConvertEnumToIEnumerable<Radyn.PaymentGateway.Tools.Enums.Bank>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Radyn.PaymentGateway.Tools.Enums.Bank>(),
                            keyValuePair.Value)), "Key", "Value");
            ViewBag.InformTypes = new SelectList(
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.UserInformType>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.UserInformType>(),
                            keyValuePair.Value)), "Key", "Value");
            ViewBag.ArticleCertificateTypes = new SelectList(
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.ArticleCertificateType>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.ArticleCertificateType>(),
                            keyValuePair.Value)), "Key", "Value");
            ViewBag.Sections =
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.PaymentSection>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.PaymentSection>(),
                            keyValuePair.Value)).ToList();

            ViewBag.ArticleCertificateStatList =
                new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.FinalState>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.FinalState>(),
                            keyValuePair.Value)).ToList(), "Key", "Value");

          
            ViewBag.Contrainers = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongressContainerFacade.SelectKeyValuePair(x => x.ContainerId, x => x.Container.Title, x => x.CongressId == this.Homa.Id,new OrderByModel<CongressContainer>(){Expression = x=>x.Container.Title}), "Key", "Value");
            ViewBag.Htmls = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongressHtmlFacade.SelectKeyValuePair(x => x.HtmlDesginId, x => x.HtmlDesgin.Title, x => x.CongressId == this.Homa.Id,new OrderByModel<CongressHtml>(){Expression = x=>x.HtmlDesgin.Title}), "Key", "Value");
            ViewBag.MenuHtmls = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongressMenuHtmlFacade.SelectKeyValuePair(x => x.MenuHtmlId, x => x.MenuHtml.Title, x => x.CongressId == this.Homa.Id), "Key", "Value");
        }


        public JsonResult FillThemeColorURL(string folder)
        {
            var themList = new List<object>();
            try
            {
                IEnumerable<string> allThemes;
                var obj = new Object();
               
                var path = $"/App_Themes/{folder}/CSS/Color";

                allThemes = Directory.EnumerateFiles(Server.MapPath(path), "*.css");
                foreach (var file in allThemes)
                {
                    obj = new
                    {
                        Name = Path.GetFileNameWithoutExtension(file),
                        Url = $"{path}/{Path.GetFileName(file)}"
                    };
                    themList.Add(obj);
                }

                return Json(themList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(themList, JsonRequestBehavior.AllowGet);
            }
        }



        [RadynAuthorize]
        public ActionResult GetConfiguration()
        {

            var config = (Homa != null && Homa.Configuration != null && Homa.Configuration.CongressId != Guid.Empty) ? Homa.Configuration : null;
            return config != null
                ? Redirect("~/Congress/Configuration/Edit?congressId=" + config.CongressId)
                : Redirect("~/Congress/Configuration/Create");

        }

        [RadynAuthorize]
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var configuration = new Configuration();
            var configurationContent = new ConfigurationContent();
            try
            {

                this.RadynTryUpdateModel(configuration);
                this.RadynTryUpdateModel(configurationContent);
                HttpPostedFileBase FavIcon = null;
                if (Session["UpFileFavIcon"] != null)
                {
                    FavIcon = (HttpPostedFileBase)Session["UpFileFavIcon"];
                    Session.Remove("UpFileFavIcon");
                }
                HttpPostedFileBase AttachRefereeFileId = null;
                if (Session["UploadAttachRefereeFileId"] != null)
                {
                    AttachRefereeFileId = (HttpPostedFileBase)Session["UploadAttachRefereeFileId"];
                    Session.Remove("UploadAttachRefereeFileId");
                }
                HttpPostedFileBase BoothMapAttachmentId = null;
                if (Session["UploadBoothMapAttachmentId"] != null)
                {
                    BoothMapAttachmentId = (HttpPostedFileBase)Session["UploadBoothMapAttachmentId"];
                    Session.Remove("UploadBoothMapAttachmentId");
                }
                HttpPostedFileBase LogoId = null;
                if (Session["UploadLogoId"] != null)
                {
                    LogoId = (HttpPostedFileBase)Session["UploadLogoId"];
                    Session.Remove("UploadLogoId");
                }
                HttpPostedFileBase OrgianalposterId = null;
                if (Session["UploadOrgianalposterId"] != null)
                {
                    OrgianalposterId = (HttpPostedFileBase)Session["UploadOrgianalposterId"];
                    Session.Remove("UploadOrgianalposterId");
                }
                HttpPostedFileBase PosterId = null;
                if (Session["UploadPosterId"] != null)
                {
                    PosterId = (HttpPostedFileBase)Session["UploadPosterId"];
                    Session.Remove("UploadPosterId");
                }
                HttpPostedFileBase HeaderId = null;
                if (Session["UploadHeaderId"] != null)
                {
                    HeaderId = (HttpPostedFileBase)Session["UploadHeaderId"];
                    Session.Remove("UploadHeaderId");
                }
                HttpPostedFileBase FooterId = null;
                if (Session["UploadFooterId"] != null)
                {
                    FooterId = (HttpPostedFileBase)Session["UploadFooterId"];
                    Session.Remove("UploadFooterId");
                }
                HttpPostedFileBase hallMapId = null;
                if (Session["HallMapId"] != null)
                {
                    hallMapId = (HttpPostedFileBase)Session["HallMapId"];
                    Session.Remove("HallMapId");
                }
                HttpPostedFileBase uploadBackgroundImageId = null;
                if (Session["UploadBackgroundImageId"] != null)
                {
                    uploadBackgroundImageId = (HttpPostedFileBase)Session["UploadBackgroundImageId"];
                    Session.Remove("UploadBackgroundImageId");
                }

                var sectiontypes = Payment.Tools.AppExtentions.FillDiscountTypeSection(collection, AppExtention.CongressMoudelName);
                configuration.PaymentType = Payment.Tools.AppExtentions.FillPaymentTypes(collection);
                configurationContent.LanguageId = collection["LanguageId"];
                configuration.CurrentUICultureName = collection["MultiLanguageConfId"];
                configuration.CongressId = Homa.ConfigurationId;
                configuration.ArticleCertificateState = string.IsNullOrEmpty(collection["ArticleCertificateState"]) ? (byte?)null : collection["ArticleCertificateState"].ToByte();
                if (CongressComponent.Instance.BaseInfoComponents.ConfigurationFacade.Insert(configuration, configurationContent, AttachRefereeFileId, BoothMapAttachmentId, OrgianalposterId, PosterId, LogoId, HeaderId, FooterId, hallMapId, uploadBackgroundImageId, FavIcon, sectiontypes))
                {
                    SessionParameters.CurrentCongress = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(configuration.CongressId);
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/Configuration/GetConfiguration");

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Configuration/GetConfiguration");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(configuration);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid congressId)
        {

            ViewBag.Id = congressId;
            return View();
        }


        [HttpPost]
        public ActionResult Edit(Guid congressId, FormCollection collection)
        {
            var configuration = CongressComponent.Instance.BaseInfoComponents.ConfigurationFacade.Get(congressId);
            try
            {
                var configurationContent = CongressComponent.Instance.BaseInfoComponents.ConfigurationContentFacade.Get(congressId, collection["LanguageId"]);
                var contentcontent = configurationContent ?? new ConfigurationContent() { CurrentUICultureName = collection["LanguageId"] };
                this.RadynTryUpdateModel(configuration);
                this.RadynTryUpdateModel(contentcontent);
                HttpPostedFileBase FavIcon = null;
                if (Session["UpFileFavIcon"] != null)
                {
                    FavIcon = (HttpPostedFileBase)Session["UpFileFavIcon"];
                    Session.Remove("UpFileFavIcon");
                }
                HttpPostedFileBase AttachRefereeFileId = null;
                if (Session["UploadAttachRefereeFileId"] != null)
                {
                    AttachRefereeFileId = (HttpPostedFileBase)Session["UploadAttachRefereeFileId"];
                    Session.Remove("UploadAttachRefereeFileId");
                }
                HttpPostedFileBase BoothMapAttachmentId = null;
                if (Session["UploadBoothMapAttachmentId"] != null)
                {
                    BoothMapAttachmentId = (HttpPostedFileBase)Session["UploadBoothMapAttachmentId"];
                    Session.Remove("UploadBoothMapAttachmentId");
                }
                HttpPostedFileBase LogoId = null;
                if (Session["UploadLogoId"] != null)
                {
                    LogoId = (HttpPostedFileBase)Session["UploadLogoId"];
                    Session.Remove("UploadLogoId");
                }
                HttpPostedFileBase OrgianalposterId = null;
                if (Session["UploadOrgianalposterId"] != null)
                {
                    OrgianalposterId = (HttpPostedFileBase)Session["UploadOrgianalposterId"];
                    Session.Remove("UploadOrgianalposterId");
                }
                HttpPostedFileBase PosterId = null;
                if (Session["UploadPosterId"] != null)
                {
                    PosterId = (HttpPostedFileBase)Session["UploadPosterId"];
                    Session.Remove("UploadPosterId");
                }
                HttpPostedFileBase HeaderId = null;
                if (Session["UploadHeaderId"] != null)
                {
                    HeaderId = (HttpPostedFileBase)Session["UploadHeaderId"];
                    Session.Remove("UploadHeaderId");
                }
                HttpPostedFileBase FooterId = null;
                if (Session["UploadFooterId"] != null)
                {
                    FooterId = (HttpPostedFileBase)Session["UploadFooterId"];
                    Session.Remove("UploadFooterId");
                }
                HttpPostedFileBase hallMapId = null;
                if (Session["HallMapId"] != null)
                {
                    hallMapId = (HttpPostedFileBase)Session["HallMapId"];
                    Session.Remove("HallMapId");
                }
                HttpPostedFileBase uploadBackgroundImageId = null;
                if (Session["UploadBackgroundImageId"] != null)
                {
                    uploadBackgroundImageId = (HttpPostedFileBase)Session["UploadBackgroundImageId"];
                    Session.Remove("UploadBackgroundImageId");
                }


                var sectiontypes = Payment.Tools.AppExtentions.FillDiscountTypeSection(collection, AppExtention.CongressMoudelName);
                configuration.PaymentType = Payment.Tools.AppExtentions.FillPaymentTypes(collection);
                configuration.CurrentUICultureName = collection["MultiLanguageConfId"];
                configuration.ArticleCertificateState = string.IsNullOrEmpty(collection["ArticleCertificateState"])?(byte?) null: collection["ArticleCertificateState"].ToByte();
                if (CongressComponent.Instance.BaseInfoComponents.ConfigurationFacade.Update(configuration, contentcontent, AttachRefereeFileId, BoothMapAttachmentId, OrgianalposterId, PosterId, LogoId, HeaderId, FooterId, hallMapId, uploadBackgroundImageId, FavIcon, AppExtention.CongressMoudelName, sectiontypes))
                {
                    SessionParameters.CurrentCongress = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(SessionParameters.CurrentCongress.Id);
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/Configuration/GetConfiguration");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Configuration/GetConfiguration");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = congressId;
                return View(configuration);
            }
        }


        public ActionResult ShowImage(Guid Id)
        {

            ViewBag.PhotoId = Id;
            return View();
        }




        public ActionResult GetMultilanguageItems(Guid? Id, string culture)
        {

            var configuration = Id.HasValue ? CongressComponent.Instance.BaseInfoComponents.ConfigurationFacade.GetLanuageContent(culture, Id) : new Configuration();
            return View("Edit", configuration);
        }

        public JsonResult ChangeLanguage(Guid congressId, string langid)
        {
            var keyList = new List<string>()
            {
                "CartTypeEmptyValue",
                "ArticleTitle",
            };


            var obj = new Object();
            var list = new List<object>();

            if (string.IsNullOrEmpty(langid)) return null;
            foreach (var i in keyList)
            {
                var contenet = CommonComponent.Instance.LanguageContentFacade.Get("Congress.Configuration." + congressId + "." + i + "", langid);
                if (contenet != null)
                    obj = new
                    { value = contenet.Value != string.Empty || contenet.Value != null ? contenet.Value : string.Empty };
                else
                    obj = new { value = "" };



                list.Add(obj);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}