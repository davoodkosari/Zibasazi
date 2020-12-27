using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressContentController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {

            var list = CongressComponent.Instance.BaseInfoComponents.CongressContentFacade.Select(x=>x.Content,x=>x.CongressId==this.Homa.Id);
            ContentManagerComponent.Instance.ContentFacade.GetLanuageContent(SessionParameters.Culture, list);
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
            ViewBag.HomaId = this.Homa.Id;
            FillTempData();
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
                if (CongressComponent.Instance.BaseInfoComponents.CongressContentFacade.Insert(this.Homa.Id, content, contentcontent))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = content.Id });

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressContent/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                FillTempData();
                return View(content);
            }
        }

        private void FillTempData()
        {
            var list = new SelectList(
                CongressComponent.Instance.BaseInfoComponents.CongressContainerFacade.SelectKeyValuePair(x => x.ContainerId,
                    x => x.Container.Title, x => x.CongressId == this.Homa.Id).Select(
                    keyValuePair =>
                        new KeyValuePair<Guid, string>(keyValuePair.Key.ToGuid(), keyValuePair.Value)),
                "Key", "Value");
            TempData["Containers"] = list;

        }

        [RadynAuthorize]
        public ActionResult Edit(Int32 id)
        {
            ViewBag.HomaId = this.Homa.Id;
            FillTempData();
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
                new ContentContent() ;
                this.RadynTryUpdateModel(content, collection);
                this.RadynTryUpdateModel(contentcontent, collection);
                contentcontent.LanguageId = collection["LanguageId"];
                if (ContentManagerComponent.Instance.ContentFacade.Update(content, contentcontent))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressContent/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                FillTempData();
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
                if (CongressComponent.Instance.BaseInfoComponents.CongressContentFacade.Delete(this.Homa.Id, id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressContent/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressContent/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = id;
                return View(content);
            }
        }

    }
}
