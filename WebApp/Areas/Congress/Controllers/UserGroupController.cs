using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.Payment.Tools;
using Radyn.Utility;
using Radyn.Web.Html;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.FormGenerator.Tools;
using Enums = Radyn.Congress.Tools.Enums;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class UserGroupController : CongressBaseController
    {
        [CongressUserAuthorize]
        public ActionResult Index()
        {

            GetUserSearchValue();
            return View();
        }

        public ActionResult EncryptValue(string value)
        {

            if (string.IsNullOrEmpty(value) || value.ToInt() <= 0)
                return Json(new { Result = false, Value = string.Empty }, JsonRequestBehavior.AllowGet);
            return Json(new { Result = true, Value = StringUtils.Encrypt(value) }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            try
            {
                var txtSearch = collection["txtSearch"];
                var status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null :
                   collection["SearchStatus"].ToByte();
                var gender = string.IsNullOrEmpty(collection["Gender"]) ? Radyn.EnterpriseNode.Tools.Enums.Gender.None : collection["Gender"].ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>();
                var paymentTypeId = string.IsNullOrEmpty(collection["PaymentTypeId"]) ? (Guid?)null :
                collection["PaymentTypeId"].ToGuid();
                var user = new User { RegisterDate = collection["RegisterDate"], StatusNullable = status, PaymentTypeId = paymentTypeId, ParentId = SessionParameters.CongressUser.Id };
                var userFacade = CongressComponent.Instance.BaseInfoComponents.UserFacade;
                var currentuser = userFacade.Get(SessionParameters.CongressUser.Id);
                var list = new List<User> { currentuser };
                list.AddRange(userFacade.Search(this.Homa.Id, txtSearch, user, Enums.AscendingDescending.Ascending, Enums.SortAccordingToUser.RegisterDate, gender).ToList());
                ViewBag.HasPayment = this.Homa.Configuration.HasUserPayment;
                ViewBag.AllowEdit = true;
                return PartialView("PartialViewIndex", list);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        [HttpPost]
        public ActionResult PaymentGroup(FormCollection collection)
        {
            try
            {
                var users = new List<User>();
                if (string.IsNullOrEmpty(collection["UserPaymentType"]))
                {
                    ShowMessage(Resources.Congress.PleaseEnterUserPaymentType, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                    return Redirect("~/Congress/UserGroup/Index");
                }
                var paymentype = collection["UserPaymentType"].ToGuid();
                var config = this.Homa.Configuration;
                var transactionDiscountAttaches = Payment.Tools.AppExtentions.FillTransactionDiscount(collection);
                if (transactionDiscountAttaches.Count > config.DisscountCount)
                {
                    ShowMessage(Resources.Congress.YouCanNotUseDiscoutOverThanMax + Tag.NewLine + Resources.Congress.DiscountCount + config.DisscountCount, Resources.Common.MessaageTitle,
                     messageIcon: MessageIcon.Warning);
                    return Json(new { Result = false, Url = "" });
                }
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("CheckSelect"));
                if (string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    ShowMessage(Resources.Congress.PleaseSelectUser, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                    return Json(new { Result = false, Url = "" });
                }
                var strings = collection[firstOrDefault].Split(',');
                foreach (var value in strings)
                {
                    if (string.IsNullOrEmpty(value)) continue;
                    var user = new User() { Id = value.ToGuid(),ChairId = null};
                    users.Add(user);
                }
                if (users.Count == 0)
                {
                    ShowMessage(Resources.Congress.PleaseSelectUser, Resources.Common.MessaageTitle,
                     messageIcon: MessageIcon.Warning);
                    return Json(new { Result = false, Url = "" });
                }
                var guids = collection["SelectedChairId"];
                if (!string.IsNullOrEmpty(guids))
                {
                    var value = guids.Split(',');
                    for (var i = 0; i < value.Count(); i++)
                    {
                        if (string.IsNullOrEmpty(value[i])) continue;
                        users[i].ChairId = value[i].ToGuid();
                    }
                }
                var dictionary = new Dictionary<int, decimal>();

                const string selectregistertype = "SelectRegisterType-";
                foreach (var variable in collection.AllKeys.Where(x => x.StartsWith(selectregistertype)))
                {
                    var key = variable.Substring(selectregistertype.Length, variable.Length - selectregistertype.Length);
                    dictionary.Add(key.ToInt(), collection[variable].ToDecimal());
                }
                if (dictionary.Count == 0)
                {
                    ShowMessage(Resources.Congress.PleaseEnterUserPaymentType, Resources.Common.MessaageTitle,
                     messageIcon: MessageIcon.Warning);
                    return Json(new { Result = false, Url = "" });
                }
                var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.GroupPayment(this.Homa.Id,
                    SessionParameters.CongressUser, paymentype, users, transactionDiscountAttaches,
                   "/Congress/UserGroup/UpdateStatusAfterTransactionGroupPay?tempId=", dictionary);
                if (list.Key)
                {
                    if (list.Value != Guid.Empty)
                    {

                        return Json(new { Result = true, Url = Extentions.PrepaymenyUrl(list.Value) });
                    }
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                 messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = true, Url = "/Congress/UserGroup/Index" });

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Succeed);
                return Json(new { Result = false, Url = "" });
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Json(new { Result = false, Url = "" });
            }
        }

        public ActionResult UpdateStatusAfterTransactionGroupPay(Guid tempId)
        {
            try
            {
                var tr = CongressComponent.Instance.BaseInfoComponents.UserFacade.UpdateStatusAfterTransactionGroupPay(this.Homa.Id, SessionParameters.CongressUser, tempId);
                return tr != Guid.Empty
         ? Redirect("~/Payment/Transaction/TransactionResult?Id=" + tr +
                    "&callbackurl=/Congress/UserGroup/Index")
                    : Redirect("~/Congress/UserGroup/Index");


            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/UserGroup/Index");

            }
        }


        private void GetUserSearchValue()
        {
            ViewBag.SearchstatusList =
             EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.UserStatus>().Select(
                 keyValuePair =>
                     new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.UserStatus>(),
                         keyValuePair.Value));
            ViewBag.GenderList =
             EnumUtils.ConvertEnumToIEnumerableInLocalization<Radyn.EnterpriseNode.Tools.Enums.Gender>().Select(
                 keyValuePair =>
                     new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>(),
                         keyValuePair.Value));
            ViewBag.PaymentTypes =
                CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.GetValidListUser(this.Homa.Id);
            var enumerable = CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.GetParents(this.Homa.Id, true);
            ViewBag.HasHall = enumerable != null && enumerable.Any();
            ViewBag.CongressHalls = new SelectList(enumerable, "Id", "Name");
        }
        [CongressUserAuthorize]
        public ActionResult ImportFromExcel()
        {
            return View();
        }
        [CongressUserAuthorize]
        public ActionResult GetImportData()
        {
            
            HttpPostedFileBase file = null;
            if (Session["Image"] != null)
            {
                file = (HttpPostedFileBase)Session["Image"];
                Session.Remove("Image");
            }
            var importFromExcel = CongressComponent.Instance.BaseInfoComponents.UserFacade.ImportFromExcel(file, this.Homa.Id, SessionParameters.CongressUser.Id);
            Session["UserList"] = importFromExcel;
            return PartialView("PVImportFromExcel", importFromExcel);
        }
        public ActionResult RemoveUser(Guid Id)
        {
            if (Session["UserList"] == null) return Content("false");
            var list = (Dictionary<User, List<string>>)Session["UserList"];
            var any = list.FirstOrDefault(x => x.Key.Id == Id);
            if (any.Key == null) return Content("false");
            list.Remove(any.Key);
            return Content("true");
        }
        [HttpPost]
        public ActionResult ImportFromExcel(FormCollection collection)
        {
            try
            {

                var users = new List<User>();
                if (Session["UserList"] == null) RedirectToAction("Index");
                var list = (Dictionary<User, List<string>>)Session["UserList"];
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("Checkselect"));
                if (string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    ShowMessage(Resources.Common.No_results_found, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                    return Redirect("~/Congress/UserGroup/Index");

                }
                var strings = collection[firstOrDefault].Split(',');
                foreach (var vale in strings)
                {
                    if (string.IsNullOrEmpty(vale)) continue;
                    var orDefault = list.Keys.FirstOrDefault(x => x.Id == vale.ToGuid());
                    if (orDefault != null)
                    {
                        orDefault.ParentId = SessionParameters.CongressUser.Id;
                        users.Add(orDefault);
                    }
                }

                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.InsertList(users))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    Session.Remove("UserList");
                    return Redirect("~/Congress/UserGroup/Index");

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/UserGroup/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View();
            }
        }





        [CongressUserAuthorize]
        public ActionResult Details(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [CongressUserAuthorize]
        public ActionResult Create()
        {
            return View();
        }
        [CongressUserAuthorize]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var user = new User(){EnterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode { RealEnterpriseNode = new RealEnterpriseNode() }};
            try
            {
                
                this.RadynTryUpdateModel(user);
                this.RadynTryUpdateModel(user.EnterpriseNode);
                this.RadynTryUpdateModel(user.EnterpriseNode.RealEnterpriseNode);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                user.CongressId = this.Homa.Id;
                user.Status = (byte)Enums.UserStatus.Register;
                user.ParentId = SessionParameters.CongressUser.Id;
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return Content("false");
                }
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.Insert(user, postFormData, file))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    this.ClearFormGeneratorData(postFormData.Id);
                    return Json(new { Result = true, Url = this.CallBackRedirect(collection, new { Id = user.Id }) }, JsonRequestBehavior.AllowGet);
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Json(new { Result = false, Url = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Json(new { Result = false, Url = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }
        [CongressUserAuthorize]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            var Id = collection["Id"].ToGuid();
            var user = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(user);
                this.RadynTryUpdateModel(user.EnterpriseNode);
                this.RadynTryUpdateModel(user.EnterpriseNode.RealEnterpriseNode);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }

                var postFormData = this.PostForFormGenerator(collection);
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.Update(user, postFormData, file))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    this.ClearFormGeneratorData(postFormData.Id);
                    return Json(new { Result = true, Url = this.CallBackRedirect(collection, new { Id = user.Id }) }, JsonRequestBehavior.AllowGet);

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Json(new { Result = false, Url = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return Json(new { Result = false, Url = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }

        [CongressUserAuthorize]
        public ActionResult Delete(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }


        public ActionResult DeleteUser(Guid Id)
        {
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return Content("false");
            }
        }



    }
}