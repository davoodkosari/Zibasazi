using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms.VisualStyles;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.FileManager;
using Radyn.Security.Facade;
using Radyn.Utility;
using Radyn.Web.Html;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.FormGenerator.Tools;
using Stimulsoft.Report;
using Radyn.FormGenerator;
using Radyn.Web.Mvc.Utility;
using Radyn.WebApp.Areas.Congress.Tools;
using ModelView = Radyn.Message.Tools.ModelView;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class UserController : CongressBaseController
    {


        [RadynAuthorize]
        public ActionResult Index()
        {

            GetUserSearchValue();

            return View();
        }
        public ActionResult LookUPSimilarUser()
        {
            return View();
        }
        public ActionResult LookUpDetails(Guid Id)
        {

            ViewBag.Id = Id;
            return View();
        }

        public ActionResult GetSimilarUser()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.GetSimilarUser(this.Homa.Id);
            return PartialView("PVSimilarUserIndex", list);
        }
        public ActionResult MergeUsers(FormCollection collection)
        {
            try
            {

                if (string.IsNullOrEmpty(collection["SourceUserId"])) return Content("false");
                var sourceUserId = collection["SourceUserId"].ToGuid();
                var model = collection.AllKeys.FirstOrDefault(x => x.Equals("CheckSelect"));
                if (string.IsNullOrEmpty(collection[model])) return Content("false");
                var userIdlist = collection[model].Split(',');
                var list = new List<Guid>();
                foreach (var s in userIdlist)
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    list.Add(s.ToGuid());

                }
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.MergUsers(sourceUserId, list))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
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
        public ActionResult GetChilds(Guid id, bool enableedit = false)
        {
            var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.GetChildsDynamic(this.Homa.Id, id);
            GetIndexViewBags(enableedit);
            return PartialView("PartialViewIndex", list);
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            try
            {
                var txtSearch = collection["txtSearch"];
                var status = string.IsNullOrEmpty(collection["Search_Status"]) ? (byte?)null :
                   collection["Search_Status"].ToByte();
                var gender = string.IsNullOrEmpty(collection["Search_Gender"]) ? Radyn.EnterpriseNode.Tools.Enums.Gender.None : collection["Search_Gender"].ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>();
                var chairStatus = string.IsNullOrEmpty(collection["Search_ChairStatus"]) ? Enums.UserChairStatus.None : collection["Search_ChairStatus"].ToEnum<Enums.UserChairStatus>();
                var paymentTypeId = string.IsNullOrEmpty(collection["Search_PaymentTypeId"]) ? (Guid?)null :
                collection["Search_PaymentTypeId"].ToGuid();
                var user = new User { UserChairStatus = chairStatus, RegisterDate = collection["RegisterDate"], StatusNullable = status, PaymentTypeId = paymentTypeId };
                var sortuser = collection["SortUser"].ToEnum<Enums.SortAccordingToUser>();
                var ascendingDescending = collection["AscendingDescending"].ToEnum<Enums.AscendingDescending>();
                var postFormData = this.PostForFormGenerator(collection);
                user.HasOtherPayState = collection["HasValueStatus"].ToEnum<Enums.HasValue>();
                var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.SearchDynamic(this.Homa.Id, txtSearch, user, ascendingDescending, sortuser, gender, postFormData);
                GetIndexViewBags(true);
                return PartialView("PartialViewIndex", list);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }



        public ActionResult GetAllUser()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.Where(x => x.CongressId == this.Homa.Id);
            GetIndexViewBags(true);
            return PartialView("PartialViewIndex", list);
        }
        private void GetIndexViewBags(bool enableedit = false)
        {
            ViewBag.SearchstatusList =
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.UserStatus>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.UserStatus>(),
                            keyValuePair.Value));

            ViewBag.enableedit = enableedit;
            ViewBag.Halls = CongressComponent.Instance.BaseInfoComponents.CongressHallFacade.SelectKeyValuePair(x => x.HallId, x => x.Hall.Name, x => x.CongressId == this.Homa.Id);
        }
        private void GetUserSearchValue()
        {
          
            ViewBag.HasValueList =
                EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.HasValue>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.HasValue>(),
                            keyValuePair.Value));
            ViewBag.SearchstatusList =
             EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.UserStatus>().Select(
                 keyValuePair =>
                     new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.UserStatus>(),
                         keyValuePair.Value));
            ViewBag.UserChairStatusList =
             EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.UserChairStatus>().Select(
                 keyValuePair =>
                     new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.UserChairStatus>(),
                         keyValuePair.Value));
            ViewBag.GenderList =
             EnumUtils.ConvertEnumToIEnumerableInLocalization<Radyn.EnterpriseNode.Tools.Enums.Gender>().Select(
                 keyValuePair =>
                     new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>(),
                         keyValuePair.Value));
            ViewBag.PaymentTypeList =
                CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.SelectKeyValuePair(x => x.Id, x => x.Title,
                    type => type.CongressId == this.Homa.Id);

            ViewBag.SortUser =
                 EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.SortAccordingToUser>().Where(c => c.Key != "0").Select(
                   keyValuePair =>
                       new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.SortAccordingToUser>(),
                           keyValuePair.Value));
            ViewBag.AscendingDescending =
                 EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.AscendingDescending>().Where(c => c.Key != "1").Select(
                 keyValuePair =>
                     new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.AscendingDescending>(),
                         keyValuePair.Value));
        }
        [HttpPost]
        public ActionResult UpdateList(FormCollection collection)
        {
            try
            {
                var list = new List<User>();
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("ModelId"));
                if (!string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    var strings = collection[firstOrDefault].Split(',');
                    foreach (var key in strings)
                    {
                        if (string.IsNullOrEmpty(key)) continue;
                        var status = collection["Status-" + key.ToGuid()].ToByte();
                        var oldstatus = collection["oldstatus-" + key.ToGuid()].ToByte();
                        var oldChair = String.IsNullOrEmpty(collection["OldChairId-" + key.ToGuid()]) ? (Guid?)null : collection["OldChairId-" + key.ToGuid()].ToGuid();
                        var chair = String.IsNullOrEmpty(collection["ChairId-" + key.ToGuid()]) ? (Guid?)null : collection["ChairId-" + key.ToGuid()].ToGuid();
                        if (oldstatus != status || oldChair != chair)
                        {
                            list.Add(new User()
                            {
                                Id = key.ToGuid(),
                                Status = oldstatus != status ? status : oldstatus,
                                ChairId = oldChair != chair ? chair : oldChair
                            });
                        }
                    }
                }
                var userFacade = CongressComponent.Instance.BaseInfoComponents.UserFacade;
                if (userFacade.UpdateList(this.Homa.Id,list))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    if (list.All(x => x.Status != (byte)Enums.UserStatus.PayConfirm) ||
                        !this.Homa.Configuration.AllowUserPrintCard)
                        return Content("true");
                    var homaFacade = CongressComponent.Instance.BaseInfoComponents.HomaFacade;
                    var @where = userFacade.Where(x => x.Id.In(list.Select(i => i.Id)));
                    foreach (var keyValuePair in list.Where(x => x.Status == (byte)Enums.UserStatus.PayConfirm))
                    {
                        
                        try
                        {
                            if (this.Homa.Configuration.UserRegisterInformType == (byte?) Enums.UserInformType.Email ||
                                this.Homa.Configuration.UserRegisterInformType == (byte?) Enums.UserInformType.Both)
                            {
                                var user = @where.FirstOrDefault(x => x.Id == keyValuePair.Id);
                                if (user == null) continue;
                                var data = StringUtils.Encrypt(keyValuePair.Id + "," + this.Homa.Id);
                                var value = user.PaymentTypeId.HasValue ? user.PaymentType.Title : this.Homa.Configuration.CartTypeEmptyValue;
                                var messageModel = new ModelView.MessageModel
                                {
                                    Email = user.EnterpriseNode.Email,
                                    EmailBody =
                                        "<a   href=\"http://" + Request.Url.Authority +
                                        Radyn.Web.Mvc.UI.Application.CurrentApplicationPath +
                                        "/Congress/ReportPanel/EmailPrintUserCard?value=" + data +
                                        "\"   style=\"font-size: 30px;font-weight: bold;color: green\">" +
                                        Resources.Congress.ClickForViewAndDownloadyourCard + "</a>",
                                    EmailTitle =
                                        string.Format(Resources.Congress.Card + "{0}" + " " + "{1}", value ?? "",
                                            this.Homa.CongressTitle),
                                    SendEmail = true
                                };
                                homaFacade.SendInform(this.Homa, (byte)Enums.UserInformType.Email, messageModel);
                            }
                            
                        }
                        catch 
                        {
                            

                        }
                    }

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
        public ActionResult ImportFromExcel()
        {
            if (this.Homa!= null && SessionParameters.CurrentCongress.Configuration != null &&
                !this.Homa.Configuration.AccessToUserImportFromExcel)
                return  this.Redirect("~/Account/AccessDeny");
            return View();
        }
        public ActionResult GetImportData()
        {
            HttpPostedFileBase file = null;
            if (Session["Image"] != null)
            {
                file = (HttpPostedFileBase)Session["Image"];
                Session.Remove("Image");
            }
            var importFromExcel = CongressComponent.Instance.BaseInfoComponents.UserFacade.ImportFromExcel(file, this.Homa.Id);
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
                    return Redirect("~/Congress/User/Index");
                }
                var strings = collection[firstOrDefault].Split(',');
                foreach (var vale in strings)
                {
                    if (string.IsNullOrEmpty(vale)) continue;
                    var orDefault = list.Keys.FirstOrDefault(x => x.Id == vale.ToGuid());
                    if (orDefault != null)
                        users.Add(orDefault);
                }
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.InsertList(users))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    Session.Remove("UserList");
                    return Redirect("~/Congress/User/Index");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/User/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View();
            }
        }


        [RadynAuthorize]
        public JsonResult SearchUser(string value)
        {

            List<dynamic> priceList = new List<dynamic>();
            priceList.AddRange(CongressComponent.Instance.BaseInfoComponents.UserFacade.Select(new Expression<Func<User, object>>[]
            {
                x => x.Id,
                x => x.Username,
                x=>x.EnterpriseNode.RealEnterpriseNode.FirstName+" "+x.EnterpriseNode.RealEnterpriseNode.LastName,
            }, x =>x.CongressId==this.Homa.Id&& ((x.EnterpriseNode.RealEnterpriseNode.FirstName + " " + x.EnterpriseNode.RealEnterpriseNode.LastName).Contains(value) || x.Number.ToString().Contains(value))));
            List<object> result = new List<object>();
            foreach (dynamic item in priceList)
            {
                result.Add(new { key = (Guid)item.Id, value = (string)item.FirstNameAndLastName + " (" + item.Username + ")" });

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ImportFromXml()
        {
            if (this.Homa != null && SessionParameters.CurrentCongress.Configuration != null &&
                !this.Homa.Configuration.AccessToUserImportFromExcel)
                return this.Redirect("~/Account/AccessDeny");
            return View();
        }
        public ActionResult GetXmlImportData()
        {
            HttpPostedFileBase file = null;
            if (Session["Xmlfile"] != null)
            {
                file = (HttpPostedFileBase)Session["Xmlfile"];
                Session.Remove("Xmlfile");
            }
            var importFromExcel = CongressComponent.Instance.BaseInfoComponents.UserFacade.ImportFromXml(file);
            Session["UserXmlList"] = importFromExcel;
            return PartialView("PVImportFromXml", importFromExcel);
        }
        public ActionResult RemoveUserXml(Guid Id)
        {
            if (Session["UserXmlList"] == null) return Content("false");
            var list = (Dictionary<User, List<string>>)Session["UserXmlList"];
            var any = list.FirstOrDefault(x => x.Key.Id == Id);
            if (any.Key == null) return Content("false");
            list.Remove(any.Key);
            return Content("true");
        }
        [HttpPost]
        public ActionResult ImportFromXml(FormCollection collection)
        {
            try
            {

                var users = new List<User>();
                if (Session["UserXmlList"] == null) RedirectToAction("Index");
                var list = (Dictionary<User, List<string>>)Session["UserXmlList"];
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("Checkselect"));
                if (string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    ShowMessage(Resources.Common.No_results_found, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                    return Redirect("~/Congress/User/Index");
                }
                var strings = collection[firstOrDefault].Split(',');
                foreach (var vale in strings)
                {
                    if (string.IsNullOrEmpty(vale)) continue;
                    var orDefault = list.Keys.FirstOrDefault(x => x.Id == vale.ToGuid());
                    if (orDefault != null)
                        users.Add(orDefault);
                }
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.UpdateUserAttendance(users))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    Session.Remove("UserXmlList");
                    return Redirect("~/Congress/User/Index");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/User/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View();
            }
        }


        public void DownLoadAsExcel(System.Data.DataTable myList, Homa homa)
        {
            string fileName = homa.CongressTitle + "UserList.xls";
            DataGrid dg = new DataGrid();
            dg.AllowPaging = false;
            dg.DataSource = myList;
            dg.DataBind();
            Response.Clear();
           Response.Buffer = true;
            
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.Charset = "";
            Response.AddHeader("Content-Disposition",
              "attachment; filename=" + fileName);
            Response.ContentType =
              "application/vnd.ms-excel";
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlTextWriter =
              new System.Web.UI.HtmlTextWriter(stringWriter);
            dg.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        public ActionResult ExportToExcel()
        {
            try
            {
                var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.Where(x => x.CongressId == this.Homa.Id);
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                var model = CongressComponent.Instance.BaseInfoComponents.UserFacade.ReportFormDataForExcel(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new List<object>(list), "User", new[] { "Id" });
                DownLoadAsExcel(model, homa);
                //string csv = String.Join(",", list.Select(x => x.ToString()).ToArray());
                return Redirect("~/Congress/User/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/User/Index");
            }
        }


        public ActionResult GetModify(Guid? Id, bool viewChild = false, string isadmin = null,bool showpassword=true,bool ShowImage=true)
        {
            var model = Id.HasValue ? CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(Id) : new User() { EnterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode() { RealEnterpriseNode = new RealEnterpriseNode() } };
            ViewBag.viewChild = viewChild;
            ViewBag.PrefixTitleList =
                 new SelectList(EnterpriseNodeComponent.Instance.PrefixTitleFacade.GetAll(), "Id", "DescriptionField", model.EnterpriseNode.PrefixTitleId);
            this.TempData.Clear();
            var admin = (!string.IsNullOrEmpty(isadmin) && StringUtils.Decrypt(isadmin).ToBool());
            if (admin)
                ViewBag.paymentTypes = new SelectList(CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.SelectKeyValuePair(x => x.Id, x => x.Title, x => x.CongressId == this.Homa.Id && x.Capacity > 0), "Key", "Value");
            ViewBag.IsAdmin = admin;
            ViewBag.IsDirty = Id.HasValue;
            ViewBag.showpassword = showpassword;
            ViewBag.ShowImage = ShowImage;
            return PartialView("PVModify", model);
        }

        public ActionResult GetDetails(Guid Id, bool viewChild = false)
        {
            ViewBag.viewChild = viewChild;
            return PartialView("PVDetails", CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(Id));
        }
        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult RegisterInNewModify(Guid userId, bool checkedModify)
        {

            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.NewsLetterFacade.RegsiterCongressUserModify(this.Homa.Id, userId, checkedModify))
                {
                    ShowMessage(checkedModify ? Resources.Common.YourSubscriptionToTheNewsletterAccepted : Resources.Common.YourSubscriptionToTheNewsletterCanceled, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(checkedModify ? Resources.Common.ErrorInInsert : Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            return View();
        }

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
                var postFormData = this.PostForFormGenerator(collection);
                if (!string.IsNullOrEmpty(postFormData.FillErrors))
                {
                    ShowMessage(postFormData.FillErrors, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return Json(new { responseState = false }, JsonRequestBehavior.AllowGet);
                }
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.Insert(user, postFormData, file))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    this.ClearFormGeneratorData(postFormData.Id);
                    return Json(new {responseState = true , userId = user.Id} , JsonRequestBehavior.AllowGet);
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Json(new { responseState = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Json(new { responseState = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [RadynAuthorize]
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
                var list = new List<User>();
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("ModelId"));
                if (!string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    var strings = collection[firstOrDefault].Split(',');
                    foreach (var key in strings)
                    {
                        if (string.IsNullOrEmpty(key)) continue;
                        var status = collection["Status-" + key.ToGuid()].ToByte();
                        var oldstatus = collection["oldstatus-" + key.ToGuid()].ToByte();
                        var oldChair = String.IsNullOrEmpty(collection["OldChairId-" + key.ToGuid()]) ? (Guid?)null : collection["OldChairId-" + key.ToGuid()].ToGuid();
                        var chair = String.IsNullOrEmpty(collection["ChairId-" + key.ToGuid()]) ? (Guid?)null : collection["ChairId-" + key.ToGuid()].ToGuid();
                        if (oldstatus != status || oldChair != chair)
                        {
                            list.Add(new User()
                            {
                                Id = key.ToGuid(),
                                Status = oldstatus != status ? status : oldstatus,
                                ChairId = oldChair != chair ? chair : oldChair
                            });
                        }

                    }
                }
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.Update(user, postFormData, file, list))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    this.ClearFormGeneratorData(postFormData.Id);
                    return Content("true");

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return Content("false");
            }
        }



        public ActionResult DeleteUser(Guid Id)
        {
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.UserFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
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
        [HttpPost]
        public ActionResult Validate(FormCollection collection)
        {

            var messageStack = new List<string>();
            var Id = collection["Id"].ToGuid();
            var user = new User();
            var enterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode
            {
                RealEnterpriseNode = new RealEnterpriseNode()
            };
            this.RadynTryUpdateModel(user);
            this.RadynTryUpdateModel(enterpriseNode);
            this.RadynTryUpdateModel(enterpriseNode.RealEnterpriseNode);
            if (Id == Guid.Empty)
            {
                if (string.IsNullOrEmpty(user.Password))
                    messageStack.Add(Resources.Congress.Please_Enter_Password);
                if (string.IsNullOrEmpty(collection["RepeatPassword"]))
                    messageStack.Add(Resources.Congress.Please_Enter_Password_Repeat);
                else if (!string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(collection["RepeatPassword"]))
                    if (user.Password != collection["RepeatPassword"])
                        messageStack.Add(Resources.Congress.Password_and_Repeat_Not_Equal);
            }
            else
            {
                if (!string.IsNullOrEmpty(user.Password))
                {
                    if (string.IsNullOrEmpty(collection["RepeatPassword"]))
                        messageStack.Add(Resources.Congress.Please_Enter_Password_Repeat);
                    else if (!string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(collection["RepeatPassword"]))
                        if (user.Password != collection["RepeatPassword"])
                            messageStack.Add(Resources.Congress.Password_and_Repeat_Not_Equal);

                }
            }
            if (user.Password != null && !string.IsNullOrEmpty(user.Password) && user.Password.Length < 6)
                messageStack.Add(Resources.Congress.MinimumPasswordCharacter);

            if (string.IsNullOrEmpty(enterpriseNode.Email))
                messageStack.Add(Resources.Congress.PleaseEnterYourEmail);
            else
            {
                if (!Utility.Utils.IsEmail(enterpriseNode.Email))
                    messageStack.Add(Resources.Congress.UnValid_Enter_Email);
            }
            if (string.IsNullOrEmpty(user.Username))
                messageStack.Add(Resources.Congress.PleaseInsertUserName);
            if (string.IsNullOrEmpty(enterpriseNode.RealEnterpriseNode.FirstName))
                messageStack.Add(Resources.Congress.Please_Enter_YourName);
            if (string.IsNullOrEmpty(enterpriseNode.RealEnterpriseNode.LastName))
                messageStack.Add(Resources.Congress.Please_Enter_YourLastName);
            if (string.IsNullOrEmpty(enterpriseNode.Cellphone))
                messageStack.Add(Resources.Congress.Please_Enter_YourMobile);
            else
            {
                if (!string.IsNullOrEmpty(enterpriseNode.Cellphone) && ((!enterpriseNode.Cellphone.StartsWith("09") && !enterpriseNode.Cellphone.StartsWith("+") && !enterpriseNode.Cellphone.StartsWith("00")) || ((enterpriseNode.Cellphone.Length < 11) && (enterpriseNode.Cellphone.Length > 15)) || enterpriseNode.Cellphone.ToLong() == 0))
                    messageStack.Add(Resources.Congress.MobileNumberIsNotValid);
            }
            if (enterpriseNode.RealEnterpriseNode.Gender == null)
                messageStack.Add(Resources.Congress.Please_Enter_YourGender);

            if(!string.IsNullOrEmpty(enterpriseNode.RealEnterpriseNode.NationalCode) && !Radyn.Utility.Utils.ValidNationalID(enterpriseNode.RealEnterpriseNode.NationalCode))
                messageStack.Add(Resources.Congress.PleaseEnterRightNationalCode);
            var postFormData = this.PostForFormGenerator(collection);
            if (!string.IsNullOrEmpty(postFormData.FillErrors))
            {
                ShowMessage(postFormData.FillErrors, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                return Content("false");
            }

            var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
            if (messageBody != "")
            {
                ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                return Content("false");
            }
            return Content("true");

        }


    }
}