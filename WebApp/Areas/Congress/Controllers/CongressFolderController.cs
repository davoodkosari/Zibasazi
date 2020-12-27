using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.FileManager;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressFolderController : CongressBaseController
    {

        [RadynAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FolderTree()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.CongressFoldersFacade.Select(x => x.Folder,
                x => x.CongressId == this.Homa.Id,
                new OrderByModel<CongressFolders>() { Expression = x => x.Folder.Title }, true);
            return PartialView("PartialViewFolders", list);
        }
        public ActionResult GetFolder(string state, Guid id)
        {
            var menu = new Folder();
            switch (state)
            {

                case "Create":
                    var menu1 = FileManagerComponent.Instance.FolderFacade.Get(id);
                    menu = new Folder();
                    if (menu1 != null) menu1.ParentFolderId = menu1.Id;
                    break;
                case "Edit":
                    menu = FileManagerComponent.Instance.FolderFacade.Get(id);
                    break;
                case "Delete":
                    menu = FileManagerComponent.Instance.FolderFacade.Get(id);
                    break;
            }
            ViewBag.id = id;
            ViewBag.state = state;
            if (state == "Create" || state == "Edit")
                return PartialView("LookUPModify", menu);
            return PartialView("LookUPDetails", menu);
        }
        public ActionResult ModifyFolder(FormCollection formCollection)
        {

            try
            {
                var id = formCollection["FolderId"];
                var state = formCollection["FolderState"];
                var parentorOwn = string.IsNullOrEmpty(id) ? null : FileManagerComponent.Instance.FolderFacade.Get(id.ToGuid());
                switch (state)
                {

                    case "Create":
                        {

                            var folder = new Folder();
                            this.RadynTryUpdateModel(folder, formCollection);
                            if (parentorOwn != null) folder.ParentFolderId = parentorOwn.Id;
                            if (CongressComponent.Instance.BaseInfoComponents.CongressFoldersFacade.Insert(this.Homa.Id, folder))
                            {
                                ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                   messageIcon: MessageIcon.Succeed);
                                return Content("true");
                            }
                            ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                              
                                messageIcon: MessageIcon.Error);
                            return Content("false");
                        }
                    case "Edit":
                        {

                            if (parentorOwn == null)
                            {
                                ShowMessage(Resources.ContentManager.Not_Exist_To_Edit, Resources.Common.MessaageTitle,
                                   
                                    messageIcon: MessageIcon.Error);
                                return Content("false");
                            }
                            this.RadynTryUpdateModel(parentorOwn, formCollection);

                            if (FileManagerComponent.Instance.FolderFacade.Update(parentorOwn))
                            {
                                ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                 messageIcon: MessageIcon.Succeed);
                                return Content("true");
                            }
                            ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Error);
                            return Content("false");
                        }
                    case "Delete":
                        {
                            if (parentorOwn == null)
                            {
                                ShowMessage(Resources.ContentManager.Not_Exist_To_delete, Resources.Common.MessaageTitle,
                                   
                                    messageIcon: MessageIcon.Error);
                                return Content("false");
                            }
                            var child =
                                FileManagerComponent.Instance.FolderFacade.Any(
                                    folder => folder.ParentFolderId == parentorOwn.Id);
                            if (child)
                            {
                                ShowMessage(Resources.ContentManager.This_Item_Not_able_to_Delete_Becase_Have_ChildNode,
                                    Resources.Common.MessaageTitle,
                                    messageIcon: MessageIcon.Error);
                                return Content("false");
                            }
                            if (CongressComponent.Instance.BaseInfoComponents.CongressFoldersFacade.Delete(this.Homa.Id, parentorOwn.Id))
                            {
                                ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                   
                                    messageIcon: MessageIcon.Succeed);
                                return Content("true");
                            }
                            ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                               
                                messageIcon: MessageIcon.Error);
                            return Content("false");
                        }
                }
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.An_error_has_occurred + exception.Message, Resources.Common.MessaageTitle,
                   
                    messageIcon: MessageIcon.Error);
                return Content("false");
            }
        }


        public ActionResult LookUPFileModify(Guid Id, Guid? fileId = null)
        {
            var folder = FileManagerComponent.Instance.FolderFacade.Get(Id);
            ViewBag.FolderName = folder != null ? folder.Title : "";
            ViewBag.FolderId = Id;
            ViewBag.IsForUser = fileId.HasValue && CongressComponent.Instance.BaseInfoComponents.UserFileFacade.Get(this.Homa.Id, fileId) != null;
            return PartialView("LookUPFileModify", fileId.HasValue ? FileManagerComponent.Instance.FileFacade.Get(fileId) : new File());
        }
        [HttpPost]
        public ActionResult FileModify(FormCollection collection)
        {
            try
            {
                List<HttpPostedFileBase> File = null;
                if (Session["File"] != null)
                {
                    File = (List<HttpPostedFileBase>)Session["File"];
                    Session.Remove("File");
                }
                var Id = string.IsNullOrEmpty(collection["FolderId"]) ? (Guid?)null : collection["FolderId"].ToGuid();
                if (collection["FileId"].ToGuid() == Guid.Empty)
                {
                    if (File == null)
                    {
                        ShowMessage(Resources.FileManager.PleaseUploadFile, Resources.Common.MessaageTitle,
                       messageIcon: MessageIcon.Error);
                        return Content("false");
                    }

                    if (CongressComponent.Instance.BaseInfoComponents.UserFileFacade.Insert(this.Homa.Id, File, collection["ForUser"].ToBool(), new File() { FolderId = Id, FileName = collection["FileName"] }))
                    {
                        ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                           
                            messageIcon: MessageIcon.Succeed);
                        return Content("true");
                    }
                    ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                if (File != null)
                {
                    if (CongressComponent.Instance.BaseInfoComponents.UserFileFacade.Update(this.Homa.Id, File.FirstOrDefault(), collection["FileId"].ToGuid(), collection["ForUser"].ToBool(), new File() { FileName = collection["FileName"], FolderId = Id }))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                           
                            messageIcon: MessageIcon.Succeed);
                        return Content("true");
                    }
                    ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                   messageIcon: MessageIcon.Error);
                    return Content("false");
                }

                if (CongressComponent.Instance.BaseInfoComponents.UserFileFacade.Update(this.Homa.Id, collection["FileId"].ToGuid(), collection["ForUser"].ToBool(), new File() { FileName = collection["FileName"], FolderId = Id }))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                       
                        messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        public ActionResult GetFirstFiles()
        {
            var firstParent = CongressComponent.Instance.BaseInfoComponents.CongressFoldersFacade.GetFirstParent();
            if (firstParent == null) return PartialView("PartialViewFiles", new Dictionary<File, bool>());
            var files = CongressComponent.Instance.BaseInfoComponents.CongressFoldersFacade.GetFolderFiles(this.Homa.Id, firstParent.Id);
            ViewBag.FolderId = firstParent.Id;
            return PartialView("PartialViewFiles", files);
        }
        public ActionResult GetFiles(Guid Id)
        {
            Dictionary<File, bool> list = null;
            if (Id == Guid.Empty)
            {
                var firstParent = CongressComponent.Instance.BaseInfoComponents.CongressFoldersFacade.GetFirstParent();
                if (firstParent == null) return PartialView("PartialViewFiles", new Dictionary<File, bool>());
                list = CongressComponent.Instance.BaseInfoComponents.CongressFoldersFacade.GetFolderFiles(this.Homa.Id, firstParent.Id);
                ViewBag.FolderId = firstParent.Id;
                return PartialView("PartialViewFiles", list);
            }
            list = CongressComponent.Instance.BaseInfoComponents.CongressFoldersFacade.GetFolderFiles(this.Homa.Id, Id);
            ViewBag.FolderId = Id;
            return PartialView("PartialViewFiles", list);
        }
    }
}
