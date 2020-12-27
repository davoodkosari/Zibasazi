using Radyn.Common;
using Radyn.Common.Component;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade;
using Radyn.Message;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.Payment.Tools;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Filter;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.FormGenerator.Tools;
using Radyn.WebApp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Framework;
using static Radyn.Congress.Tools.Enums;
using Enums = Radyn.Congress.Tools.Enums;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class ManagmentPanelController : CongressBaseController
    {


        [RadynAuthorize]
        public ActionResult LoginAsUser(Guid Id)
        {
            var user = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(Id);
            if (user == null) return Redirect("~/Congress/User/Index");
            SessionParameters.CongressUser = user;
            return Redirect("~/Congress/UserPanel/Home");
        }

        [RadynAuthorize]
        public ActionResult LoginAsReferee(Guid Id)
        {
            var congressReferee = CongressComponent.Instance.BaseInfoComponents.RefereeFacade.Get(Id);
            if (congressReferee == null) return Redirect("~/Congress/Referee/Index");
            SessionParameters.CongressReferee = congressReferee;
            return Redirect("~/Congress/RefereePanel/RefereeCartablIndex");
        }

        #region WorkShop


        [RadynAuthorize]
        public ActionResult GetModifyUserWorkShop(Guid userId,Guid workshopId,PageMode status)
        {
            try
            {
               
                WorkShopUser workShopUser= null;
                switch (status)
                {
                    case PageMode.Create:
                        workShopUser = new WorkShopUser(){WorkShopId = workshopId};
                        break;
                    case PageMode.Edit:
                        workShopUser =
                            CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.Get(userId,workshopId);
                        break;
                }
                ViewBag.WorkShopRezervstatus =
                    EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.WorkShopRezervState>().Select(
                        keyValuePair =>
                            new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.WorkShopRezervState>(),
                                keyValuePair.Value));
                this.PrepareViewBags(workShopUser,status);
                return PartialView("PartialViewModifyUserWorkShop", workShopUser);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return null;
            }
            
        }
        [RadynAuthorize]
        [HttpPost]
        public ActionResult GetModifyUserWorkShop(FormCollection collection)
        {
            try
            {
                WorkShopUser executiveOffice = null;
                PageMode pageMode = base.GetPageMode<WorkShopUser>(collection);
                var id = base.GetModelKey<WorkShopUser>(collection);
                var postFormData = this.PostForFormGenerator(collection);
                switch (pageMode)
                {
                    case PageMode.Edit:
                        executiveOffice = CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.Get(id);
                        RadynTryUpdateModel(executiveOffice, collection);
                        if (CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.WorkShopUserUpdate(this.Homa.Id,executiveOffice,postFormData))
                        {
                            
                            return Content("true");
                        }
                        ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                        return Content("false");
                    case PageMode.Create:
                        executiveOffice = new WorkShopUser();
                        RadynTryUpdateModel(executiveOffice, collection);
                        if (CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.WorkShopUserInsert(this.Homa.Id, executiveOffice, postFormData))
                        {
                            
                            return Content("true");
                        }
                        ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                        return Content("false");
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
        public ActionResult UserRequestWorkShops(Guid Id)
        {
            GetWorkShopSearchViewBags(Id);
            return View();
        }

        [HttpPost]
        public ActionResult UserRequestWorkShops(FormCollection collection)
        {
            try
            {
                var Id = collection["WorkShopId"].ToGuid();
                byte? status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null :
                       (byte)collection["SearchStatus"].ToEnum<Enums.WorkShopRezervState>();
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.Search(Id, status, collection["RegisterDate"], collection["txtSearch"], postFormData);
                ViewBag.status =
               EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.WorkShopRezervState>().Select(
                   keyValuePair =>
                       new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.WorkShopRezervState>(),
                           keyValuePair.Value));
                return PartialView("PartialViewUserRequestWorkShops", list);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        private void GetWorkShopSearchViewBags(Guid workshopId)
        {
            ViewBag.status =
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.WorkShopRezervState>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.WorkShopRezervState>(),
                            keyValuePair.Value));
            var workShop = CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.Get(workshopId);
            ViewBag.WorkShop = workShop != null ? workShop.Subject : "";
            ViewBag.WorkShopId = workshopId;
        }
        [HttpPost]
        public ActionResult UpdateUserRequestWorkShops(FormCollection collection)
        {
            try
            {
                var list = new List<WorkShopUser>();
                var Id = collection["WorkShopId"].ToGuid();
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("ModelId"));
                if (!string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    var strings = collection[firstOrDefault].Split(',');
                    foreach (var key in strings)
                    {
                        if (string.IsNullOrEmpty(key)) continue;
                        var model = new WorkShopUser { WorkShopId = Id, UserId = key.ToGuid() };
                        model.Status = (byte)collection["Status-" + model.UserId].ToEnum<Enums.WorkShopRezervState>();
                        var oldstatus = (byte)collection["oldstatus-" + model.UserId].ToEnum<Enums.WorkShopRezervState>();
                        if (oldstatus != model.Status) list.Add(model);
                    }
                }


                if (CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.UpdateList(this.Homa.Id, list))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        public ActionResult DeleteUserWorkShop(Guid userId, Guid workshopId)
        {
            try
            {

                if (CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.Delete(userId, workshopId))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
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

        #endregion
        public ActionResult ReportResetFactory()
        {
            var congressDefinitionGetReportList =
                CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.CongressDefinitionGetReportList(this.Homa.Id);
            return View(congressDefinitionGetReportList);
        }
        [HttpPost]
        public ActionResult ReportResetFactory(FormCollection collection)
        {
            try
            {
                var list = new List<string>();
                var enumerable = collection.AllKeys.FirstOrDefault(s => s.Equals("SelectedReport"));
                if (enumerable != null)
                {
                    var strings = collection[enumerable].Split(',');
                    list.AddRange(strings);
                }
                if (CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.ResetFactoryList(this.Homa.Id, list))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/ManagmentPanel/ReportResetFactory");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/ManagmentPanel/ReportResetFactory");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/ManagmentPanel/ReportResetFactory");
            }
        }
        #region UserBooth
        public ActionResult LookUpUserForm(Guid userId)
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.UserFormsFacade.SelectKeyValuePair(
                    c => c.FormStructure.Id, c => c.FormStructure.Name, x => x.FormStructure.Enable);
            ViewBag.FormList = new SelectList(list, "Key", "Value");
            ViewBag.userId = userId;
            return View();
        }
        public ActionResult LookUpBoothReserver(Guid userId, Guid boothId)
        {
            ViewBag.userId = userId;
            ViewBag.boothId = boothId;
            return View();
        }
        public ActionResult BoothOfficer(Guid userId, Guid boothId)
        {
            ViewBag.userId = userId;
            ViewBag.boothId = boothId;
            return View();
        }

        [RadynAuthorize]
        public ActionResult UserBooth(Guid boothId)
        {

            GetUserboothSearchViewBags(boothId);
            return View();
        }
        private void GetUserboothSearchViewBags(Guid boothId)
        {
            ViewBag.status =
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.RezervState>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.RezervState>(),
                            keyValuePair.Value));
            var booth = CongressComponent.Instance.BaseInfoComponents.BoothFacade.Get(boothId);
            ViewBag.Booth = booth != null ? booth.Code : "";
            ViewBag.BoothId = boothId;
        }
        [RadynAuthorize]
        [HttpPost]
        public ActionResult UserBooth(FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["BoothId"])) return Content("false");
                var Id = collection["BoothId"].ToGuid();
                byte? status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null :
                       (byte)collection["SearchStatus"].ToEnum<Enums.RezervState>();
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Search(Id, status, collection["RegisterDate"], collection["txtSearch"], postFormData);
                ViewBag.status =
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.RezervState>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.RezervState>(),
                            keyValuePair.Value));
                return PartialView("PartialViewUserRequestBooth", list);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }


        [HttpPost]
        public ActionResult UpdateUserRequestBooth(FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["BoothId"])) return Content("false");
                var list = new List<UserBooth>();
                var Id = collection["BoothId"].ToGuid();
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("ModelId"));
                if (!string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    var strings = collection[firstOrDefault].Split(',');
                    foreach (var key in strings)
                    {
                        if (string.IsNullOrEmpty(key)) continue;
                        var model = new UserBooth { BoothId = Id, UserId = key.ToGuid() };
                        model.Status = (byte)collection["Status-" + model.UserId].ToEnum<Enums.RezervState>();
                        var oldstatus = (byte)collection["oldstatus-" + model.UserId].ToEnum<Enums.RezervState>();
                        if (oldstatus != model.Status) list.Add(model);
                    }
                }

                if (CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.UpdateList(this.Homa.Id, list))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }


        public ActionResult DeleteUserbooth(Guid userId, Guid boothId)
        {
            try
            {

                if (CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Delete(userId, boothId))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
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

        [RadynAuthorize]
        public ActionResult GetModifyUserBooth(Guid userId, Guid boothId, PageMode status)
        {
            try
            {

                UserBooth userBooth = null;
                switch (status)
                {
                    case PageMode.Create:
                        userBooth = new UserBooth() { BoothId = boothId };
                        break;
                    case PageMode.Edit:
                        userBooth =
                            CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Get(userId, boothId);
                        Session["BoothOfficers"] = CongressComponent.Instance.BaseInfoComponents.BoothOfficerFacade.Where(
                            x =>

                                x.UserId == userId &&
                                x.BoothId == boothId);
                        break;
                }
                ViewBag.userBoothstatus =
                    EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.RezervState>().Select(
                        keyValuePair =>
                            new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.RezervState>(),
                                keyValuePair.Value));
                this.PrepareViewBags(userBooth, status);
                return PartialView("PartialViewModifyUserBooth", userBooth);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return null;
            }

        }
        [RadynAuthorize]
        [HttpPost]
        public ActionResult GetModifyUserBooth(FormCollection collection)
        {
            try
            {
                UserBooth userBooth = null;
                PageMode pageMode = base.GetPageMode<UserBooth>(collection);
                var id = base.GetModelKey<UserBooth>(collection);
                var boothOfficers = (List<BoothOfficer>)Session["BoothOfficers"];
                var postFormData = this.PostForFormGenerator(collection);
                switch (pageMode)
                {
                    case PageMode.Edit:
                        userBooth = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Get(id);
                        RadynTryUpdateModel(userBooth, collection);
                        if (CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.UserBoothUpdate(this.Homa.Id,userBooth,postFormData,boothOfficers))
                        {
                            
                            return Content("true");
                        }
                        ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                        return Content("false");
                    case PageMode.Create:
                        userBooth = new UserBooth();
                        RadynTryUpdateModel(userBooth, collection);
                        if (CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.UserBoothInsert(this.Homa.Id, userBooth, postFormData, boothOfficers))
                        {
                            
                            return Content("true");
                        }
                        ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                        return Content("false");
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

        #endregion

        #region Hotel

        [RadynAuthorize]
        public ActionResult UserRequestHotels(Guid hotelId)
        {
            GetHotelSearchViewBags(hotelId);
            return View();
        }
        private void GetHotelSearchViewBags(Guid hotelId)
        {
            ViewBag.status =
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.RezervState>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.RezervState>(),
                            keyValuePair.Value));
            var hotel = CongressComponent.Instance.BaseInfoComponents.HotelFacade.Get(hotelId);
            ViewBag.Hotel = hotel != null ? hotel.Name : "";
            ViewBag.HotelId = hotelId;
        }
        [RadynAuthorize]
        [HttpPost]
        public ActionResult UserRequestHotels(FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["HotelId"])) return Content("false");
                var Id = collection["HotelId"].ToGuid();
                byte? status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null :
                       (byte)collection["SearchStatus"].ToEnum<Enums.RezervState>();
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.Search(Id, status, collection["RegisterDate"], collection["txtSearch"], postFormData);
                ViewBag.status =
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.RezervState>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.RezervState>(),
                            keyValuePair.Value));
                return PartialView("PartialViewUserRequestHotels", list);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }


        [HttpPost]
        public ActionResult UpdateUserRequestHotels(FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["HotelId"])) return Content("false");
                var list = new List<HotelUser>();
                var Id = collection["HotelId"].ToGuid();
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("ModelId"));
                if (!string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    var strings = collection[firstOrDefault].Split(',');
                    foreach (var key in strings)
                    {
                        if (string.IsNullOrEmpty(key)) continue;
                        var model = new HotelUser { HotelId = Id, UserId = key.ToGuid() };
                        model.Status = (byte)collection["Status-" + model.UserId].ToEnum<Enums.RezervState>();
                        var oldstatus = (byte)collection["oldstatus-" + model.UserId].ToEnum<Enums.RezervState>();
                        if (oldstatus != model.Status) list.Add(model);
                    }
                }

                if (CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.UpdateList(this.Homa.Id, list))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        public ActionResult DeleteUserHotel(Guid userId, Guid hotelId)
        {
            try
            {

                if (CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.Delete(hotelId, userId))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
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


        [RadynAuthorize]
        public ActionResult GetModifyUserHotel(Guid userId, Guid hotelId, PageMode status)
        {
            try
            {

                HotelUser hotelUser = null;
                switch (status)
                {
                    case PageMode.Create:
                        hotelUser = new HotelUser() { HotelId = hotelId };
                        break;
                    case PageMode.Edit:
                        hotelUser =
                            CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.Get(hotelId,userId);
                        break;
                }
                ViewBag.Hotelstatus =
                    EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.RezervState>().Select(
                        keyValuePair =>
                            new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.RezervState>(),
                                keyValuePair.Value));
                this.PrepareViewBags(hotelUser, status);
                return PartialView("PartialViewModifyUserHotel", hotelUser);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return null;
            }

        }
        [RadynAuthorize]
        [HttpPost]
        public ActionResult GetModifyUserHotel(FormCollection collection)
        {
            try
            {
                HotelUser hotelUser = null;
                PageMode pageMode = base.GetPageMode<HotelUser>(collection);
                var id = base.GetModelKey<HotelUser>(collection);
                var postFormData = this.PostForFormGenerator(collection);
                switch (pageMode)
                {
                    case PageMode.Edit:
                        hotelUser = CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.Get(id);
                        RadynTryUpdateModel(hotelUser, collection);
                        if (CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.HotelUserUpdate(this.Homa.Id,hotelUser,postFormData))
                        {
                           
                            return Content("true");
                        }
                        ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                        return Content("false");
                    case PageMode.Create:
                        hotelUser = new HotelUser();
                        RadynTryUpdateModel(hotelUser, collection);
                        if (CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.Insert(hotelUser))
                        {
                          
                            return Content("true");
                        }
                        ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                        return Content("false");
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

        #endregion

        #region Articles
        public ActionResult GetFlow()
        {
            return PartialView("PartialViewFlow");
        }

        [RadynAuthorize]
        public ActionResult UserArticles()
        {
            Session.Remove("ArticleAuthors");
            GetArticleSearchViewBags();
            return View();
        }
        private void GetArticleSearchViewBags()
        {


            ViewBag.ArticleType = CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.SelectKeyValuePair(x => x.Id, x => x.Title, type => type.CongressId == this.Homa.Id);
            ViewBag.status =
               EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.ArticleState>().Select(
                   keyValuePair =>
                       new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.ArticleState>(),
                           keyValuePair.Value));
            ViewBag.paystatus =
               EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.ArticlepayState>().Select(
                   keyValuePair =>
                       new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.ArticlepayState>(),
                           keyValuePair.Value));
            ViewBag.SortAccordingTo =
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.SortAccordingToArticle>().Where(c => c.Key != "0").Select(
                   keyValuePair =>
                       new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.SortAccordingToArticle>(),
                           keyValuePair.Value));
            ViewBag.AscendingDescending =
              EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.AscendingDescending>().Where(c => c.Key != "1").Select(
                 keyValuePair =>
                     new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.AscendingDescending>(),
                         keyValuePair.Value));
            ViewBag.finalstatus =
               EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.FinalState>().Select(
                   keyValuePair =>
                       new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.FinalState>(),
                           keyValuePair.Value));

            ViewBag.PivotCategory = CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.SelectKeyValuePair(x => x.Id, x => x.Title, x => x.CongressId == this.Homa.Id, new OrderByModel<PivotCategory>() { Expression = x => x.Order });

            ViewBag.totalstatus = new SelectList(Utility.EnumUtils.ConvertEnumToIEnumerableInLocalization<FinalState>(), "Key", "Value");

            ViewBag.HasArticlePayment = this.Homa.Configuration.HasArticlePayment;

        }

        public JsonResult GetPivot(Guid? id)
        {
            var result = new List<object>();
            var obj = new Object();
            var list = id.HasValue ? CongressComponent.Instance.BaseInfoComponents.PivotFacade.Where(x => x.PivotCategoryId == id.Value) : new List<Pivot>();
            foreach (var item in list)
            {
                obj = new
                {
                    Id = item.Id,
                    Title = item.Title
                };
                result.Add(obj);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UserArticles(FormCollection collection)
        {
            try
            {
                var article = Areas.Congress.Tools.AppExtention.PrepareArticleSearch(collection);
                var ascendingDescending = collection["SortByAscendingDescending"].ToEnum<Enums.AscendingDescending>();
                var articleflow = collection["SortFinalState"].ToEnum<Enums.SortAccordingToArticle>();
                GetArticleSearchViewBags();
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(collection["txtReferee"]))
                {
                    article.RefreeTitle = collection["txtReferee"];
                }
                var list = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.SearchDynamic(this.Homa.Id, article, collection["txtSearch"], postFormData, ascendingDescending, articleflow);

                return PartialView("PartialViewUserArticles", list);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

      


        [HttpPost]
        public ActionResult UpdateUserArticles(FormCollection collection)
        {

            try
            {
                var model = UpdateArticleGrid(collection);
                if (CongressComponent.Instance.BaseInfoComponents.ArticleFacade.AdminUpdateArticles(this.Homa.Id, model))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }

                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }



        [HttpPost]
        public ActionResult UpdateRecord(Guid id, Guid? articleType, byte finalStatus)
        {

            try
            {
                var article = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(id);
                article.TypeId = articleType;
                article.FinalState = finalStatus;
                if (CongressComponent.Instance.BaseInfoComponents.ArticleFacade.AdminUpdate(article))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                               messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        public ActionResult GetArticleFlow(Guid articelId, Guid? filterId, bool? isReferee = null, bool? isuser = null)
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.ArticleFlowFacade.GetArticleFlow(this.Homa.Id, articelId, filterId, isReferee, isuser);
            return PartialView("PartialViewArticleFlowList", list);
        }


        private static List<Radyn.Congress.DataStructure.Article> UpdateArticleGrid(FormCollection collection)
        {
            var model = new List<Radyn.Congress.DataStructure.Article>();
            var articleFacade = CongressComponent.Instance.BaseInfoComponents.ArticleFacade;
            var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("ModelId"));
            if (string.IsNullOrEmpty(collection[firstOrDefault])) return model;
            var strings = collection[firstOrDefault].Split(',');
            foreach (var vale in strings)
            {
                if (string.IsNullOrEmpty(vale)) continue;
                var articleId = vale.ToGuid();
                var oldtype = string.IsNullOrEmpty(collection["oldtype-" + articleId]) ? (Guid?)null : collection["oldtype-" + articleId].ToGuid();
                var oldFinalState = string.IsNullOrEmpty(collection["oldFinalState-" + articleId]) ? (byte?)null : (byte)collection["oldFinalState-" + articleId].ToEnum<Enums.FinalState>();
                var oldPayStatus = string.IsNullOrEmpty(collection["oldPayStatus-" + articleId]) ? (byte?)null : (byte)collection["oldPayStatus-" + articleId].ToEnum<Enums.ArticlepayState>();

                var newtype = string.IsNullOrEmpty(collection["drpArticleType-" + articleId]) ? (Guid?)null : collection["drpArticleType-" + articleId].ToGuid();
                var newFinalState = string.IsNullOrEmpty(collection["drpfinalstatus-" + articleId]) ? (byte?)null : (byte)collection["drpfinalstatus-" + articleId].ToEnum<Enums.FinalState>();
                var newPayStatus = string.IsNullOrEmpty(collection["drppaymentstatus-" + articleId]) ? (byte?)null : (byte)collection["drppaymentstatus-" + articleId].ToEnum<Enums.ArticlepayState>();

                if (oldtype == newtype && oldFinalState == newFinalState && oldPayStatus == newPayStatus) continue;
                var article = articleFacade.Get(articleId);
                article.TypeId = newtype;
                if (newFinalState != null) article.FinalState = (byte)newFinalState;
                article.PayStatus = newPayStatus;
                model.Add(article);
            }

            return model;
        }

        public ActionResult ArticleRefereesLookUp(Guid articleId)
        {
            var referees =
                CongressComponent.Instance.BaseInfoComponents.RefereeFacade.GetAllForArticle(this.Homa.Id, articleId, this.Homa.Configuration.SentArticleSpecialReferee);
            ViewBag.ArticleId = articleId;
            return View(referees);
        }

        public ActionResult ArticleRefereeopinionLookUp(Guid articleId)
        {
            var cartables = CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.Where(cartable => cartable.ArticleId == articleId);
            this.TempData.Clear();
            return View(cartables);
        }
        [HttpPost]
        public ActionResult UpdateArticleRefereesLookUp(FormCollection collection)
        {
            try
            {
                var refereesId = new List<Guid>();
                var enumerable = collection.AllKeys.FirstOrDefault(s => s.Equals("SelectedReferee"));
                if (enumerable != null)
                {
                    var strings = collection[enumerable].Split(',');
                    foreach (var variable in strings)
                    {
                        refereesId.Add(variable.ToGuid());
                    }
                }
                var articleId = collection["ArticleId"].ToGuid();
                if (CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.AssigneArticleToRefreeCartabl(this.Homa.Id, articleId, SessionParameters.User.Id, refereesId))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }

                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        [RadynAuthorize]
        public ActionResult ModifyArticle(Guid articleId, string state)
        {
            ViewBag.State = state;
            ViewBag.Id = articleId;

            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ModifyArticle(FormCollection collection)
        {
            var articleId = collection["Id"].ToGuid();
            var obj = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);
            var facade = new ArticleFacade();
            try
            {




                this.RadynTryUpdateModel(obj, collection);
                obj.CurrentUICultureName = collection["LanguageId"];
                var value = collection["Comments"];
                HttpPostedFileBase attachment = null;
                HttpPostedFileBase abstractFile = null;
                HttpPostedFileBase orginalFile = null;
                if (Session["FlowAttachment"] != null)
                {
                    attachment = (HttpPostedFileBase)Session["FlowAttachment"];
                    Session.Remove("FlowAttachment");
                }
                if (Session["AbstractFileId"] != null)
                {
                    abstractFile = (HttpPostedFileBase)Session["AbstractFileId"];
                    Session.Remove("AbstractFileId");
                }
                if (Session["OrginalFileId"] != null)
                {
                    orginalFile = (HttpPostedFileBase)Session["OrginalFileId"];
                    Session.Remove("OrginalFileId");
                }
                var postFormData = this.PostForFormGenerator(collection);
                if (CongressComponent.Instance.BaseInfoComponents.ArticleFacade.AdminUpdate(
                        SessionParameters.User.Id, obj, (List<ArticleAuthors>)Session["ArticleAuthors"],
                        value, attachment, orginalFile, abstractFile, postFormData))
                {

                    this.ClearFormGeneratorData(postFormData.Id);
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                       messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }

                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        public ActionResult DeleteArticle(Guid articleId)
        {

            var obj = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Delete(obj))
                {

                    ShowMessage(Resources.Congress.SuccesfullyDetele, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
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
        public ActionResult RecycleArticle(Guid articleId)
        {

            var obj = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);
            obj.IsArchive = false;
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.ArticleFacade.AdminUpdate(obj))
                {
                    if (obj.IsArchive == false)
                    {
                        ShowMessage(Resources.Congress.SentToArchive, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                        return Content("true");
                    }

                    ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Error);
                }

                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        public ActionResult DeleteArticleData(Guid articleId)
        {

            var obj = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Get(articleId);
            obj.IsArchive = true;
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.ArticleFacade.AdminUpdate(obj))
                {
                    if (obj.IsArchive == true)
                    {
                        ShowMessage(Resources.Congress.SentToArchive, Resources.Common.MessaageTitle,
                                   messageIcon: MessageIcon.Succeed);
                        return Content("true");
                    }
                    ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);

                    return Content("false");
                }

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);

                return Content("false");
            }
            return Content("false");
        }

        #endregion

        #region Payment
        private void GetValue(Temp decryptVariables)
        {
            var list = string.IsNullOrEmpty(decryptVariables.PayType) ? null : decryptVariables.PayType.Split('-').ToList();
            ViewBag.PayTypes = list != null ? list.Select(variable => variable.ToByte()).ToList() : null;
            ViewBag.Accounts = new SelectList(CongressComponent.Instance.BaseInfoComponents.CongressAccountFacade.SelectKeyValuePair(x => x.AccountId, x => x.Account.AccountNo + " " + "(" + x.Account.Bank.Title + ")", x => x.CongressId == this.Homa.Id), "Key", "Value");


        }
        [CongressUserAuthorize]
        public ActionResult Payment(Guid Id)
        {
            var decryptVariables = Extentions.DecryptVariables(Id);
            try
            {
                decryptVariables.AdditionalData = CongressComponent.Instance.BaseInfoComponents.CongressDiscountTypeFacade.FillTempAdditionalData(this.Homa.Id);
                PaymentComponenets.Instance.TempFacade.Update(decryptVariables);
                GetValue(decryptVariables);
                return View(decryptVariables);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                GetValue(decryptVariables);

                return View();
            }
        }
        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult Payment(Guid Id, FormCollection collection)
        {
            var decryptVariables = Extentions.DecryptVariables(Id);
            try
            {
                var userobj = SessionParameters.CongressUser;
                var tr = new Transaction();
                this.RadynTryUpdateModel(tr, collection);
                tr.PayTypeId = (byte)collection["PayTypeId"].ToEnum<Radyn.Payment.Tools.Enums.PayType>();
                if (!string.IsNullOrEmpty(collection["PayDate"]))
                    tr.PayDate = DateTime.Parse(DateTimeUtil.ShamsiDateToGregorianDate(collection["PayDate"]).ToString("yyyy-MM-dd ") + collection["PayTime"]);


                HttpPostedFileBase DocScanId = null;
                if (Session["DocScanId"] != null)
                {
                    DocScanId = (HttpPostedFileBase)Session["DocScanId"];
                    Session.Remove("DocScanId");
                }
                if (tr.PayTypeId.Equals((byte)Radyn.Payment.Tools.Enums.PayType.Documnet))
                {
                    var documnetPay = PaymentComponenets.Instance.TransactionFacade.DocumnetPay(decryptVariables.Id, tr, DocScanId);
                    if (documnetPay != null)
                    {


                        return Redirect("~" + documnetPay.CallBackUrl);
                    }
                    ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                    GetValue(decryptVariables);
                    return View(decryptVariables);
                }

                if (string.IsNullOrEmpty(collection["bankId"]))
                {
                    ShowMessage(Resources.Payment.PleaseSelectBank, Resources.Common.MessaageTitle,
                          messageIcon: MessageIcon.Error);
                    GetValue(decryptVariables);
                    return View(decryptVariables);
                }

                tr.OnlineBankId = (Byte)collection["bankId"].ToEnum<Radyn.PaymentGateway.Tools.Enums.Bank>();
                string withGateway;
                if (string.IsNullOrEmpty(decryptVariables.TerminalId))
                {
                    withGateway =
                        Radyn.PaymentGateway.PaymentGatewayComponenets.Instance.GeneralFacade.OnlinePay(
                            decryptVariables.Id, tr, Request.Url.Authority + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath);
                }
                else
                {
                    withGateway =
                        Radyn.PaymentGateway.PaymentGatewayComponenets.Instance.GeneralFacade.OnlinePay(
                            decryptVariables.Id, tr, Request.Url.Authority + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath,
                            decryptVariables.MerchantId, decryptVariables.TerminalId, decryptVariables.TerminalUserName, decryptVariables.TerminalPassword, decryptVariables.CertificatePath, decryptVariables.CertificatePassword, decryptVariables.MerchantPublicKey, decryptVariables.MerchantPrivateKey);
                    var transobj =
                        PaymentComponenets.Instance.TransactionFacade.FirstOrDefault(
                            c =>
                                c.PayDate == tr.PayDate && c.PayerId == userobj.Id &&
                                c.PayTypeId.Equals((byte)Radyn.Payment.Tools.Enums.PayType.OnlinePay));
                    if (transobj != null)
                    {
                        if (!userobj.HasSuccedPayment)
                        {
                            userobj.TransactionId = transobj.Id;
                            userobj.PaymentTypeDaysInfo = collection["PayDate"] + "/" + collection["PayTime"];
                            CongressComponent.Instance.BaseInfoComponents.UserFacade.Update(userobj);
                        }

                    }

                }

                return Redirect(withGateway);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                GetValue(decryptVariables);
                return View(decryptVariables);
            }

        }

        #endregion

        [RadynAuthorize]
        [SourceCodeFile("ImageBrowser Controller", "~/Controllers/ImageBrowserController.cs")]
        public ActionResult UserInform()
        {
            GetInformValues();
            return View();


        }
        private void GetInformValues()
        {
            var config = this.Homa.Configuration;
            ViewBag.str =
                StringUtils.Encrypt(config.SMSAccountId + "," + config.SMSAccountUserName + "," + config.SMSAccountPassword);
            ViewBag.InformTypes = new SelectList(CongressComponent.Instance.BaseInfoComponents.UserFacade.GetUserInforms(this.Homa.Id), "Key",
                "Value");
            ViewBag.Credit = MessageComponenet.Instance.SMSFacade.AccountCredit(config.SMSAccountId,
                config.SMSAccountUserName, config.SMSAccountPassword).ToString("n0");
        }
        public ActionResult SearchUser(string filter, string InformType = "")
        {
            var users = !string.IsNullOrEmpty(InformType)
                ? CongressComponent.Instance.BaseInfoComponents.UserFacade.SearchUserWithInformType(this.Homa.Id, InformType)
                : CongressComponent.Instance.BaseInfoComponents.UserFacade.SearchText(this.Homa.Id, filter, Enums.AscendingDescending.Ascending, Enums.SortAccordingToUser.RegisterDate);
            ViewBag.CheckAll = !string.IsNullOrEmpty(InformType);
            return PartialView("PartialViewUserSearchResult", users);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UserInform(FormCollection collection)
        {
            GetInformValues();
            try
            {
                var messageModel = new Radyn.Message.Tools.ModelView.MessageModel();
                this.RadynTryUpdateModel(messageModel, collection);
                var list = new List<Guid>();
                if (messageModel.SendEmail == false && messageModel.SendSMS == false && messageModel.SendIntrenalMessage == false)
                {
                    ShowMessage(Resources.Congress.PleaseSelectSendType, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                    return View();
                }

                var variables = collection.AllKeys.Where(x => x.StartsWith("CheckSelect-"));
                foreach (var VARIABLE in variables)
                {
                    list.Add(collection[VARIABLE].ToGuid());
                }


                if (list.Count == 0)
                {
                    ShowMessage(Resources.Congress.PleaseSelectUser, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                    return View();
                }
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.InformUser(this.Homa.Id, messageModel, list))
                {
                    ShowMessage(Resources.Congress.SuccedInformationSend, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return View();
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return View();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                ViewBag.Message = ex.Message;

                return View();
            }
        }

        public ActionResult SendNewletter(int newsId)
        {
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.NewsLetterFacade.SentToUser(this.Homa.Id, newsId))
                {
                    ShowMessage(Resources.Congress.SuccedInformationSend, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", messageIcon: MessageIcon.Security);
                ViewBag.Message = ex.Message;

                return Content("false");
            }
        }
        [HttpPost]
        public ActionResult ChangeArtilceStatusAsGroup(string articles, FinalState status)
        {
            try
            {
                List<Guid> guids = articles.Split(',').Select(i => i.ToGuid()).ToList();
                if (!CongressComponent.Instance.BaseInfoComponents.ArticleFacade.UpdateStatus(this.Homa.Id, guids, status))
                {
                    ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Error);
                    return Content("true");

                }
                ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                       messageIcon: MessageIcon.Succeed);
                return Content("false");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return Content("false");
            }

        }

        public ActionResult ArticleArchive()
        {
            var articles = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Where(x => x.IsArchive == true);
            return View(articles);
        }
    }
}
