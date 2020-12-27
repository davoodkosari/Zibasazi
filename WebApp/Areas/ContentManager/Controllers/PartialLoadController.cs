using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Radyn.ContentManager.Definition;

namespace Radyn.WebApp.Areas.ContentManager.Controllers
{
    public class PartialLoadController : LocalizedController<PartialLoad>
    {
        [RadynAuthorize]
        public ActionResult Index(Guid htmlId)
        {
            ViewBag.htmlId = htmlId;
            return View();
        }
        [RadynAuthorize]
        public ActionResult GetIndex(Guid htmlId)
        {

            var list = ContentManagerComponent.Instance.PartialLoadFacade.OrderBy(x => x.position, x => x.HtmlDesginId == htmlId);
            var instanceContentFacade = ContentManagerComponent.Instance.ContentFacade;
            var instancePartialsFacade = ContentManagerComponent.Instance.PartialsFacade;
            foreach (var partialLoad in list)
            {
                partialLoad.Type = partialLoad.PartialId.ToGuid() != Guid.Empty ? Enums.PartialTypes.Modual : Enums.PartialTypes.ContentManager;
                switch (partialLoad.Type)
                {
                    case Enums.PartialTypes.ContentManager:
                        partialLoad.Content = instanceContentFacade.Get(partialLoad.PartialId.ToInt());
                        break;
                    case Enums.PartialTypes.Modual:
                        partialLoad.Partials = instancePartialsFacade.Get(partialLoad.PartialId);
                        break;
                   
                }
               
             
            }
            return PartialView("PVGetIndex", list);
        }


        [RadynAuthorize]
        public ActionResult Modify(string PartialId, string CustomId, Guid HtmlDesginId,PageMode pageMode)
        {
            PartialLoad partialLoad=new PartialLoad();
            switch (pageMode)
            {
               
                case PageMode.Edit:
                    var instanceContentFacade = ContentManagerComponent.Instance.ContentFacade;
                    var instancePartialsFacade = ContentManagerComponent.Instance.PartialsFacade;
                    partialLoad = ContentManagerComponent.Instance.PartialLoadFacade.Get(PartialId, CustomId, HtmlDesginId);
                    partialLoad.Type = partialLoad.PartialId.ToGuid() != Guid.Empty ? Enums.PartialTypes.Modual : Enums.PartialTypes.ContentManager;
                    switch (partialLoad.Type)
                    {
                        case Enums.PartialTypes.ContentManager:
                            partialLoad.Content = instanceContentFacade.Get(partialLoad.PartialId.ToInt());
                            break;
                        case Enums.PartialTypes.Modual:
                            partialLoad.Partials = instancePartialsFacade.Get(partialLoad.PartialId);
                            partialLoad.OperationId = partialLoad.Partials.OperationId;
                            break;

                    }

                   
                      
                    break;
                case PageMode.Create:
                     partialLoad.HtmlDesginId = HtmlDesginId;
                     ViewBag.OperationList = new SelectList(SessionParameters.UserOperation, "Id", "Title");
                     ViewBag.CustomIdList = new SelectList(ContentManagerComponent.Instance.HtmlDesginFacade.ReturnCustomeAttributes(HtmlDesginId), "Key", "Value");
                     ViewBag.PartialTypes = new SelectList(EnumUtils.ConvertEnumToIEnumerable<Enums.PartialTypes>(), "Key", "Value");

                    break;
               
            }
           
            PrepareViewBags(partialLoad, pageMode);
            return PartialView("PVModify",partialLoad);
        }

        [HttpPost]
        public ActionResult Modify(FormCollection collection)
        {

            PartialLoad partialLoad = new PartialLoad();
            object[] id = GetModelKey(collection);
            PageMode pageMode = GetPageMode(collection);
            try
            {
                switch (pageMode)
                {
                    case PageMode.Edit:
                        partialLoad = ContentManagerComponent.Instance.PartialLoadFacade.Get(id);
                        RadynTryUpdateModel(partialLoad, collection);
                        partialLoad.CurrentUICultureName = collection["LanguageId"];
                        if (ContentManagerComponent.Instance.PartialLoadFacade.Update(partialLoad))
                        {
                            ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                            return Content("true");
                        }
                        break;
                    case PageMode.Create:
                        RadynTryUpdateModel(partialLoad, collection);
                        partialLoad.CurrentUICultureName = collection["LanguageId"];
                        if (ContentManagerComponent.Instance.PartialLoadFacade.Insert(partialLoad))
                        {
                            ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                            return Content("true");
                        }
                        break;
                }

                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");

            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return Content("false");
            }




            
        }



        [RadynAuthorize]
        public ActionResult Delete(string PartialId, string CustomId, Guid HtmlDesginId)
        {
            
          
            try
            {
                if (ContentManagerComponent.Instance.PartialLoadFacade.Delete(PartialId, CustomId, HtmlDesginId))
                {
                   
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Content("false");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }


    }
}