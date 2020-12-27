using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Reservation;
using Radyn.Reservation.DataStructure;
using Radyn.Reservation.Definition;
using Radyn.Reservation.Facade.Interface;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;

namespace Radyn.WebApp.Areas.Reservation.Controllers
{
    public class HallChairDesignController : LocalizedController
    {

        public ActionResult LookUpView(Guid hallId, string onlyselectChairTypeId = null, string selectedId = null, string maxselect = null, bool canselectUnableSale = false)
        {
            var hallWithChairs = ReservationComponent.Instance.HallFacade.GetHallWithChairs(hallId);
            if (!string.IsNullOrEmpty(onlyselectChairTypeId))
            {
                var byRefId = ReservationComponent.Instance.ChairTypeFacade.FirstOrDefault(x => x.HallId == hallId && x.RefId == onlyselectChairTypeId);
                ViewBag.SelectedChairTypeId = byRefId != null ? byRefId.Id.ToString() : null;

            }
            ViewBag.selectedId = selectedId;
            ViewBag.maxselect = maxselect;
            ViewBag.canselectUnableSale = canselectUnableSale;
            return View(hallWithChairs);
        }

        public ActionResult LookUpDesign(Guid hallId)
        {
            try
            {
                var hallWithChairs = ReservationComponent.Instance.HallFacade.GetHallWithChairs(hallId);
                ViewBag.StatusList = new SelectList(
                    EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.ReservStatus>().Select(
                        keyValuePair =>
                            new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.ReservStatus>(),
                                keyValuePair.Value)), "Key", "Value");
                ViewBag.ChairTypes = new SelectList(ReservationComponent.Instance.ChairTypeFacade.SelectKeyValuePair(x => x.Id, x => x.Title, x =>
                      x.HallId == hallId), "Key", "Value");
                return View(hallWithChairs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public ActionResult SaveDesign(FormCollection collection)
        {

            try
            {
                var value = collection.AllKeys.Where(x => x.StartsWith("ItemParamerts-"));
                if (!value.Any())
                {
                    ShowMessage("No Selected Item", Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var list = value.Select(variable => collection[variable]).ToList();
                if (ReservationComponent.Instance.HallFacade.HallChairUpdate(list))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        public ActionResult AddChairToAddToBasket(Guid Id, string selectedId, string maxselect)
        {
            var list = new List<Guid>();
            var maxselectint = 0;
            try
            {
                var result = 1;
                if (!string.IsNullOrEmpty(maxselect))
                    maxselectint = StringUtils.Decrypt(maxselect).ToInt();
                if (!string.IsNullOrEmpty(selectedId))
                {

                    var chairs = selectedId.Split(',');
                    list.AddRange(chairs.Select(chair => chair.ToGuid()));
                }
                if (list.Any(x => x == Id))
                {
                    list.Remove(Id);
                    result = 0;
                }
                else if (maxselectint > 0 && list.Count >= maxselectint)
                    result = -1;
                else list.Add(Id);
                return Json(new { Result = result, Value = string.Join(",", list), ChairCount = maxselectint }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Json(new { Result = -2, Value = string.Join(",", list), ChairCount = maxselectint }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetSelectedHtml(string selectedId)
        {
            var list = new List<Guid>();
            var html = "";
            try
            {
                if (!string.IsNullOrEmpty(selectedId))
                {

                    var chairs = selectedId.Split(',');
                    list.AddRange(chairs.Select(chair => chair.ToGuid()));
                }
                var chairByIdList = ReservationComponent.Instance.ChairFacade.GetListChairByIdList(list);
                if (chairByIdList != null)
                {
                    html = chairByIdList.Aggregate(html,
                        (current, chair) =>
                            current +
                            ("<div  class='table-row-item' style=\"width:20%;height:3%\" >" +
                             (chair.Hall.ParentId.HasValue ? Resources.Reservation.Parent + ":" + chair.Hall.ParentHall.Name + " " : "") +
                             Resources.Reservation.Hall + ":" + chair.Hall.Name + "<br/>" + Resources.Reservation.ChairType + ":" +
                             chair.ChairType.Title + " " + Resources.Reservation.Row + ":" +
                             chair.Row + " " + Resources.Reservation.Column + ":" + chair.Column + " " +
                             Resources.Reservation.Number + ":" + chair.Number + "</div>"));
                }
                return Json(new { Html = html }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Json(new { Html = html }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
