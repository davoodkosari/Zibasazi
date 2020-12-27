using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebDesign;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Definition;

namespace Radyn.WebApp.Areas.WebDesign.Controllers
{
    public class WebSiteController : LocalizedController
    {
        public ActionResult Error()
        {


            return View();
        }
        public ActionResult GetError()
        {

            var keyValuePair = WebDesign.Tools.AppExtention.WebSitError();
            ViewBag.Error = keyValuePair != null ? keyValuePair.Value.Key : null;
            ViewBag.Photo = keyValuePair != null ? keyValuePair.Value.Value : null;
            return PartialView("PVError");
        }

        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = WebDesignComponent.Instance.WebSiteFacade.GetAll();
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            return View(WebDesignComponent.Instance.WebSiteFacade.Get(Id));
        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            return View(new WebSite { Enabled = true });
        }
        [RadynAuthorize]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var homa = new WebSite();
            try
            {

                this.RadynTryUpdateModel(homa);
                homa.CurrentUICultureName = collection["LanguageId"];
                var list = (List<WebSiteAlias>)Session["WebSiteAliasDataSource"];
                if (WebDesignComponent.Instance.WebSiteFacade.Insert(homa, list))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    SessionParameters.CurrentWebSite = null;
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInInsert + exception.Message, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return View(homa);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            return View(WebDesignComponent.Instance.WebSiteFacade.Get(Id));
        }
        [RadynAuthorize]
        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var webSite = WebDesignComponent.Instance.WebSiteFacade.Get(Id);
            try
            {
                
                this.RadynTryUpdateModel(webSite);
                var list = (List<WebSiteAlias>)Session["WebSiteAliasDataSource"];
                webSite.CurrentUICultureName = collection["LanguageId"];
                if (WebDesignComponent.Instance.WebSiteFacade.Update(webSite, list))
                {
                    SessionParameters.CurrentWebSite = null;
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInEdit + exception.Message, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return View(webSite);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            return View(WebDesignComponent.Instance.WebSiteFacade.Get(Id));
        }
        [RadynAuthorize]
        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            var webSite = WebDesignComponent.Instance.WebSiteFacade.Get(Id);
            try
            {
                if (WebDesignComponent.Instance.WebSiteFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInDelete + exception.Message, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return View(webSite);
            }
        }

    }
}