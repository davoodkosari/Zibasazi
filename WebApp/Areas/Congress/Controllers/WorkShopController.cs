using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Common;
using Radyn.Common.Component;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class WorkShopController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {

            var list =
                CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.GetByCongressId(this.Homa.Id);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        
        [RadynAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var workShop = new WorkShop();
            try
            {
                this.RadynTryUpdateModel(workShop);
                workShop.CongressId = this.Homa.Id;
                HttpPostedFileBase fileProgram = null;
                if (Session["ProgramFile"] != null)
                {
                    fileProgram = (HttpPostedFileBase)Session["ProgramFile"];
                    Session.Remove("ProgramFile");
                }
                HttpPostedFileBase file = null;
                if (Session["File"] != null)
                {
                    file = (HttpPostedFileBase)Session["File"];
                    Session.Remove("File");
                }
                var list = new List<Guid>();
                foreach (var key in collection.AllKeys.Where(s => s.StartsWith("CheckSelect")))
                {
                    var id = key.Substring(12, key.Length - 12);
                    list.Add(Guid.Parse(id));
                }
                
                workShop.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.Insert(workShop, list, fileProgram, file))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, new[] { Resources.Common.Ok, " window.location='"+Radyn.Web.Mvc.UI.Application.CurrentApplicationPath+"/Congress/WorkShop/Index'; " }, 
                                messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = true, Url = this.CallBackRedirect(collection, new { Id = workShop.Id }) }, JsonRequestBehavior.AllowGet);
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Json(new { Result = false, Url = string.Empty}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Json(new { Result = false, Url = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult GetDetail(Guid Id)
        {
            var hotel = CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.Get(Id);
            if (hotel == null) return Content("false");
            return PartialView("PVDetails", hotel);
        }
        public ActionResult GetModify(Guid? Id,string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            ViewBag.Currency =
                new SelectList(
                    EnumUtils.ConvertEnumToIEnumerableInLocalization<Radyn.Common.Definition.Enums.CurrencyType>(),
                    "Key", "Value");
            if (!Id.HasValue) return PartialView("PVModify", new WorkShop());
            var workShop = CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.GetLanuageContent(culture,Id);
            return PartialView("PVModify",  workShop);
        }
        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost,ValidateInput(false)]
        public ActionResult Edit(FormCollection collection)
        {
            var workShop = CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.Get(collection["Id"].ToGuid());
            try
            {
                this.RadynTryUpdateModel(workShop);
                HttpPostedFileBase fileProgram = null;
                if (Session["ProgramFile"] != null)
                {
                    fileProgram = (HttpPostedFileBase)Session["ProgramFile"];
                    Session.Remove("ProgramFile");
                }
                HttpPostedFileBase file = null;
                if (Session["File"] != null)
                {
                    file = (HttpPostedFileBase)Session["File"];
                    Session.Remove("File");
                }
                var list = new List<Guid>();
                foreach (var key in collection.AllKeys.Where(s => s.StartsWith("CheckSelect")))
                {
                    var id = key.Substring(12, key.Length - 12);
                    list.Add(Guid.Parse(id));
                }
                workShop.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.Update(workShop, list, fileProgram, file))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,  
                                messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = true, Url = this.CallBackRedirect(collection,new {Id=workShop.Id})},JsonRequestBehavior.AllowGet);
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Json(new { Result = false, Url = string.Empty},JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Json(new { Result = false, Url = string.Empty }, JsonRequestBehavior.AllowGet);
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
           
            try
            {
                
                if (CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/WorkShop/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/WorkShop/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }
        public ActionResult DeleteWorkShop(Guid Id)
        {
            try
            {

                if (CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, new[] { Resources.Common.Ok, " window.location='"+Radyn.Web.Mvc.UI.Application.CurrentApplicationPath+"/Congress/WorkShop/Index'; " }, messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
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