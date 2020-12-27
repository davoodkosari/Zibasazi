using System;
using System.Linq;
using System.Web.Mvc;
using Radyn.FormGenerator;
using Radyn.FormGenerator.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.WebDesign.Security.Filter;
using Radyn.WebDesign;

namespace Radyn.WebApp.Areas.WebDesign.Controllers
{
    public class FormsController : WebDesignBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = WebDesignComponent.Instance.FormsFacade.Select(forms => forms.WebSiteForm, x=>x.WebId == this.WebSite.Id);
            return View(list);
        }
        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        [RadynAuthorize]
        public ActionResult Create()
        {
            TempData["Containers"] = new SelectList(WebDesignComponent.Instance.ContainerFacade.GetByWebSiteId(this.WebSite.Id), "Id", "Title");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var formStructure = new FormStructure();
            try
            {
                this.RadynTryUpdateModel(formStructure, collection);
                if (WebDesignComponent.Instance.FormsFacade.Insert(this.WebSite.Id, formStructure))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Edit", new { Id = formStructure.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInInsert + exception.Message, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                TempData["Containers"] =
                new SelectList(
                   WebDesignComponent.Instance.ContainerFacade.GetByWebSiteId(this.WebSite.Id), "Id", "Title");
                return View();
            }
        }


        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            TempData["Containers"] =
      new SelectList(
         WebDesignComponent.Instance.ContainerFacade.GetByWebSiteId(this.WebSite.Id), "Id", "Title");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var formStructure = FormGeneratorComponent.Instance.FormStructureFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(formStructure, collection);
                if (FormGeneratorComponent.Instance.FormStructureFacade.Update(formStructure))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");


            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInEdit + exception.Message, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                ViewBag.Id = Id;
                TempData["Containers"] =
                new SelectList(
                   WebDesignComponent.Instance.ContainerFacade.GetByWebSiteId(this.WebSite.Id), "Id", "Title");
                return View();
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
            try
            {
                if (WebDesignComponent.Instance.FormsFacade.Delete(this.WebSite.Id, Id))
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
                ViewBag.Id = Id;
                return View();
            }
        }
    }
}