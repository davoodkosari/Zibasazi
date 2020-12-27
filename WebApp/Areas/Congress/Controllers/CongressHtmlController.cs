using System;
using System.Linq;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.Web.Mvc.UI.Theme;
using Radyn.WebApp.AppCode.Filter;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressHtmlController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.CongressHtmlFacade.Select(x=>x.HtmlDesgin,x=>x.CongressId==this.Homa.Id&&x.HtmlDesgin.IsExternal);
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
            return View(new HtmlDesgin { Enabled = true });
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var htmlDesgin = new HtmlDesgin();
            try
            {
                this.RadynTryUpdateModel(htmlDesgin, collection);
                htmlDesgin.Enabled = true;
                htmlDesgin.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.CongressHtmlFacade.Insert(this.Homa.Id, htmlDesgin))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);

                    return Redirect("~/Congress/CongressHtml/Index");

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);

                return Redirect("~/Congress/CongressHtml/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(htmlDesgin);
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
            var htmlDesgin = ContentManagerComponent.Instance.HtmlDesginFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(htmlDesgin, collection);
               htmlDesgin.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.CongressHtmlFacade.Update(this.Homa.Id, htmlDesgin))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressHtml/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(htmlDesgin);
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
            var htmlDesgin = ContentManagerComponent.Instance.HtmlDesginFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.CongressHtmlFacade.Delete(this.Homa.Id, Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressHtml/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressHtml/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(htmlDesgin);
            }
        }
    }
}