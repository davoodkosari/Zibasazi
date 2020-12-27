using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Radyn.Common;
using Radyn.Common.DataStructure;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;

namespace Radyn.WebApp.Areas.Common.Controllers
{
    public class LanguageController : LocalizedController
    {
        public ActionResult GetDetail(string Id)
        {
            return PartialView("PVDetails", CommonComponent.Instance.LanguageFacade.Get(Id));
        }
        public ActionResult GetModify(string Id)
        {
            return PartialView("PVModify", !string.IsNullOrEmpty(Id) ? CommonComponent.Instance.LanguageFacade.Get(Id) : new Language());
        }

        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CommonComponent.Instance.LanguageFacade.GetAll();
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(string Id)
        {
            ViewBag.Id = Id;
            return View();

        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Create(FormCollection collection)
        {
            var language = new Language();

            try
            {
                this.RadynTryUpdateModel(language, collection);

                HttpPostedFileBase image = null;
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }

                if (CommonComponent.Instance.LanguageFacade.Insert(language, image))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(language);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Edit(string Id, FormCollection collection)
        {
            var language = CommonComponent.Instance.LanguageFacade.Get(Id);

            try
            {

                this.RadynTryUpdateModel(language, collection);
                HttpPostedFileBase image = null;
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }

                if (CommonComponent.Instance.LanguageFacade.Update(language, image))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
               ShowExceptionMessage(exception);
                return View();
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Delete(string Id, FormCollection collection)
        {


            try
            {
                if (CommonComponent.Instance.LanguageFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }

      
        public ActionResult CultureBar()
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.CongressLanguageFacade.Select(
                    new Expression<Func<CongressLanguage, object>>[]
                        {x =>x.Language.Id,x=> x.Language.ShowLogo, x => x.Language.LogoId, x => x.Language.DisplayName},
                    x => x.CongressId == SessionParameters.CurrentCongress.Id && x.Language.Enabled);
            return PartialView("LanguageBar", list);
        }

        public ActionResult SetLanguage(string culture)
        {
            SessionParameters.Culture = culture;
            return Redirect(HttpContext.Request.UrlReferrer.ToString());
        }
    }
}
