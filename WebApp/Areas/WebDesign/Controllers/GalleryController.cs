using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Common.Component;
using Radyn.Gallery;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.WebDesign.Security.Filter;
using Radyn.WebDesign;

namespace Radyn.WebApp.Areas.WebDesign.Controllers
{
    public class GalleryController : WebDesignBaseController
    {
        [RadynAuthorize]
        public ActionResult Index(Guid? parentId)
        {
            var congessGalleries = WebDesignComponent.Instance.GalleryFacade.Select(c => c.WebSiteGallery,
                c => c.WebId == this.WebSite.Id);
            var list = parentId.HasValue ?
            congessGalleries.Where(gallery => gallery.ParentGallery == parentId)
           : congessGalleries.Where(gallery => gallery.ParentGallery == null);
            return View(list);
        }
        public ActionResult Details(Guid id)
        {
            ViewBag.Id = id;
            return View();
        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            TempData["ParentGallery"] = new SelectList(WebDesignComponent.Instance.GalleryFacade.GetParents(this.WebSite.Id), "Id", "Title");
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Create(FormCollection collection)
        {
            var gallery = new Radyn.Gallery.DataStructure.Gallery();

            try
            {
                this.RadynTryUpdateModel(gallery);
                HttpPostedFileBase image = null;
                gallery.CurrentUICultureName = collection["LanguageId"];
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                if (WebDesignComponent.Instance.GalleryFacade.Insert(this.WebSite.Id, gallery, image))
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
                TempData["ParentGallery"] = new SelectList(WebDesignComponent.Instance.GalleryFacade.GetParents(this.WebSite.Id), "Id", "Title");
                return View(gallery);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid id)
        {

            ViewBag.Id = id;
            TempData["ParentGallery"] = new SelectList(WebDesignComponent.Instance.GalleryFacade.GetParents(this.WebSite.Id), "Id", "Title");
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Edit(Guid id, FormCollection collection)
        {

            var gallery = GalleryComponent.Instance.GalleryFacade.Get(id);
            try
            {
                this.RadynTryUpdateModel(gallery, collection);
                gallery.CurrentUICultureName = collection["LanguageId"];
                if (string.IsNullOrEmpty(gallery.CreateDate.Trim()))
                    gallery.CreateDate = Utility.DateTimeUtil.ShamsiDate(DateTime.Now);
                HttpPostedFileBase image = null;
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                if (GalleryComponent.Instance.GalleryFacade.Update(gallery, image))
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
                ViewBag.Id = id;
                TempData["ParentGallery"] = new SelectList(WebDesignComponent.Instance.GalleryFacade.GetParents(this.WebSite.Id), "Id", "Title");
                return View(gallery);
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
                if (WebDesignComponent.Instance.GalleryFacade.Delete(this.WebSite.Id, id))
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
        public ActionResult GalleryList()
        {
            var congessGalleries = WebDesignComponent.Instance.GalleryFacade.Select(x => x.WebSiteGallery, x => x.WebId == this.WebSite.Id && x.WebSiteGallery.ParentGallery == null).OrderByDescending(x => x.CreateDate);

            if (congessGalleries.Count() == 1)
            {
                return RedirectToAction("GalleryPhotos", "Photo", new { Area = "Gallery", galleyId = congessGalleries.FirstOrDefault().Id });
            }
            
            return View(congessGalleries.ToList());
        }

    }
}
