using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.ContentManager.DataStructure;
using Radyn.Reservation;
using Radyn.Reservation.DataStructure;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressHallController : CongressBaseController
    {

        public ActionResult GetReserveList(Guid Id)
        {
            var list = CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetReservedList(this.Homa.Id,Id);
            return PartialView("PVReserveUserIndex", list);

        }
       
        [RadynAuthorize]
        public ActionResult Index(Guid? parentId)
        {
            var list = parentId.HasValue ? CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetByParents(this.Homa.Id,(Guid) parentId) 
                : CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetParents(this.Homa.Id);
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
            TempData["hall"] = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetByCongressId(this.Homa.Id), "Id", "Name");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Hall();
            try
            {
                this.RadynTryUpdateModel(model, collection);
                var guids = new List<Guid>();
                var value = collection["SelectedType"];
                if (!string.IsNullOrEmpty(value))
                {
                    var split = value.Split(',');
                    foreach (var s1 in split)
                    {
                        guids.Add(s1.ToGuid());
                    }
                }
                HttpPostedFileBase image = null;
                if (Session["Hallphoto"] != null)
                {
                    image = (HttpPostedFileBase)Session["Hallphoto"];
                    Session.Remove("Hallphoto");
                }
                model.CurrentUICultureName =  collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.Insert(this.Homa.Id, model,image, guids))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = model.Id });

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressHall/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                TempData["hall"] = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetByCongressId(this.Homa.Id), "Id", "Name");
                return View(model);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            TempData["hall"] = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetByCongressId(this.Homa.Id), "Id", "Name");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var model = ReservationComponent.Instance.HallFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(model, collection);
                model.CurrentUICultureName = collection["LanguageId"]; 
                var guids = new List<Guid>();
                var value = collection["SelectedType"];
                if (!string.IsNullOrEmpty(value))
                {
                    var split = value.Split(',');
                    foreach (var s1 in split)
                    {
                        guids.Add(s1.ToGuid());
                    }
                }
                HttpPostedFileBase image = null;
                if (Session["Hallphoto"] != null)
                {
                    image = (HttpPostedFileBase)Session["Hallphoto"];
                    Session.Remove("Hallphoto");
                }
                if (CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.Update(this.Homa.Id,model,image, guids))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressHall/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                TempData["hall"] = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetByCongressId(this.Homa.Id), "Id", "Name");
                return View();
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        public ActionResult GetHallChairTypes(Guid? hallId)
        {
            var model = CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetChairTypes(this.Homa.Id, hallId);
            ViewBag.EditMode = true;
            return PartialView("PVUserRegiserType", model);
        }
        public ActionResult GetHallChairTypesDetails(Guid hallId)
        {
            var model = CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetChairTypes(this.Homa.Id,hallId);
            ViewBag.EditMode = false;
            return PartialView("PVUserRegiserType", model);
        }
        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.Delete(this.Homa.Id,Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressHall/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressHall/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }
    }
}