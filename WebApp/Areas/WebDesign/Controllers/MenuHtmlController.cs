using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.Web.Mvc.UI.Theme;
using Radyn.WebApp.AppCode.Filter;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.WebDesign.Security.Filter;
using Radyn.WebDesign;
using AppExtentions = Radyn.WebApp.Areas.ContentManager.Tools.AppExtentions;

namespace Radyn.WebApp.Areas.WebDesign.Controllers
{
    public class MenuHtmlController : WebDesignBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = WebDesignComponent.Instance.MenuHtmlFacade.Select(x=>x.WebSiteMenuHtml,x=>x.WebSiteId==this.WebSite.Id&&x.WebSiteMenuHtml.IsExternal);
            return View(list);
        }

      
       
        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        [RadynAuthorize]
        [SourceCodeFile("ImageBrowser Controller", "~/Controllers/ImageBrowserController.cs")]
        public ActionResult Create()
        {
            return View(new MenuHtml { Enabled = true });
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var MenuHtml = new MenuHtml();
            try
            {
                this.RadynTryUpdateModel(MenuHtml, collection);
                MenuHtml.CurrentUICultureName = collection["LanguageId"];
                MenuHtml.Enabled = true;
                if (WebDesignComponent.Instance.MenuHtmlFacade.Insert(this.WebSite.Id, MenuHtml))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = MenuHtml.Id });

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);

                return Redirect("~/WebDesign/MenuHtml/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(MenuHtml);
            }
        }

        [RadynAuthorize]
        [SourceCodeFile("ImageBrowser Controller", "~/Controllers/ImageBrowserController.cs")]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var MenuHtml = ContentManagerComponent.Instance.MenuHtmlFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(MenuHtml, collection);
               MenuHtml.CurrentUICultureName = collection["LanguageId"];
                if (ContentManagerComponent.Instance.MenuHtmlFacade.Update(MenuHtml))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/WebDesign/MenuHtml/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(MenuHtml);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            var MenuHtml = ContentManagerComponent.Instance.MenuHtmlFacade.Get(Id);
            try
            {
                if (WebDesignComponent.Instance.MenuHtmlFacade.Delete(this.WebSite.Id, Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/WebDesign/MenuHtml/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/WebDesign/MenuHtml/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(MenuHtml);
            }
        }
    }
}