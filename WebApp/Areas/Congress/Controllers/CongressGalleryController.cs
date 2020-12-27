using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Gallery;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.Web.Mvc.Utility;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using AppExtentions = Radyn.WebApp.Areas.ContentManager.Tools.AppExtentions;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressGalleryController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index(Guid? parentId)
        {

            var predicateBuilder=new PredicateBuilder<CongessGallery>();
            predicateBuilder.And(x=>x.CongressId==this.Homa.Id);
            if(parentId.HasValue)
                predicateBuilder.And(x=>x.Gallery.ParentGallery==parentId);
            else predicateBuilder.And(x=>x.Gallery.ParentGallery==null);
            var list = CongressComponent.Instance.BaseInfoComponents.CongessGalleryFacade.Select(x=>x.Gallery, predicateBuilder.GetExpression(),new OrderByModel<CongessGallery>(){Expression = x=>x.Gallery.Order});
           
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
            TempData["ParentGallery"] = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongessGalleryFacade.GetParents(this.Homa.Id), "Key", "Value");
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
                gallery.CreateDate = DateTime.Now.ShamsiDate();
                HttpPostedFileBase image = null;
               gallery.CurrentUICultureName = collection["LanguageId"];
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                List<HttpPostedFileBase> fileBases = null;

                if (Session["PhotoList"] != null)
                {
                    fileBases = (List<HttpPostedFileBase>)Session["PhotoList"];
                    Session.Remove("PhotoList");
                }
                if (CongressComponent.Instance.BaseInfoComponents.CongessGalleryFacade.Insert(this.Homa.Id, gallery, image, fileBases))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = gallery.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressGallery/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                TempData["ParentGallery"] = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongessGalleryFacade.GetParents(this.Homa.Id), "Key", "Value");
                return View(gallery);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid id)
        {
            TempData["ParentGallery"] = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongessGalleryFacade.GetParents(this.Homa.Id), "Key", "Value");
            ViewBag.Id = id;
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
               HttpPostedFileBase image = null;
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                gallery.CurrentUICultureName = collection["LanguageId"];
                List<HttpPostedFileBase> fileBases = null;

                if (Session["PhotoList"] != null)
                {
                    fileBases = (List<HttpPostedFileBase>)Session["PhotoList"];
                    Session.Remove("PhotoList");
                }
                if (GalleryComponent.Instance.GalleryFacade.Update(gallery, image, fileBases))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressGallery/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = id;
                TempData["ParentGallery"] = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongessGalleryFacade.GetParents(this.Homa.Id), "Key", "Value");
                return View(gallery);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid id)
        {
            ViewBag.Id = id;
            ViewBag.PhotoCount = GalleryComponent.Instance.PhotoFacade.Count(x => x.GalleryId == id);
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            var gallery = GalleryComponent.Instance.GalleryFacade.Get(id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.CongessGalleryFacade.Delete(this.Homa.Id, id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressGallery/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressGallery/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = id;
                return View(gallery);
            }
        }
        public ActionResult GalleryList()
        {
            var congessGalleries = CongressComponent.Instance.BaseInfoComponents.CongessGalleryFacade.Select(x=>x.Gallery,news => news.CongressId == this.Homa.Id&&news.Gallery.Enabled,new OrderByModel<CongessGallery>(){Expression = x=>x.Gallery.Order});
            if (congessGalleries.Count() == 1)
            {
                return RedirectToAction("GalleryPhotos", "Photo", new { Area = "Gallery", galleyId = congessGalleries.FirstOrDefault().Id });
            }
            return View();
        }
        public ActionResult GetGalleryList()
        {
            var congessGalleries =
                CongressComponent.Instance.BaseInfoComponents.CongessGalleryFacade.Select(new Expression<Func<CongessGallery, object>>[] {x=>x.Gallery.Thumbnail,x=>x.Gallery.Title,x=>x.Gallery.Id,x=>x.Gallery.Order},news => news.CongressId == this.Homa.Id&&news.Gallery.Enabled,new OrderByModel<CongessGallery>() { Expression = x => x.Gallery.Order });
            return PartialView("PVGalleryList", congessGalleries);
        }
        public ActionResult PrepareGalleryList()
        {
            var actionUrl = this.RadynRenderActionUrl("/Congress/CongressGallery/GetGalleryList");
            var str=AppExtentions.GetHtmlByUrl( actionUrl, Resources.Gallery1.Galleryname, SessionParameters.CurrentCongress.Configuration.Container);
            return PartialView("PVShowGalleryList", str);
        }
    }
}
