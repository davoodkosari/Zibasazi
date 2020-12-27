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
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Resource = Radyn.Congress.DataStructure.Resource;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class ResourceController : CongressBaseController
    {

        public ActionResult GetDetails(Guid Id)
        {
            var resource = CongressComponent.Instance.BaseInfoComponents.ResourceFacade.Get(Id);
            ViewBag.UseLayouts = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.UseLayout>().Select(
                keyValuePair =>
                    new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.UseLayout>(),
                        keyValuePair.Value));
            return PartialView("PVDetails", resource);
        }
        public ActionResult GetModify(Guid? Id,string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            ViewBag.Types = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.ResourceType>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.ResourceType>(),
                            keyValuePair.Value)), "Key", "Value");

            ViewBag.UseLayouts = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.UseLayout>().Select(
                keyValuePair =>
                    new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.UseLayout>(),
                        keyValuePair.Value));
            return PartialView("PVModify", Id.HasValue ? CongressComponent.Instance.BaseInfoComponents.ResourceFacade.GetLanuageContent(culture,Id) : new Resource());
        }
        [RadynAuthorize]
        public ActionResult Index()
        {
            var slides = CongressComponent.Instance.BaseInfoComponents.ResourceFacade.Where(congessSlide => congessSlide.CongressId == this.Homa.Id);
            return View(slides);
        }
        public ActionResult Details(Guid id)
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
            var resource = new Radyn.Congress.DataStructure.Resource();

            try
            {
                this.RadynTryUpdateModel(resource);
                HttpPostedFileBase ResourceFile = null;
                if (Session["ResourceFile"] != null)
                {
                    ResourceFile = (HttpPostedFileBase)Session["ResourceFile"];
                    Session.Remove("ResourceFile");
                }
                resource.CongressId = this.Homa.Id;
                resource.CurrentUICultureName = collection["LanguageId"];
                UpdateLayoutUse(collection, resource);
                if (CongressComponent.Instance.BaseInfoComponents.ResourceFacade.Insert(resource, ResourceFile))
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
                return View();
            }
        }

        private static void UpdateLayoutUse(FormCollection collection, Resource resource)
        {
            var str = string.Empty;
            foreach (var variable in collection.AllKeys.Where(x => x.StartsWith("SelectType-")))
            {
                var key = variable.Substring(11, variable.Length - 11);
                if (!string.IsNullOrEmpty(str)) str += ",";
                str += key;
            }

            resource.UseLayoutId = str;
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

            var resource = CongressComponent.Instance.BaseInfoComponents.ResourceFacade.Get(id);
            try
            {
                this.RadynTryUpdateModel(resource, collection);
               HttpPostedFileBase resourceFile = null;
                if (Session["ResourceFile"] != null)
                {
                    resourceFile = (HttpPostedFileBase)Session["ResourceFile"];
                    Session.Remove("ResourceFile");
                }
                resource.CurrentUICultureName = collection["LanguageId"];
                UpdateLayoutUse(collection, resource);
                if (CongressComponent.Instance.BaseInfoComponents.ResourceFacade.Update(resource, resourceFile))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    SessionParameters.CurrentCongress = null;
                    return RedirectToAction("Index");

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
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
                if (CongressComponent.Instance.BaseInfoComponents.ResourceFacade.Delete(id))
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
                ViewBag.Id = id;
                return View();
            }
        }
       

       
    }
}
