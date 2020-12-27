using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Common.Component;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.FormGenerator.Tools;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class ChipsFoodController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.Where(x => x.CongressId == this.Homa.Id);
            if (list.Count == 0) return this.Redirect("~/Congress/ChipsFood/Create");
            return View(list);
        }
        public ActionResult GetDaysInfo(Guid? Id)
        {
            var list = CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.GetDaysInfo(this.Homa.Id, Id);
            return PartialView("PVDaysInfo", list);
        }

        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult GetDetail(Guid Id)
        {
            var userRegisterPaymentType = CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.Get(Id);
            if (userRegisterPaymentType == null) return Content("false");
            return PartialView("PVDetails", userRegisterPaymentType);
        }
        public ActionResult GetModify(Guid? Id, string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            ViewBag.culture = culture;
            if (!Id.HasValue) return PartialView("PVModify", new ChipsFood());
            var languageContent = CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.GetLanuageContent(culture,Id);
            return PartialView("PVModify", languageContent);
        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var chipsFood = new ChipsFood();
            try
            {
                this.RadynTryUpdateModel(chipsFood);
                chipsFood.CurrentUICultureName = collection["LanguageId"];
               var keyValuePairs = new List<int>();
                var selectChipFood = collection["SelectChipFood"];
                if (!string.IsNullOrEmpty(selectChipFood))
                {
                    var split = selectChipFood.Split(',');
                    foreach (var variable in split)
                    {
                        if (string.IsNullOrEmpty(variable)) continue;
                        keyValuePairs.Add(variable.ToInt());
                    }
                }
                chipsFood.CongressId = this.Homa.Id;
                if (CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.Insert(chipsFood, keyValuePairs))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = chipsFood.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return this.Redirect("~/Congress/ChipsFood/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(chipsFood);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var chipsFood = CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(chipsFood);
                chipsFood.CurrentUICultureName = collection["LanguageId"];
                var keyValuePairs = new List<int>();
                var selectChipFood = collection["SelectChipFood"];
                if (!string.IsNullOrEmpty(selectChipFood))
                {
                    var split = selectChipFood.Split(',');
                    foreach (var variable in split)
                    {
                        if (string.IsNullOrEmpty(variable)) continue;
                        keyValuePairs.Add(variable.ToInt());
                    }
                }
                if (CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.Update(chipsFood, keyValuePairs))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return this.Redirect("~/Congress/ChipsFood/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
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
                if (CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.Redirect("~/Congress/ChipsFood/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return this.Redirect("~/Congress/ChipsFood/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }
        private void GetUserSearchValue()
        {
            ViewBag.status =
           EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.UserStatus>().Select(
               keyValuePair =>
                   new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.UserStatus>(),
                       keyValuePair.Value));
            ViewBag.Gender =
             EnumUtils.ConvertEnumToIEnumerableInLocalization<Radyn.EnterpriseNode.Tools.Enums.Gender>().Select(
                 keyValuePair =>
                     new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>(),
                         keyValuePair.Value));
            ViewBag.PaymentType =
                CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.Where(
                    type => type.CongressId == this.Homa.Id);
         
        }
        public ActionResult JoinUser(Guid Id)
        {
            ViewBag.Id = Id;
            GetUserSearchValue();
            return View();
        }
        [HttpPost]
        public ActionResult JoinUser(Guid Id, FormCollection collection)
        {

            try
            {
                var list = new List<Guid>();
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("Checkselect"));
                if (!string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    var strings = collection[firstOrDefault].Split(',');
                    foreach (var key in strings)
                    {
                        if (string.IsNullOrEmpty(key)) continue;
                        list.Add(key.ToGuid());
                    }
                }
                if (CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.JoinUser(Id, list))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return this.Redirect("~/Congress/ChipsFood/JoinUser?Id="+Id);
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return this.Redirect("~/Congress/ChipsFood/JoinUser?Id="+Id);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                GetUserSearchValue();
                return View();
            }
        }

        public ActionResult SearchUserFoods(FormCollection collection)
        {
            try
            {
                var txtSearch = collection["txtSearch"];
                var chipFoodId = collection["chipFoodId"];
                var status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null : collection["SearchStatus"].ToByte();
                var gender = string.IsNullOrEmpty(collection["Gender"]) ? Radyn.EnterpriseNode.Tools.Enums.Gender.None : collection["Gender"].ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>();
                var paymentTypeId = string.IsNullOrEmpty(collection["PaymentTypeId"]) ? (Guid?)null :
                collection["PaymentTypeId"].ToGuid();
                var user = new User { RegisterDate = collection["RegisterDate"], StatusNullable = status, PaymentTypeId = paymentTypeId };
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.SearchChipFood(this.Homa.Id, chipFoodId.ToGuid(), txtSearch, user, gender, postFormData);
                return PartialView("PartialViewUserSearchResult", list);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        public ActionResult GetAllChipFoods(Guid Id)
        {
            try
            {
                var list = CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.SearchChipFood(this.Homa.Id, Id, null, null, Radyn.EnterpriseNode.Tools.Enums.Gender.None,null);
                return PartialView("PartialViewUserSearchResult", list);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
    }
}
