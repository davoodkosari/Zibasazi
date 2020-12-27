using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Reservation;
using Radyn.Reservation.DataStructure;
using Radyn.Reservation.Definition;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;

namespace Radyn.WebApp.Areas.Reservation.Controllers
{
    public class HallController : LocalizedController
    {



        [RadynAuthorize]
        public ActionResult Index(Guid? parentId)
        {

            var builder = new PredicateBuilder<Hall>();
            if (parentId.HasValue)
                builder.And(x => x.ParentId == parentId);
            else builder.And(x => x.ParentId == null);

            var list =
                ReservationComponent.Instance.HallFacade.Where(builder.GetExpression());

            if (!list.Any()) return this.Redirect("~/Reservation/Hall/Create");
            return View(list);
        }
        public ActionResult GetDetail(Guid Id)
        {
            return PartialView("PVDetails", ReservationComponent.Instance.HallFacade.Get(Id));
        }
        public ActionResult GetModify(Guid? Id)
        {
            return PartialView("PVModify", Id.HasValue ? ReservationComponent.Instance.HallFacade.Get(Id) : new Hall());
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
            TempData["hall"] = new SelectList(ReservationComponent.Instance.HallFacade.SelectKeyValuePair(x => x.Id, x => x.Name, x => x.IsExternal == false), "Key", "Value");
            return View(new Hall());
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var hall = new Hall();
            try
            {
                this.RadynTryUpdateModel(hall);
                HttpPostedFileBase image = null;
                if (Session["Hallphoto"] != null)
                {
                    image = (HttpPostedFileBase)Session["Hallphoto"];
                    Session.Remove("Hallphoto");
                }
                hall.CurrentUICultureName = collection["LanguageId"];
                if (ReservationComponent.Instance.HallFacade.Insert(hall, image))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.Redirect("~/Reservation/Hall/Edit?Id=" + hall.Id);
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return this.Redirect("~/Reservation/Hall/Edit?Id=");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                TempData["hall"] = new SelectList(ReservationComponent.Instance.HallFacade.SelectKeyValuePair(x => x.Id, x => x.Name, x => x.IsExternal == false), "Key", "Value");
                return View(hall);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            TempData["hall"] = new SelectList(ReservationComponent.Instance.HallFacade.SelectKeyValuePair(x => x.Id, x => x.Name, x => x.IsExternal == false), "Key", "Value");
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var hall = ReservationComponent.Instance.HallFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(hall);
                HttpPostedFileBase image = null;
                if (Session["Hallphoto"] != null)
                {
                    image = (HttpPostedFileBase)Session["Hallphoto"];
                    Session.Remove("Hallphoto");
                }
                hall.CurrentUICultureName = collection["LanguageId"];
                if (ReservationComponent.Instance.HallFacade.Update(hall, image))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.Redirect("~/Reservation/Hall/Index");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return this.Redirect("~/Reservation/Hall/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                TempData["hall"] = new SelectList(ReservationComponent.Instance.HallFacade.SelectKeyValuePair(x => x.Id, x => x.Name, x => x.IsExternal == false), "Key", "Value");
                return View(hall);
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
            var hall = ReservationComponent.Instance.HallFacade.Get(Id);
            try
            {
                if (ReservationComponent.Instance.HallFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.Redirect("~/Reservation/Hall/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return this.Redirect("~/Reservation/Hall/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(hall);
            }
        }

        public ActionResult ShowHallPhoto(Guid Id)
        {

            ViewBag.PhotoId = Id;
            return View();
        }
    }
}
