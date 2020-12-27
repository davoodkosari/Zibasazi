using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Common;
using Radyn.Slider;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.WebDesign.Security.Filter;
using Radyn.WebDesign;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Definition;

namespace Radyn.WebApp.Areas.WebDesign.Controllers
{
    public class ResourceController : WebDesignBaseController
    {

        public ActionResult GetDetails(Guid Id)
        {
            var resource = WebDesignComponent.Instance.ResourceFacade.Get(Id);
            return PartialView("PVDetails", resource);
        }
        public ActionResult GetModify(Guid? Id)
        {
            ViewBag.Types = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.ResourceType>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.ResourceType>(),
                            keyValuePair.Value)), "Key", "Value");
            ViewBag.LanguageList = new SelectList(CommonComponent.Instance.LanguageFacade.GetValidList(), "Id", "DisplayName");
            return PartialView("PVModify", Id.HasValue ? WebDesignComponent.Instance.ResourceFacade.Get(Id) : new Resource());
        }
        [RadynAuthorize]
        public ActionResult Index()
        {
            var slides = WebDesignComponent.Instance.ResourceFacade.Where(x => x.WebId == this.WebSite.Id);
            return View(slides);
        }
        public ActionResult Details(short id)
        {
            ViewBag.Id = id;
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
            var resource = new Radyn.WebDesign.DataStructure.Resource();

            try
            {
                this.RadynTryUpdateModel(resource);
                HttpPostedFileBase ResourceFile = null;
                if (Session["ResourceFile"] != null)
                {
                    ResourceFile = (HttpPostedFileBase)Session["ResourceFile"];
                    Session.Remove("ResourceFile");
                }
                resource.WebId = this.WebSite.Id;
                if (WebDesignComponent.Instance.ResourceFacade.Insert(resource, ResourceFile))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInInsert + exception.Message, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return View();
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Edit(Guid id, FormCollection collection)
        {

            var resource = WebDesignComponent.Instance.ResourceFacade.Get(id);
            try
            {
                this.RadynTryUpdateModel(resource, collection);
                HttpPostedFileBase resourceFile = null;
                if (Session["ResourceFile"] != null)
                {
                    resourceFile = (HttpPostedFileBase)Session["ResourceFile"];
                    Session.Remove("ResourceFile");
                }
                //  resource.CurrentUICultureName = collection["LanguageId"];
                if (WebDesignComponent.Instance.ResourceFacade.Update(resource, resourceFile))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    SessionParameters.CurrentWebSite = null;
                    return RedirectToAction("Index");

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInEdit + exception.Message, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                ViewBag.Id = id;
                return View();
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            try
            {
                if (WebDesignComponent.Instance.ResourceFacade.Delete(id))
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
                return View();
            }
        }
        public ActionResult RemoveImage()
        {
            Session.Remove("ResourceFile");
            return Content("");
        }
        public ActionResult UploadImage(IEnumerable<HttpPostedFileBase> fileBase)
        {
            HttpPostedFileBase file = Request.Files["UploadResourceFile"];
            if (file != null)
            {
                if (file.InputStream != null)
                {

                    Session["ResourceFile"] = file;
                }
            }
            return Content("");
        }


    }
}
