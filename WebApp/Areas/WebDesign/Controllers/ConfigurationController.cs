using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Slider;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.WebDesign.Security.Filter;
using Radyn.WebApp.Areas.WebDesign.Tools;
using Radyn.WebDesign;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Definition;


namespace Radyn.WebApp.Areas.WebDesign.Controllers
{
    public class ConfigurationController : WebDesignBaseController
    {
       
        [RadynAuthorize]
        [WebDesignHost]
        public ActionResult GetConfiguration()
        {

            var config = (WebSite != null && WebSite.Configuration != null && WebSite.Configuration.Id != Guid.Empty) ? WebSite.Configuration : null;
            return config != null
                ? Redirect("~/WebDesign/Configuration/Edit?Id=" + config.Id)
                : Redirect("~/WebDesign/Configuration/Create");


          
        }
        public ActionResult GetModify(Guid? Id)
        {
            ViewBag.Slides = new SelectList(WebDesignComponent.Instance.SliderFacade.SelectKeyValuePair(x => x.WebSiteSlide.Id, x => x.WebSiteSlide.Title, x => x.WebId == this.WebSite.Id), "Key", "Value");
            ViewBag.Contents = new SelectList(WebDesignComponent.Instance.ContentFacade.GetByWebSiteId(this.WebSite.Id, true), "Id", "Title");
            ViewBag.Container =new SelectList(WebDesignComponent.Instance.ContainerFacade.SelectKeyValuePair(c => c.WebSiteContainer.Id,c => c.WebSiteContainer.Title, c => c.WebId == this.WebSite.Id), "Key", "Value");
            ViewBag.Html =new SelectList(WebDesignComponent.Instance.HtmlFacade.SelectKeyValuePair(c => c.HtmlDesgin.Id,c => c.HtmlDesgin.Title, c => c.WebId ==this.WebSite.Id), "Key", "Value");
            ViewBag.MenuHtmls = new SelectList(WebDesignComponent.Instance.MenuHtmlFacade.SelectKeyValuePair(x => x.MenuHtmlId, x => x.WebSiteMenuHtml.Title, x => x.WebSiteId == this.WebSite.Id), "Key", "Value");
            return PartialView("PVModify", Id.HasValue ? WebDesignComponent.Instance.ConfigurationFacade.Get(Id) : new Configuration());
        }

        [RadynAuthorize]
        [WebDesignHost]
        public ActionResult Create()
        {

            if (this.WebSite == null)
                return RedirectToAction("Index", "WebSite", new { area = "WebDesign" });
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var configuration = new Configuration();

            try
            {
                this.RadynTryUpdateModel(configuration);
                HttpPostedFileBase FavIcon = null;
                if (Session["UpFileFavIcon"] != null)
                {
                    FavIcon = (HttpPostedFileBase)Session["UpFileFavIcon"];
                    
                }
                if (this.WebSite == null)
                    return RedirectToAction("Index", "WebSite", new { area = "WebDesign" });
                configuration.Id = this.WebSite.ConfigurationId;
                if (WebDesignComponent.Instance.ConfigurationFacade.Insert(configuration, FavIcon))
                {
                    Session.Remove("UpFileFavIcon");
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    SessionParameters.CurrentWebSite = null;
                    return RedirectToAction("GetConfiguration");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return RedirectToAction("GetConfiguration");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInInsert + exception.Message, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);

                return View(configuration);
            }
        }

        [RadynAuthorize]
        [WebDesignHost]
        public ActionResult Edit(Guid Id)
        {


            if (this.WebSite == null)
                return RedirectToAction("Index", "WebSite", new { area = "WebDesign" });
            ViewBag.Id = this.WebSite.Id;
            return View();
        }


        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var configuration = WebDesignComponent.Instance.ConfigurationFacade.Get(Id);
            try
            {


                this.RadynTryUpdateModel(configuration);
                HttpPostedFileBase FavIcon = null;
                if (Session["UpFileFavIcon"] != null)
                {
                    FavIcon = (HttpPostedFileBase)Session["UpFileFavIcon"];
                    
                }
                if (WebDesignComponent.Instance.ConfigurationFacade.Update(configuration, FavIcon))
                {
                    Session.Remove("UpFileFavIcon");
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    SessionParameters.CurrentWebSite = null;
                    return RedirectToAction("GetConfiguration");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("GetConfiguration");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInEdit + exception.Message, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                ViewBag.Id = Id;
                return View(configuration);
            }
        }





    }
}