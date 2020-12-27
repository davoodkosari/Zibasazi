using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Slider;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.Congress.Tools;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressSlideController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var slides = CongressComponent.Instance.BaseInfoComponents.CongessSlideFacade.Select(x => x.Slide,
                congessSlide => congessSlide.CongressId == this.Homa.Id);
            return View(slides);
        }
        public ActionResult Details(short id)
        {

            GetValue(id);
            return View();
        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            ViewBag.Sliders = new SelectList(AppExtention.SlideList(), "Key", "Value");
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Create(FormCollection collection)
        {
            var slide = new Radyn.Slider.DataStructure.Slide();

            try
            {
                this.RadynTryUpdateModel(slide);
                var url = collection["Selected"];
                slide.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.CongessSlideFacade.Insert(this.Homa.Id, slide, url))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    if (!string.IsNullOrEmpty(url))
                        SessionParameters.CurrentCongress = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                    return this.SubmitRedirect(collection, new { Id = slide.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressSlide/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Sliders = new SelectList(AppExtention.SlideList(), "Key", "Value");
                return View(slide);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(short id)
        {
            GetValue(id);
            return View();
        }

        private void GetValue(short? id)
        {
            ViewBag.Id = id;
            var str = "";
            if (this.Homa.Configuration.BigSlideId != null && this.Homa.Configuration.BigSlideId == id)
                str = "1";
            if (this.Homa.Configuration.AverageSlideId != null && this.Homa.Configuration.AverageSlideId == id)
                str = "0";
            if (this.Homa.Configuration.MiniSlideId != null && this.Homa.Configuration.MiniSlideId == id)
                str = "-1";
            var list = new List<KeyValuePair<string, string>>();
            list = AppExtention.SlideList();
            list.Insert(0, new KeyValuePair<string, string>(
                    "-2", Resources.Congress.None));
            ViewBag.Sliders = new SelectList(list, "Key", "Value", str);
        }
        [HttpPost]
        [RadynAuthorize]
        public ActionResult Edit(short id, FormCollection collection)
        {

            var slide = SliderComponent.Instance.SlideFacade.Get(id);
            try
            {
                this.RadynTryUpdateModel(slide, collection);
                var url = collection["Selected"];
                slide.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.CongessSlideFacade.Update(this.Homa.Id, slide, url))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    SessionParameters.CurrentCongress = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                    return this.SubmitRedirect(collection, new { Id = id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressSlide/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                GetValue(id);
                return View(slide);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(short id)
        {

            GetValue(id);
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Delete(short id, FormCollection collection)
        {
            var gallery = SliderComponent.Instance.SlideFacade.Get(id);
            var slide = CongressComponent.Instance.BaseInfoComponents.CongessSlideFacade.Get(this.Homa.Id, id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.CongessSlideFacade.Delete(slide))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressSlide/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressSlide/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = id;
                return View(gallery);
            }
        }


    }
}
