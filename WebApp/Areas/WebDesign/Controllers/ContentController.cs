using System;
using System.Linq;
using System.Web.Mvc;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.WebDesign.Security.Filter;
using Radyn.WebDesign;

namespace Radyn.WebApp.Areas.WebDesign.Controllers
{
    public class ContentController : WebDesignBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = WebDesignComponent.Instance.ContentFacade.GetByWebSiteId(this.WebSite.Id,false);
            return View(list);
        }
        [RadynAuthorize]
        public ActionResult Details(Int32 Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        [RadynAuthorize]
        public ActionResult Create()
        {
            TempData["Containers"] =
                 new SelectList(
                     WebDesignComponent.Instance.ContainerFacade.GetByWebSiteId(this.WebSite.Id),
                     "Id", "Title");
            return View();
        }
        [RadynAuthorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var content = new Content();

            try
            {
                var contentcontent = new ContentContent();
                this.RadynTryUpdateModel(content, collection);
                this.RadynTryUpdateModel(contentcontent, collection);
                if (WebDesignComponent.Instance.ContentFacade.Insert(this.WebSite.Id, content, contentcontent))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
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
                TempData["Containers"] =
                 new SelectList(
                     WebDesignComponent.Instance.ContainerFacade.GetByWebSiteId(this.WebSite.Id),
                     "Id", "Title");
                return View(content);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Int32 id)
        {
            TempData["Containers"] =
                new SelectList(
                    WebDesignComponent.Instance.ContainerFacade.GetByWebSiteId(this.WebSite.Id),
                    "Id", "Title");
            ViewBag.Id = id;
            return View();
        }

        [RadynAuthorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Int32 id, FormCollection collection)
        {
            var content = ContentManagerComponent.Instance.ContentFacade.Get(id);
            try
            {
                var contentcontent = ContentManagerComponent.Instance.ContentContentFacade.Get(id, collection["LanguageId"]) ??
                new ContentContent();
                this.RadynTryUpdateModel(content, collection);
                this.RadynTryUpdateModel(contentcontent, collection);
                if (ContentManagerComponent.Instance.ContentFacade.Update(content, contentcontent))
                {
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
                TempData["Containers"] =
                 new SelectList(
                     WebDesignComponent.Instance.ContainerFacade.GetByWebSiteId(this.WebSite.Id),
                     "Id", "Title");
                ViewBag.Id = id;
                return View(content);
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
            var content = ContentManagerComponent.Instance.ContentFacade.Get(id);
            try
            {
                if (WebDesignComponent.Instance.ContentFacade.Delete(this.WebSite.Id, id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInDelete + exception.Message, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                ViewBag.Id = id;
                return View(content);
            }
        }

    }
}
