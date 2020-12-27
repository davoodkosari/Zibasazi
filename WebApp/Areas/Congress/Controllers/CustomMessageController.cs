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
using static Radyn.Congress.Tools.Enums;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CustomMessageController : CongressBaseController
    {
        // GET: Congress/CustomMessage
        public ActionResult Index()
        {
            var result = CongressComponent.Instance.BaseInfoComponents.CustomMessageFacade.Where(x => x.CongressId == this.Homa.Id);
            return View(result);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var obj = new CustomMessage();
            try
            {
                RadynTryUpdateModel(obj, collection);
                obj.CongressId = this.Homa.Id;
                obj.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.CustomMessageFacade.Insert(obj))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CustomMessage/Index");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CustomMessage/Index");

            }
            catch (Exception ex)
            {

                ShowExceptionMessage(ex);
                return View();
            }
        }

        public ActionResult Edit(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int Id, FormCollection collection)
        {
            var obj = CongressComponent.Instance.BaseInfoComponents.CustomMessageFacade.Get(Id);
            try
            {
                RadynTryUpdateModel(obj, collection);
                obj.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.CustomMessageFacade.Update(obj))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CustomMessage/Index");

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CustomMessage/Index");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                ViewBag.Id = Id;
                return View();
            }
        }

        public ActionResult Delete(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int Id, FormCollection collection)
        {
           
            try
            {
                if (!CongressComponent.Instance.BaseInfoComponents.CustomMessageFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,messageIcon: MessageIcon.Error);
                    ViewBag.Id = Id;
                    return View();
                }
                ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,messageIcon: MessageIcon.Succeed);
                return Redirect("~/Congress/CustomMessage/Index");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                ViewBag.Id = Id;
                return View();
            }
        }

        public ActionResult Details(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        public ActionResult GetModify(int? Id, string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            ViewBag.SelectType = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<MessageInformType>(), "Key", "Value");
            CustomMessage customMessage;
            if (Id.HasValue && Id != 0)
                customMessage =CongressComponent.Instance.BaseInfoComponents.CustomMessageFacade.GetLanuageContent(culture, Id);
            else customMessage = new CustomMessage();
            return PartialView("PVModify", customMessage);
        }

        public ActionResult GetDetail(int Id)
        {
            return PartialView("PVDetails", CongressComponent.Instance.BaseInfoComponents.CustomMessageFacade.Get(Id));
        }

        public ActionResult GetMessageKeys(MessageInformType key)
        {
            switch (key)
            {
                case MessageInformType.User:
                    ViewBag.Keys = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<UserMessageKey>(), "Key", "Value");
                    break;
                case MessageInformType.Workshop:
                    ViewBag.Keys = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<WorkshopMessageKey>(), "Key", "Value");
                    break;
                case MessageInformType.Hotel:
                    ViewBag.Keys = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<HotelMessageKey>(), "Key", "Value");
                    break;
                case MessageInformType.Booth:
                    ViewBag.Keys = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<BoothMessageKey>(), "Key", "Value");
                    break;
                case MessageInformType.Article:
                    ViewBag.Keys = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<ArticleMessageKey>(), "Key", "Value");
                    break;
                case MessageInformType.Referee:
                    ViewBag.Keys = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<RefereeMessageKey>(), "Key", "Value");
                    break;
                case MessageInformType.RefereeArticle:
                    ViewBag.Keys = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<RefereeArticleMessageKey>(), "Key", "Value");
                    break;
            }
            return PartialView("PVKeysHelp");
        }
    }
}