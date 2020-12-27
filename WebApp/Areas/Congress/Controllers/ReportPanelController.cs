using Ionic.Zip;
using Radyn.Congress;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.FileManager;
using Radyn.FormGenerator;
using Radyn.Framework;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.Web.Parser;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.Congress.Tools;
using Radyn.WebApp.Areas.FormGenerator.Tools;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using File = Radyn.FileManager.DataStructure.File;


namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class ReportPanelController : CongressBaseController
    {

        [RadynAuthorize]
        public ActionResult AdminEditReport()
        {
            var definition =
                CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
            return View(definition);
        }
        public ActionResult UploadImage(HttpPostedFileBase fileBase)
        {
            var congressDefinition =
                   CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(
                       this.Homa.Id);
            var enumerable = congressDefinition.GetType().GetProperties().Where(x => x.Name.ToLower().StartsWith("rpt"));
            foreach (var propertyInfo in enumerable)
            {

                var httpPostedFileBase = Request.Files[propertyInfo.Name + "-Uploader"];
                if (httpPostedFileBase != null && httpPostedFileBase.InputStream != null)
                    Session[propertyInfo.Name] = httpPostedFileBase;
            }

            return Content("");
        }
        [HttpPost]
        public ActionResult AdminEditReport(FormCollection collection)
        {
            var dictionary = new Dictionary<string, HttpPostedFileBase>();
            try
            {
                var congressDefinition =
                    CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(
                        this.Homa.Id);
                this.RadynTryUpdateModel(congressDefinition);
                var enumerable = congressDefinition.GetType().GetProperties().Where(x => x.Name.ToLower().StartsWith("rpt"));
                foreach (var propertyInfo in enumerable)
                {
                    if (Session[propertyInfo.Name] != null)
                    {
                        dictionary.Add(propertyInfo.Name, (HttpPostedFileBase)Session[propertyInfo.Name]);
                        Session.Remove(propertyInfo.Name);
                    }
                }
                if (CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.ModifyReports(congressDefinition, dictionary))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/ReportPanel/AdminEditReport");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                          messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/ReportPanel/AdminEditReport");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/ReportPanel/AdminEditReport");
            }
        }

        public ActionResult RemoveImage(HttpPostedFileBase fileBase, string filename)
        {
            Session.Remove(filename);
            return Content("");
        }




        [RadynAuthorize]
        public ActionResult Index()
        {
            ViewBag.year = new SelectList(this.Homa.GetCongressYear());
            ViewBag.Moth = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<Radyn.Common.Definition.Enums.PersianMonth>(), "Key", "Value");
            ViewBag.ChartTypes = new SelectList(EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.ChartEnums>(), "Key", "Value");
            return View();
        }
        public ActionResult GetChart(string chartTypeId, string year, string moth)
        {
            try
            {
                var authority = Request.Url.Authority;
                var url = authority + "/";
               var chartModels = CongressComponent.Instance.ReportComponents.CongressReport.GetChart(chartTypeId.ToEnum<Enums.ChartEnums>(), this.Homa.Id,
                       year, moth, url);
                ViewBag.title = chartTypeId.ToEnum<Enums.ChartEnums>().GetDescriptionInLocalization();
                var array = chartModels.Select(x => x.Value).ToArray();
                var objects = chartModels.Select(x => x.Count).ToArray();
                ViewBag.ArgumantDataColum = objects.JsonSerializer();
                ViewBag.ValueDataColum = array.JsonSerializer();
                return PartialView("PartialViewChart", chartModels);
            }
            catch (Exception exception)
            {
                ViewBag.message = exception.Message;
                return Content("false");
            }
        }


        public ActionResult GetPrintChart(string chartTypeId, string year, string moth)
        {
            try
            {
                var stiReport1 = new StiReport();
                var obj = new Object();
                var authority = Request.Url.Authority;
                var url = authority + "/Home/index";
                var chartModels = CongressComponent.Instance.ReportComponents.CongressReport.GetChart(chartTypeId.ToEnum<Enums.ChartEnums>(), this.Homa.Id,
                       year, moth, url);
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptArticleByChart.mrt"));
                obj = new { title = chartTypeId.ToEnum<Enums.ChartEnums>().GetDescriptionInLocalization() };

                stiReport1.RegBusinessObject("Model", chartModels);
                stiReport1.RegBusinessObject("Title", obj);
                SessionParameters.Report = stiReport1;
                return Content("true");

            }
            catch (Exception exception)
            {
                ViewBag.message = exception.Message;
                return Content("false");
            }
        }


        #region Booth

        public ActionResult DesginBoothOfficerCard()
        {

            var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
            var stiReport1 = new StiReport();
            if (congressDefinition.RptBoothOfficerId == null)
            {
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptBoothOfficerCard.mrt"));
                stiReport1.RegBusinessObject("Model", new ModelView.UserCardModel());
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                var file = new File
                {
                    Content = stiReport1.SaveToByteArray(),
                    ContentType = stiReport1.GetType().Name,
                    Extension = "mrt",
                    FileName = "RptBoothOfficerCard"
                };

                congressDefinition.RptBoothOfficerId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                if (!CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(congressDefinition))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptBoothOfficerId);
                if (file1 == null)
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                stiReport1.Load(file1.Content);
                stiReport1.RegBusinessObject("Model", new ModelView.UserCardModel());
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                file1.Content = stiReport1.SaveToByteArray();
                if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                {
                    ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                           messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptBoothOfficerId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintBoothOfficerCards(Guid userId, Guid boothId)
        {

            try
            {

                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptBoothOfficerId == null)
                {
                    ShowMessage(Resources.Congress.BoothOfficerCardNotDesgineInCongress, Resources.Common.MessaageTitle,
                           messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var list =
                    CongressComponent.Instance.BaseInfoComponents.BoothOfficerFacade.GetCardList(boothId, userId);
                var stiReport1 = new StiReport();
                var fileid = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptBoothOfficerId);
                if (fileid == null) return Content("false");
                stiReport1.Load(fileid.Content);
                stiReport1.RegBusinessObject("Model", list);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        public ActionResult PrintBoothOfficerCard(Guid Id, Guid userId, Guid boothId)
        {
            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptBoothOfficerId == null)
                {
                    ShowMessage(Resources.Congress.BoothOfficerCardNotDesgineInCongress, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var list =
                    CongressComponent.Instance.BaseInfoComponents.BoothOfficerFacade.GetBoothOfficerCard(Id, boothId, userId);
                var stiReport1 = new StiReport();
                var fileid = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptBoothOfficerId);
                if (fileid == null) return Content("false");
                stiReport1.Load(fileid.Content);
                stiReport1.RegBusinessObject("Model", list);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        public ActionResult PrintBoothOfficerList()
        {
            try
            {


                var list = CongressComponent.Instance.BaseInfoComponents.BoothOfficerFacade.Where(x => x.Booth.CongressId == this.Homa.Id);
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                var stiReport1 = new StiReport();
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressBoothOfficerList.mrt"));
                stiReport1.RegBusinessObject("Model", list);
                stiReport1.RegBusinessObject("Title", homa);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        public ActionResult PrintBoothList()
        {
            try
            {

                var list =
                 CongressComponent.Instance.BaseInfoComponents.BoothFacade.GetByCongressId(this.Homa.Id);
                var objects = new List<dynamic>();
                if (list.Any())
                {
                    objects =
                        CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.GroupBy(
                            new Expression<Func<UserBooth, object>>[] { x => x.BoothId },
                            new GroupByModel<UserBooth>[]
                            {
                                new GroupByModel<UserBooth>()
                                {
                                    Expression = x => x.Transaction.Amount,
                                    AggrigateFuntionType = AggrigateFuntionType.Sum
                                }
                            }, x => x.Booth.CongressId == this.Homa.Id && x.TransactionId.HasValue && x.Transaction.Done);
                }
                foreach (var booth in list)
                {
                    var firstOrDefault = objects.FirstOrDefault(x => x.BoothId == booth.Id);
                    if (firstOrDefault != null && firstOrDefault.SumAmount is decimal)
                        booth.ReservAmount = firstOrDefault.SumAmount;
                }
                var stiReport1 = new StiReport();
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressBoothList.mrt"));
                stiReport1.RegBusinessObject("Model", list);
                stiReport1.RegBusinessObject("Title", homa);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        [HttpPost]
        public ActionResult PrintBoothReservList(FormCollection collection)
        {
            try
            {

                if (string.IsNullOrEmpty(collection["BoothId"])) return Content("false");
                var Id = collection["BoothId"].ToGuid();
                byte? status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null :
                       (byte)collection["SearchStatus"].ToEnum<Enums.RezervState>();
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Search(Id, status, collection["RegisterDate"], collection["txtSearch"], postFormData);
                foreach (var userBooth in list)
                    userBooth.BoothOfficerNames =
                        CongressComponent.Instance.BaseInfoComponents.BoothOfficerFacade.GetNamesByBoothId(userBooth);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/Booth/Booth", new List<object>(list), new[] { "UserId", "BoothId" });
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                var stiReport1 = new StiReport();
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.RegBusinessObject("Title", homa);
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptUserBoothId == null)
                {
                    stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressUserBoothList.mrt"));
                    var file = new File
                    {
                        Content = stiReport1.SaveToByteArray(),
                        ContentType = stiReport1.GetType().Name,
                        Extension = "mrt",
                        FileName = "RptBoothReportId"
                    };
                    congressDefinition.RptUserBoothId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                    if (!CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(congressDefinition))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                    messageIcon: MessageIcon.Succeed);
                        return Content("false");
                    }
                }
                var fileid = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptUserBoothId);
                if (fileid != null)
                    stiReport1.Load(fileid.Content);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        public ActionResult DesginUserBoothReport()
        {
            try
            {
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Booth/Booth", new UserBooth(), null, false);
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                var stiReport1 = new StiReport();

                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptUserBoothId == null)
                {
                    stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressUserBoothList.mrt"));
                    stiReport1.RegBusinessObject("Model", model);
                    stiReport1.RegBusinessObject("Title", homa);
                    stiReport1.Dictionary.SynchronizeBusinessObjects();
                    var file = new File
                    {
                        Content = stiReport1.SaveToByteArray(),
                        ContentType = stiReport1.GetType().Name,
                        Extension = "mrt",
                        FileName = "RptBoothReportId"
                    };
                    congressDefinition.RptUserBoothId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                    if (
                        !CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(
                            congressDefinition))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptUserBoothId);
                    if (file1 == null)
                        return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                    stiReport1.Load(file1.Content);
                    stiReport1.RegBusinessObject("Model", model);
                    stiReport1.RegBusinessObject("Title", homa);
                    stiReport1.Dictionary.SynchronizeBusinessObjects();
                    file1.Content = stiReport1.SaveToByteArray();
                    if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                    {
                        ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                               messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                    }

                }
                return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptUserBoothId }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        #endregion

        #region WorkShop

        public ActionResult PrintWorkShopList()
        {
            try
            {
                var workShopFacade = CongressComponent.Instance.BaseInfoComponents.WorkShopFacade;
                var workShopUserFacade = CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade;
                var list = workShopFacade.GetByCongressId(this.Homa.Id);
                var groupBy = workShopUserFacade.GroupBy(new Expression<Func<WorkShopUser, object>>[] { x => x.WorkShopId },
                    new GroupByModel<WorkShopUser>[]
                    {
                        new GroupByModel<WorkShopUser>()
                        {
                            Expression = x => x.Transaction.Amount,
                            AggrigateFuntionType = AggrigateFuntionType.Sum
                        },
                    },
                    z => z.WorkShop.CongressId == this.Homa.Id && z.TransactionId.HasValue && z.Transaction.Done);
                foreach (var workShop in list)
                {
                    var sum = groupBy.FirstOrDefault(x => x.WorkShopId == workShop.Id);
                    if (sum != null && sum.SumAmount is decimal)
                        workShop.ReservAmount = sum.SumAmount;

                }
                var stiReport1 = new StiReport();
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressWorkShopList.mrt"));
                stiReport1.RegBusinessObject("Model", list);
                stiReport1.RegBusinessObject("Title", homa);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        [HttpPost]
        public ActionResult PrintUserRequestWorkShops(FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["WorkShopId"])) return Content("false");
                var guid = collection["WorkShopId"].ToGuid();
                byte? status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null :
                       (byte)collection["SearchStatus"].ToEnum<Enums.WorkShopRezervState>();
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.WorkShopUserFacade.Search(guid, status, collection["RegisterDate"], collection["txtSearch"], postFormData);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/WorkShop/WorkShop", new List<object>(list), new[] { "UserId", "WorkShopId" });
                var workShop = CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.Get(guid);
                var stiReport1 = new StiReport();
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.RegBusinessObject("Title", workShop);
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptWorkShopUserId == null)
                {
                    stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressUserWorkShopList.mrt"));

                    var file = new File
                    {
                        Content = stiReport1.SaveToByteArray(),
                        ContentType = stiReport1.GetType().Name,
                        Extension = "mrt",
                        FileName = "RptWorkShopReportId"
                    };
                    congressDefinition.RptWorkShopUserId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                    if (!CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(congressDefinition))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                    messageIcon: MessageIcon.Succeed);
                        return Content("false");
                    }
                }
                var fileid = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptWorkShopUserId);
                if (fileid == null) return Content("false");
                stiReport1.Load(fileid.Content);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        [HttpPost]
        public ActionResult DesginWorkShopUserReport(FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["WorkShopId"])) return Content("false");
                var guid = collection["WorkShopId"].ToGuid();
                var workShop = CongressComponent.Instance.BaseInfoComponents.WorkShopFacade.Get(guid);

                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/WorkShop/WorkShop", new WorkShopUser(), null, false);
                var stiReport1 = new StiReport();
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptWorkShopUserId == null)
                {
                    stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressUserWorkShopList.mrt"));
                    stiReport1.RegBusinessObject("Model", model);
                    stiReport1.RegBusinessObject("Title", workShop);
                    stiReport1.Dictionary.SynchronizeBusinessObjects();
                    var file = new File
                    {
                        Content = stiReport1.SaveToByteArray(),
                        ContentType = stiReport1.GetType().Name,
                        Extension = "mrt",
                        FileName = "RptWorkShopReportId"
                    };
                    congressDefinition.RptWorkShopUserId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                    if (
                        !CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(
                            congressDefinition))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptWorkShopUserId);
                    if (file1 == null)
                        return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                    stiReport1.Load(file1.Content);
                    stiReport1.RegBusinessObject("Model", model);
                    stiReport1.RegBusinessObject("Title", workShop);
                    stiReport1.Dictionary.SynchronizeBusinessObjects();
                    file1.Content = stiReport1.SaveToByteArray();
                    if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                    {
                        ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                               messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                    }

                }
                return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptWorkShopUserId }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        #endregion

        #region Hotel
        [HttpPost]
        public ActionResult PrintUserRequestHotels(FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["HotelId"])) return Content("false");
                var hotelId = collection["HotelId"].ToGuid();
                byte? status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null :
                       (byte)collection["SearchStatus"].ToEnum<Enums.RezervState>();
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.Search(hotelId, status, collection["RegisterDate"], collection["txtSearch"], postFormData);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/Hotel/Hotel", new List<object>(list), new[] { "UserId", "HotelId" });
                var hotel = CongressComponent.Instance.BaseInfoComponents.HotelFacade.Get(hotelId);
                var stiReport1 = new StiReport();
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.RegBusinessObject("Title", hotel);
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptHotelUserId == null)
                {
                    stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressUserHotelList.mrt"));
                    var file = new File
                    {
                        Content = stiReport1.SaveToByteArray(),
                        ContentType = stiReport1.GetType().Name,
                        Extension = "mrt",
                        FileName = "RptUserReportId"
                    };
                    congressDefinition.RptHotelUserId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                    if (
                        !CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(
                            congressDefinition))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Succeed);
                        return Content("false");
                    }
                }
                var fileid = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptHotelUserId);
                if (fileid == null) return Content("false");
                stiReport1.Load(fileid.Content);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        [HttpPost]
        public ActionResult DesginUserHotelReport(FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["HotelId"])) return Content("false");
                var hotelId = collection["HotelId"].ToGuid();
                var hotel = CongressComponent.Instance.BaseInfoComponents.HotelFacade.Get(hotelId);

                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Hotel/Hotel", new HotelUser(), null, false);
                var stiReport1 = new StiReport();

                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptHotelUserId == null)
                {
                    stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressUserHotelList.mrt"));
                    stiReport1.RegBusinessObject("Model", model);
                    stiReport1.RegBusinessObject("Title", hotel);
                    stiReport1.Dictionary.SynchronizeBusinessObjects();
                    var file = new File
                    {
                        Content = stiReport1.SaveToByteArray(),
                        ContentType = stiReport1.GetType().Name,
                        Extension = "mrt",
                        FileName = "RptUserReportId"
                    };
                    congressDefinition.RptHotelUserId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                    if (
                        !CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(
                            congressDefinition))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptHotelUserId);
                    if (file1 == null)
                        return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                    stiReport1.Load(file1.Content);
                    stiReport1.RegBusinessObject("Model", model);
                    stiReport1.RegBusinessObject("Title", hotel);
                    stiReport1.Dictionary.SynchronizeBusinessObjects();
                    file1.Content = stiReport1.SaveToByteArray();
                    if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                    {
                        ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                               messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                    }

                }
                return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptHotelUserId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        public ActionResult PrintHotelList()
        {
            try
            {
                var hotelFacade = CongressComponent.Instance.BaseInfoComponents.HotelFacade;
                var list =
                 hotelFacade.GetByCongressId(this.Homa.Id);

                var objects = new List<dynamic>();
                if (list.Any())
                {
                    objects =
                        CongressComponent.Instance.BaseInfoComponents.HotelUserFacade.GroupBy(
                            new Expression<Func<HotelUser, object>>[] { x => x.HotelId },
                            new GroupByModel<HotelUser>[]
                            {
                                new GroupByModel<HotelUser>()
                                {
                                    Expression = x => x.Transaction.Amount,
                                    AggrigateFuntionType = AggrigateFuntionType.Sum
                                }
                            }, x => x.Hotel.CongressId == this.Homa.Id && x.TransactionId.HasValue && x.Transaction.Done);
                }
                foreach (var hotel in list)
                {
                    var firstOrDefault = objects.FirstOrDefault(x => x.HotelId == hotel.Id);
                    if (firstOrDefault != null && firstOrDefault.SumAmount is decimal)
                        hotel.ReservAmount = firstOrDefault.SumAmount;
                }
                var stiReport1 = new StiReport();
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressHotelList.mrt"));
                stiReport1.RegBusinessObject("Model", list);
                stiReport1.RegBusinessObject("Title", homa);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        #endregion


        #region ChipFood

        public ActionResult DesginChipFoodCard()
        {

            var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
            var stiReport1 = new StiReport();

            if (congressDefinition.RptChipFoodId == null)
            {
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptChipFoodCard.mrt"));
                stiReport1.RegBusinessObject("Model", new ModelView.UserCardModel());
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                var file = new File
                {
                    Content = stiReport1.SaveToByteArray(),
                    ContentType = stiReport1.GetType().Name,
                    Extension = "mrt",
                    FileName = "RptChipFoodCard"
                };

                congressDefinition.RptChipFoodId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                if (!CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(congressDefinition))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptChipFoodId);
                if (file1 == null)
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                stiReport1.Load(file1.Content);
                stiReport1.RegBusinessObject("Model", new ModelView.UserCardModel());
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                file1.Content = stiReport1.SaveToByteArray();
                if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                {
                    ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                           messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptChipFoodId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintUserChipFoodCard(Guid chipFoodId, Guid userId)
        {
            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptChipFoodId == null)
                {
                    ShowMessage(Resources.Congress.ChipFoodCardNotDesgineInCongress, Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return Redirect("~/Congress/ChipsFood/JoinUser?Id" + chipFoodId);
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptChipFoodId);
                if (file == null) return Redirect("~/Congress/ChipsFood/JoinUser?Id" + chipFoodId);

                var userCards = CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.SearchChipFoodReport(chipFoodId, userId);
                if (userCards != null)
                {

                    var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", userCards, "User", new[] { "Id" });
                    var stiReport1 = new StiReport();
                    stiReport1.Load(file.Content);
                    stiReport1.RegBusinessObject("Model", model);
                    stiReport1.Render(false);
                    var stream = new MemoryStream();
                    var settings = new StiPdfExportSettings { AutoPrintMode = StiPdfAutoPrintMode.None };
                    var service = new StiPdfExportService();
                    service.ExportPdf(stiReport1, stream, settings);
                    return File(stream.ToArray(), "application/pdf", string.Format("{0}.Card.pdf", "ChipFood"));

                }


                return Redirect("~/Congress/ChipsFood/JoinUser?Id" + chipFoodId);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/ChipsFood/JoinUser?Id" + chipFoodId);
            }
        }
        [HttpPost]
        public ActionResult PrintChipFoodList(FormCollection collection)
        {
            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptChipFoodId == null)
                {
                    ShowMessage(Resources.Congress.ChipFoodCardNotDesgineInCongress, Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptChipFoodId);
                if (file == null) return Content("false");
                var txtSearch = collection["txtSearch"];
                var chipsFoodId = collection["chipFoodId"];
                if (string.IsNullOrEmpty(chipsFoodId))
                {
                    ShowMessage(Resources.Congress.PleaseSelectChipFood, Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null :
                   (byte)collection["SearchStatus"].ToEnum<Enums.UserStatus>();
                var gender = string.IsNullOrEmpty(collection["Gender"])
                    ? Radyn.EnterpriseNode.Tools.Enums.Gender.None
                    : collection["Gender"].ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>();
                var paymentTypeId = string.IsNullOrEmpty(collection["PaymentTypeId"]) ? (Guid?)null :
                collection["PaymentTypeId"].ToGuid();
                var postFormData = this.PostForFormGenerator(collection);
                var user = new User { RegisterDate = collection["RegisterDate"], StatusNullable = status, PaymentTypeId = paymentTypeId };
                var list = CongressComponent.Instance.BaseInfoComponents.ChipsFoodFacade.SearchChipFoodReport(this.Homa.Id, chipsFoodId.ToGuid(), txtSearch, user, gender, postFormData);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new List<object>(list), "User", new[] { "Id" });
                var stiReport1 = new StiReport();
                stiReport1.Load(file.Content);
                stiReport1.RegBusinessObject("Model", model);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        #endregion

        #region User
        [HttpPost]
        public ActionResult PrintUserList(FormCollection collection)
        {
            var gender = string.IsNullOrEmpty(collection["Gender"]) ? Radyn.EnterpriseNode.Tools.Enums.Gender.None : collection["Gender"].ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>();
            try
            {
                var txtSearch = collection["txtSearch"];
                var status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null : collection["SearchStatus"].ToByte();
                var paymentTypeId = string.IsNullOrEmpty(collection["PaymentTypeId"]) ? (Guid?)null :
                collection["PaymentTypeId"].ToGuid();
                var user = new User { RegisterDate = collection["RegisterDate"], StatusNullable = status, PaymentTypeId = paymentTypeId };
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.Search(this.Homa.Id, txtSearch, user, Enums.AscendingDescending.Ascending, Enums.SortAccordingToUser.RegisterDate, gender, postFormData);
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new List<object>(list), "User", new[] { "Id" });
                var stiReport1 = new StiReport();
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.RegBusinessObject("Title", homa);
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptUserId == null)
                {
                    stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressUserList.mrt"));
                    var file = new File
                    {
                        Content = stiReport1.SaveToByteArray(),
                        ContentType = stiReport1.GetType().Name,
                        Extension = "mrt",
                        FileName = "RptUserReportId"
                    };
                    congressDefinition.RptUserId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                    if (!CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(congressDefinition))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                    messageIcon: MessageIcon.Succeed);
                        return Content("false");
                    }

                }
                var fileid = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptUserId);
                if (fileid == null) return Content("false");
                stiReport1.Load(fileid.Content);

                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        
        public ActionResult DesigneUserReport()
        {
            try
            {
                var stiReport1 = new StiReport();
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new User(), null, false);
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptUserId == null)
                {
                    stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressUserList.mrt"));
                    stiReport1.RegBusinessObject("Model", model);
                    stiReport1.RegBusinessObject("Title", homa);
                    stiReport1.Dictionary.SynchronizeBusinessObjects();
                    var file = new File
                    {
                        Content = stiReport1.SaveToByteArray(),
                        ContentType = stiReport1.GetType().Name,
                        Extension = "mrt",
                        FileName = "RptUserReport"
                    };
                    congressDefinition.RptUserId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                    if (
                        !CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(
                            congressDefinition))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptUserId);
                    if (file1 == null)
                        return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                    stiReport1.Load(file1.Content);
                    stiReport1.RegBusinessObject("Model", model);
                    stiReport1.RegBusinessObject("Title", homa);
                    stiReport1.Dictionary.SynchronizeBusinessObjects();
                    file1.Content = stiReport1.SaveToByteArray();
                    if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                    {
                        ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                               messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptUserId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        #endregion

        #region CardAndCongressCertificate

        [RadynAuthorize]
        public ActionResult UserCards()
        {

            GetUserSearchValue();
            return View();
        }


        [HttpPost]
        public ActionResult SearchUserCards(FormCollection collection)
        {
            try
            {
                var txtSearch = collection["txtSearch"];
                var status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null : collection["SearchStatus"].ToByte();
                var gender = string.IsNullOrEmpty(collection["Gender"])
                    ? Radyn.EnterpriseNode.Tools.Enums.Gender.None
                    : collection["Gender"].ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>();
                var paymentTypeId = string.IsNullOrEmpty(collection["PaymentTypeId"]) ? (Guid?)null :
                collection["PaymentTypeId"].ToGuid();
                var articleTypeId = string.IsNullOrEmpty(collection["ArticleTypeId"]) ? (Guid?)null :
                collection["ArticleTypeId"].ToGuid();
                var user = new User { RegisterDate = collection["RegisterDate"], StatusNullable = status, PaymentTypeId = paymentTypeId };
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.SearchCards(this.Homa.Id, txtSearch, user, articleTypeId, gender, postFormData);
                return PartialView("PartialViewUserIndex", list);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        [HttpPost]
        public ActionResult PrintCardList(FormCollection collection)
        {
            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptUserCardId == null)
                {
                    ShowMessage(Resources.Congress.UserCardNotDesgineInCongress, Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptUserCardId);
                if (file == null) return Content("false");
                var txtSearch = collection["txtSearch"];
                var status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null :
                   (byte)collection["SearchStatus"].ToEnum<Enums.UserStatus>();
                var gender = string.IsNullOrEmpty(collection["Gender"])
                    ? Radyn.EnterpriseNode.Tools.Enums.Gender.None
                    : collection["Gender"].ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>();
                var paymentTypeId = string.IsNullOrEmpty(collection["PaymentTypeId"]) ? (Guid?)null :
                collection["PaymentTypeId"].ToGuid();

                var articleTypeId = string.IsNullOrEmpty(collection["ArticleTypeId"]) ? (Guid?)null :
                collection["ArticleTypeId"].ToGuid();
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.SearchCards(this.Homa.Id, txtSearch, new User { RegisterDate = collection["RegisterDate"], StatusNullable = status, PaymentTypeId = paymentTypeId }, articleTypeId, gender, postFormData);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new List<object>(list), "User", new[] { "Id" });
                var stiReport1 = new StiReport();
                stiReport1.Load(file.Content);
                stiReport1.RegBusinessObject("Model", model);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        [HttpPost]
        public ActionResult PrintMiniCardList(FormCollection collection)
        {
            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptMiniUserCardId == null)
                {
                    ShowMessage(Resources.Congress.UserCardNotDesgineInCongress, Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptMiniUserCardId);
                if (file == null) return Content("false");
                var txtSearch = collection["txtSearch"];
                var status = string.IsNullOrEmpty(collection["SearchStatus"]) ? (byte?)null :
                   (byte)collection["SearchStatus"].ToEnum<Enums.UserStatus>();
                var gender = string.IsNullOrEmpty(collection["Gender"])
                    ? Radyn.EnterpriseNode.Tools.Enums.Gender.None
                    : collection["Gender"].ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>();
                var paymentTypeId = string.IsNullOrEmpty(collection["PaymentTypeId"]) ? (Guid?)null :
                collection["PaymentTypeId"].ToGuid();

                var articleTypeId = string.IsNullOrEmpty(collection["ArticleTypeId"]) ? (Guid?)null :
                collection["ArticleTypeId"].ToGuid();
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.SearchCards(this.Homa.Id, txtSearch, new User { RegisterDate = collection["RegisterDate"], StatusNullable = status, PaymentTypeId = paymentTypeId }, articleTypeId, gender, postFormData);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new List<object>(list), "User", new[] { "Id" });
                var stiReport1 = new StiReport();
                stiReport1.Load(file.Content);
                stiReport1.RegBusinessObject("Model", model);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        [HttpPost]
        public ActionResult PrintArticlesAbstract(FormCollection collection)
        {
            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptAbstractArticleId == null)
                {
                    ShowMessage(string.Format(Resources.Congress.UserArticleAbstractNotDesgineInCongress, this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptAbstractArticleId);
                if (file == null) return Content("false");
                var postFormData = this.PostForFormGenerator(collection);
                var article = Areas.Congress.Tools.AppExtention.PrepareArticleSearch(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.SearchArticle(this.Homa.Id, article, collection["txtSearch"], postFormData, Enums.AscendingDescending.Ascending, Enums.SortAccordingToArticle.DateOfSendingArticle);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new List<object>(list), "User", new[] { "Id" });

                var stiReport1 = new StiReport();
                stiReport1.Load(file.Content);
                stiReport1.RegBusinessObject("Model", model);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        [HttpPost]
        public ActionResult PrintCongressCertificateList(FormCollection collection)
        {
            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptCongressCertificateId == null)
                {
                    ShowMessage(Resources.Congress.CongressCertificateNotDesgineInCongress, Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptCongressCertificateId);
                if (file == null) return Content("false");
                var txtSearch = collection["txtSearch"];
                var gender = string.IsNullOrEmpty(collection["Gender"])
                    ? Radyn.EnterpriseNode.Tools.Enums.Gender.None
                    : collection["Gender"].ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>();
                var paymentTypeId = string.IsNullOrEmpty(collection["PaymentTypeId"]) ? (Guid?)null :
                collection["PaymentTypeId"].ToGuid();
                var articleTypeId = string.IsNullOrEmpty(collection["ArticleTypeId"]) ? (Guid?)null :
                collection["ArticleTypeId"].ToGuid();

                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.SearchCards(this.Homa.Id, txtSearch, new User { RegisterDate = collection["RegisterDate"], Status = (byte)Enums.UserStatus.ConfirmPresentInHoma, PaymentTypeId = paymentTypeId }, articleTypeId, gender, postFormData);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new List<object>(list), "User", new[] { "Id" });
                var stiReport1 = new StiReport();
                stiReport1.Load(file.Content);
                stiReport1.RegBusinessObject("Model", model);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        
        [HttpPost]
        public ActionResult PrintUserInfoCardList(FormCollection collection)
        {
            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptUserInfoCardId == null)
                {
                    //todo error message
                    //ShowMessage(Resources.Congress.CongressCertificateNotDesgineInCongress, Resources.Common.MessaageTitle,
                    //             messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptUserInfoCardId);
                if (file == null) return Content("false");
                var txtSearch = collection["txtSearch"];
                var gender = string.IsNullOrEmpty(collection["Gender"])
                    ? Radyn.EnterpriseNode.Tools.Enums.Gender.None
                    : collection["Gender"].ToEnum<Radyn.EnterpriseNode.Tools.Enums.Gender>();
                var paymentTypeId = string.IsNullOrEmpty(collection["PaymentTypeId"]) ? (Guid?)null :
                collection["PaymentTypeId"].ToGuid();
                var articleTypeId = string.IsNullOrEmpty(collection["ArticleTypeId"]) ? (Guid?)null :
                collection["ArticleTypeId"].ToGuid();

                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.UserFacade.SearchCards(this.Homa.Id, txtSearch, new User { RegisterDate = collection["RegisterDate"], Status = (byte)Enums.UserStatus.ConfirmPresentInHoma, PaymentTypeId = paymentTypeId }, articleTypeId, gender, postFormData);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new List<object>(list), "User", new[] { "Id" });
                var stiReport1 = new StiReport();
                stiReport1.Load(file.Content);
                stiReport1.RegBusinessObject("Model", model);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        [RadynAuthorize]
        public ActionResult DesginMiniCard()
        {

            var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
            var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new ModelView.UserCardModel(), null, false);
            var stiReport1 = new StiReport();
            if (congressDefinition.RptMiniUserCardId == null)
            {
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCard.mrt"));
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                var file = new File
                {
                    Content = stiReport1.SaveToByteArray(),
                    ContentType = stiReport1.GetType().Name,
                    Extension = "mrt",
                    FileName = "RptMiniCard"
                };
                congressDefinition.RptMiniUserCardId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                if (!CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(congressDefinition))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptMiniUserCardId);
                if (file1 == null)
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                stiReport1.Load(file1.Content);
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                file1.Content = stiReport1.SaveToByteArray();
                if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                {
                    ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                           messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptMiniUserCardId }, JsonRequestBehavior.AllowGet);
        }

        [RadynAuthorize]
        public ActionResult DesginAbstractArticle()
        {

            var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
            var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new ModelView.UserArticleAbstract(), null, false);
            var stiReport1 = new StiReport();
            if (congressDefinition.RptAbstractArticleId == null)
            {
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptAbstractArticle.mrt"));
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                var file = new File
                {
                    Content = stiReport1.SaveToByteArray(),
                    ContentType = stiReport1.GetType().Name,
                    Extension = "mrt",
                    FileName = "RptAbstractArticle"
                };
                congressDefinition.RptAbstractArticleId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                if (!CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(congressDefinition))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptAbstractArticleId);
                if (file1 == null)
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                stiReport1.Load(file1.Content);
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                file1.Content = stiReport1.SaveToByteArray();
                if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                {
                    ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                           messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptAbstractArticleId }, JsonRequestBehavior.AllowGet);
        }
        [RadynAuthorize]
        public ActionResult DesginCard()
        {
            var stiReport1 = new StiReport();
            var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
            var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new ModelView.UserCardModel(), null, false);
            if (congressDefinition.RptUserCardId == null)
            {
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCard.mrt"));
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.Dictionary.SynchronizeBusinessObjects();

                var file = new File
                {
                    Content = stiReport1.SaveToByteArray(),
                    ContentType = stiReport1.GetType().Name,
                    Extension = "mrt",
                    FileName = "RptCard"
                };
                congressDefinition.RptUserCardId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                if (!CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(congressDefinition))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptUserCardId);
                if (file1 == null)
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                stiReport1.Load(file1.Content);
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                file1.Content = stiReport1.SaveToByteArray();
                if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                {
                    ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                           messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptUserCardId }, JsonRequestBehavior.AllowGet);
        }
        [RadynAuthorize]
        public ActionResult DesginCongressCertifite()
        {


            var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
            var stiReport1 = new StiReport();
            var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new ModelView.UserCardModel(), null, false);
            if (congressDefinition.RptCongressCertificateId == null)
            {
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptCongressCertificate.mrt"));
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                var file = new File
                {
                    Content = stiReport1.SaveToByteArray(),
                    ContentType = stiReport1.GetType().Name,
                    Extension = "mrt",
                    FileName = "RptCongressCertificate"
                };
                congressDefinition.RptCongressCertificateId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                if (!CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(congressDefinition))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptCongressCertificateId);
                if (file1 == null)
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                stiReport1.Load(file1.Content);
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                file1.Content = stiReport1.SaveToByteArray();
                if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                {
                    ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                           messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptCongressCertificateId }, JsonRequestBehavior.AllowGet);
        }
        [RadynAuthorize]
        public ActionResult DsignUserInfoCard()
        {
            var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
            var stiReport1 = new StiReport();
            var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", new ModelView.UserCardModel(), null, false);
            if (congressDefinition.RptUserInfoCardId == null)
            {
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptUserInfoCard.mrt"));
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                var file = new File
                {
                    Content = stiReport1.SaveToByteArray(),
                    ContentType = stiReport1.GetType().Name,
                    Extension = "mrt",
                    FileName = "RptUserInfoCard"
                };
                congressDefinition.RptUserInfoCardId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                if (!CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(congressDefinition))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptUserInfoCardId);
                if (file1 == null)
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                stiReport1.Load(file1.Content);
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                file1.Content = stiReport1.SaveToByteArray();
                if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                {
                    ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                           messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptUserInfoCardId }, JsonRequestBehavior.AllowGet);

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
                CongressComponent.Instance.BaseInfoComponents.UserRegisterPaymentTypeFacade.SelectKeyValuePair(x => x.Id, x => x.Title,
                    type => type.CongressId == this.Homa.Id);

            ViewBag.ArticleTypes =
               CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.SelectKeyValuePair(x => x.Id, x => x.Title,
                   type => type.CongressId == this.Homa.Id);
        }

        [CongressUserAuthorize]
        public ActionResult UserCard()
        {


            try
            {

                var userCards = CongressComponent.Instance.BaseInfoComponents.UserFacade.GetUserCards(SessionParameters.CongressUser.Id);
                return View(userCards);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View();
            }
        }
        public ActionResult PrintUserCard(string type, string Id, bool Isadmin = false, bool Ismini = false)
        {
            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (Ismini ? congressDefinition.RptMiniUserCardId == null : congressDefinition.RptUserCardId == null)
                {
                    ShowMessage(Resources.Congress.UserCardNotDesgineInCongress, Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return Redirect("~" + (Isadmin ? "/Congress/ReportPanel/UserCards" : "/Congress/ReportPanel/UserCard"));
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(Ismini ? congressDefinition.RptMiniUserCardId : congressDefinition.RptUserCardId);

                if (file == null) return Redirect("~" + (Isadmin ? "/Congress/ReportPanel/UserCards" : "/Congress/ReportPanel/UserCard"));
                var userCards = CongressComponent.Instance.BaseInfoComponents.UserFacade.GetUserCards(SessionParameters.CongressUser != null ? SessionParameters.CongressUser.Id : Id.ToGuid(), Isadmin);
                if (userCards != null)
                {
                    var firstOrDefault = userCards.FirstOrDefault(x => x.CardId.ToLower() == type.ToLower() && x.Id == Id.ToString());
                    if (firstOrDefault != null)
                    {
                        var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", firstOrDefault, "User", new[] { "Id" });
                        var stiReport1 = new StiReport();
                        stiReport1.Load(file.Content);
                        stiReport1.RegBusinessObject("Model", model);
                        stiReport1.Render(false);
                        var stream = new MemoryStream();
                        var settings = new StiPdfExportSettings { AutoPrintMode = StiPdfAutoPrintMode.None };
                        var service = new StiPdfExportService();
                        service.ExportPdf(stiReport1, stream, settings);
                        return File(stream.ToArray(), "application/pdf", string.Format("{0}.Card.pdf", firstOrDefault.UseName));



                    }
                }


                return Redirect("~" + (Isadmin ? "/Congress/ReportPanel/UserCards" : "/Congress/ReportPanel/UserCard"));
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~" + (Isadmin ? "/Congress/ReportPanel/UserCards" : "/Congress/ReportPanel/UserCard"));
            }
        }


        public ActionResult PrintUserCardFromAdmin(string id)
        {
            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptUserCardId);
                if (file == null) return null;
                var userCards = CongressComponent.Instance.BaseInfoComponents.UserFacade.GetUserCards(SessionParameters.CongressUser != null ? SessionParameters.CongressUser.Id : id.ToGuid(), true);
                if (userCards != null)
                {
                    var firstOrDefault = userCards.FirstOrDefault(x => x.Id == id.ToString());
                    if (firstOrDefault != null)
                    {
                        var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", firstOrDefault, "User", new[] { "Id" });
                        var stiReport1 = new StiReport();
                        stiReport1.Load(file.Content);
                        stiReport1.RegBusinessObject("Model", model);
                        stiReport1.Render(false);
                        var stream = new MemoryStream();
                        var settings = new StiPdfExportSettings { AutoPrintMode = StiPdfAutoPrintMode.None };
                        var service = new StiPdfExportService();
                        service.ExportPdf(stiReport1, stream, settings);
                        return File(stream.ToArray(), "application/pdf", string.Format("{0}.Card.pdf", firstOrDefault.UseName));

                    }
                }
                return Redirect("~" + "/Congress/ReportPanel/UserCards");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~" + "/Congress/ReportPanel/UserCards");
            }
        }

        public ActionResult EmailPrintUserCard(string value)
        {

            try
            {
                var textToBeDecrypted = value;
                var decrypt = Utility.StringUtils.Decrypt(textToBeDecrypted);
                var type = decrypt.Split(',');
                var userId = type[0].ToGuid();
                var homaId = type[1].ToGuid();
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(homaId);
                if (congressDefinition.RptUserCardId == null)
                {
                    ShowMessage(Resources.Congress.UserCardNotDesgineInCongress, Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return this.Redirect("/ReportBuilder/ReportView");
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptUserCardId);
                if (file == null) return this.Redirect("/ReportBuilder/ReportView");
                var userCards = CongressComponent.Instance.BaseInfoComponents.UserFacade.GetUserCards(userId);
                if (userCards != null)
                {
                    var firstOrDefault = userCards.FirstOrDefault(x => x.CardType == Enums.CardType.RegisterCard && x.Id == userId.ToString());
                    if (firstOrDefault != null)
                    {

                        var stiReport1 = new StiReport();
                        var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete", firstOrDefault, "User", new[] { "Id" });
                        stiReport1.Load(file.Content);
                        stiReport1.RegBusinessObject("Model", model);
                        stiReport1.Render(false);
                        var stream = new MemoryStream();
                        var settings = new StiPdfExportSettings { AutoPrintMode = StiPdfAutoPrintMode.None };
                        var service = new StiPdfExportService();
                        service.ExportPdf(stiReport1, stream, settings);
                        return File(stream.ToArray(), "application/pdf", string.Format("{0}.Card.pdf", firstOrDefault.UseName));




                    }
                }
                return this.Redirect("/ReportBuilder/ReportView");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return this.Redirect("/ReportBuilder/ReportView");
            }
        }
        [CongressUserAuthorize]
        public ActionResult UserCongressCertificte()
        {

            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptCongressCertificateId == null)
                {
                    ShowMessage(Resources.Congress.CongressCertificateNotDesgineInCongress, Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return RedirectToAction("Home", "UserPanel", new { area = "Congress" });
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptCongressCertificateId);
                if (file == null) return Content("false");
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                var stiReport1 = new StiReport();
                stiReport1.Load(file.Content);
                var enterpriseNode = SessionParameters.CongressUser.EnterpriseNode;
                if (enterpriseNode != null && enterpriseNode.RealEnterpriseNode != null)
                {
                    var model =
                  FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(
                      AppExtention.CongressMoudelName + "-/Congress/Userpanel/Complete",
                      new ModelView.UserCardModel
                      {
                          FirstName = enterpriseNode.RealEnterpriseNode.FirstName,
                          LastName = enterpriseNode.RealEnterpriseNode.LastName,
                          NationalCode = enterpriseNode.RealEnterpriseNode.NationalCode,
                          CongressTitle = homa.CongressTitle
                      }, "User", new[] { "Id" });
                    stiReport1.RegBusinessObject("Model", model);
                }

                SessionParameters.Report = stiReport1;
                return this.Redirect("/ReportBuilder/ReportView");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return RedirectToAction("Home", "UserPanel", new { area = "Congress" });
            }
        }

        #endregion

        #region ArticleCertificate


        [RadynAuthorize]
        public ActionResult DesginArticleCertificate()
        {

            var congressDefinitionFacade = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade;
            var congressDefinition = congressDefinitionFacade.GetValidDefinition(this.Homa.Id);
            var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
            var stiReport1 = new StiReport();
            var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/ArticlePanel/Article", new ModelView.ArticleCertificateModel(), null, false);

            if (congressDefinition.RptArticleCertificateId == null)
            {
                stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/RptArticleCertificate.mrt"));
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.RegBusinessObject("Title", homa);
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                var file = new File
                {
                    Content = stiReport1.SaveToByteArray(),
                    ContentType = stiReport1.GetType().Name,
                    Extension = "mrt",
                    FileName = "RptArticleCertificate"
                };
                congressDefinition.RptArticleCertificateId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                if (!congressDefinitionFacade.Update(congressDefinition))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptArticleCertificateId);
                if (file1 == null)
                    return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                stiReport1.Load(file1.Content);
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.RegBusinessObject("Title", homa);
                stiReport1.Dictionary.SynchronizeBusinessObjects();
                file1.Content = stiReport1.SaveToByteArray();
                if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                {
                    ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                           messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptArticleCertificateId }, JsonRequestBehavior.AllowGet);
        }
        [RadynAuthorize]
        public ActionResult DownloaArticleZipFile()
        {
            if (Session["ArticleZipFile"] == null) return Content("false");
            ZipFile zipFile = (ZipFile)Session["ArticleZipFile"];
            Session.Remove("ArticleZipFile");
            this.Response.Clear();
            this.Response.BufferOutput = false;
            this.Response.ContentType = "application/zip";
            this.Response.ContentEncoding = Encoding.UTF8;
            var zipName = String.Format("ArticleZip_{0}.zip", DateTime.Now.ShamsiDate() + "-" + DateTime.Now.GetTime());
            this.Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
            zipFile.Save(this.Response.OutputStream);
            this.Response.End();
            return Content("true");
        }
        [HttpPost]
        public ActionResult DownloadAllarticleZipFile(FormCollection collection)
        {


            try
            {
                var article1 = Areas.Congress.Tools.AppExtention.PrepareArticleSearch(collection);
                var postFormData = this.PostForFormGenerator(collection);

                var allForZipFile =
                  CongressComponent.Instance.BaseInfoComponents.ArticleFacade.GetAllForZipFile(this.Homa.Id, article1, collection["txtSearch"], postFormData, Enums.AscendingDescending.Ascending, Enums.SortAccordingToArticle.DateOfSendingArticle).OrderBy(x => x.Code);
                using (var zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.Always;
                    zip.AlternateEncoding = Encoding.UTF8;
                    var fileFacade = FileManagerComponent.Instance.FileFacade;
                    foreach (var article in allForZipFile)
                    {
                        if (article.AbstractFileId.HasValue)
                        {
                            var file = fileFacade.Get(article.AbstractFileId);
                            if (file != null)
                            {
                                var ext = file.Extension.Replace(".", "").ToLower();
                                zip.AddEntry(article.Code + "-" + article.User.DescriptionField + "-" + article.Title + "-Abstract." + ext, file.Content);

                            }
                        }
                        else if (!string.IsNullOrEmpty(article.Abstract))
                            zip.AddEntry("AbstractText-" + article.Code + "-" + article.User.DescriptionField + "-" + article.Title + ".txt", article.Abstract, Encoding.UTF8);

                        if (article.OrginalFileId.HasValue)
                        {


                            var file = fileFacade.Get(article.OrginalFileId);
                            if (file != null && file.Content != null)
                            {
                                var ext = file.Extension.Replace(".", "").ToLower();
                                zip.AddEntry(article.Code + "-" + article.User.DescriptionField + "-" + article.Title + "-Orginal." + ext, file.Content);
                            }
                        }
                        else if (!string.IsNullOrEmpty(article.ArticleOrginalText))
                            zip.AddEntry(
                                "OrginalText-" + article.Code + "-" + article.User.DescriptionField + "-" + article.Title + ".txt", article.ArticleOrginalText, Encoding.UTF8);

                    }
                    Session["ArticleZipFile"] = zip;
                }

                return Content("true");





            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }

        }


        public ActionResult PrintArticleCertificate(Guid Id, bool Isadmin = false)
        {

            try
            {

                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptArticleCertificateId == null)
                {
                    ShowMessage(string.Format(Resources.Congress.ArticleCertificateNotDesgineInCongress, this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return this.Redirect(Isadmin ? "~/Congress/ManagmentPanel/UserArticles" : "~/Congress/UserPanel/IndexArticle");
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptArticleCertificateId);
                if (file == null) return Content("false");
                var article = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.GetArticleCertificate(this.Homa, Id, Isadmin);
                var stiReport1 = new StiReport();
                stiReport1.Load(file.Content);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/ArticlePanel/Article", new List<object>(article), "Article", new[] { "Id" });
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.Render(false);
                var stream = new MemoryStream();
                var settings = new StiPdfExportSettings { AutoPrintMode = StiPdfAutoPrintMode.None };
                var service = new StiPdfExportService();
                service.ExportPdf(stiReport1, stream, settings);
                return File(stream.ToArray(), "application/pdf", string.Format("{0}.Card.pdf", "ArticleCertificate"));
                return this.Redirect(Isadmin ? "~/Congress/ManagmentPanel/UserArticles" : "~/Congress/UserPanel/IndexArticle");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return this.Redirect(Isadmin ? "~/Congress/ManagmentPanel/UserArticles" : "~/Congress/UserPanel/IndexArticle");
            }

        }

     
        public ActionResult DesginArticlesReport()
        {
            try
            {
                var config = this.Homa.Configuration.HasArticlePayment;
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                var stiReport1 = new StiReport();
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataFromObj(AppExtention.CongressMoudelName + "-/Congress/ArticlePanel/Article", new Article(), null, false);

                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptArticleId == null)
                {
                    stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/" + (config ? "RptCongressArticleListWithpayment.mrt" : "RptCongressArticleList.mrt")));
                    stiReport1.RegBusinessObject("Model", model);
                    stiReport1.RegBusinessObject("Title", homa);
                    stiReport1.RegBusinessObject("SUM", 0);
                    stiReport1.RegBusinessObject("COUNT", 0);
                    stiReport1.Dictionary.SynchronizeBusinessObjects();
                    var file = new File
                    {
                        Content = stiReport1.SaveToByteArray(),
                        ContentType = stiReport1.GetType().Name,
                        Extension = "mrt",
                        FileName = "RptArticleCertificate"
                    };
                    congressDefinition.RptArticleId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                    if (!CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(congressDefinition))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                    messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var file1 = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptArticleId);
                    if (file1 == null)
                        return Json(new { Result = false, ReportId = Guid.Empty }, JsonRequestBehavior.AllowGet);
                    stiReport1.Load(file1.Content);
                    stiReport1.RegBusinessObject("Model", model);
                    stiReport1.RegBusinessObject("Title", homa);
                    stiReport1.RegBusinessObject("SUM", 0);
                    stiReport1.RegBusinessObject("COUNT", 0);
                    stiReport1.Dictionary.SynchronizeBusinessObjects();
                    file1.Content = stiReport1.SaveToByteArray();
                    if (!FileManagerComponent.Instance.FileFacade.Update(file1))
                    {
                        ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle,
                               messageIcon: MessageIcon.Succeed);
                        return Json(new { Result = false, ReportId = (Guid?)Guid.Empty }, JsonRequestBehavior.AllowGet);
                    }

                }
                return Json(new { Result = true, ReportId = (Guid)congressDefinition.RptArticleId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }

        }

        [HttpPost]
        public ActionResult PrintArticlesList(FormCollection collection)
        {
            try
            {

                var article = Areas.Congress.Tools.AppExtention.PrepareArticleSearch(collection);
                var articleAuthorsFacade = CongressComponent.Instance.BaseInfoComponents.ArticleAuthorsFacade;
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.Search(this.Homa.Id, article, collection["txtSearch"], postFormData, Enums.AscendingDescending.Ascending, Enums.SortAccordingToArticle.DateOfSendingArticle);
                var objects = new List<dynamic>();
                if (list.Any())
                {
                    objects = articleAuthorsFacade.Select(new Expression<Func<ArticleAuthors, object>>[] { x => x.ArticleId, x => x.Name },
                        authors => authors.ArticleId.In(list.Select(i => i.Id)));
                }
                foreach (var article1 in list)
                {

                    var enumerable = objects.Where(x => x.ArticleId == article1.Id && x.Name is string).Select(x => (string)x.Name);
                    if (enumerable.Any())
                        article1.AuthorsToString = string.Join(",", enumerable);
                    var referee = CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.Select(x => x.Referee.EnterpriseNode.RealEnterpriseNode.FirstName + " " + x.Referee.EnterpriseNode.RealEnterpriseNode.LastName, x => x.ArticleId == article1.Id, true);
                    article1.RefreeTitle = string.Join(",", referee);


                }
                var config = this.Homa.Configuration.HasArticlePayment;
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                var stiReport1 = new StiReport();
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/ArticlePanel/Article", new List<object>(list), "Article", new[] { "Id" });

                stiReport1.RegBusinessObject("Model", model);
                stiReport1.RegBusinessObject("Title", homa);
                stiReport1.RegBusinessObject("SUM", list.Where(x => x.TransactionId.HasValue).Sum(y => y.Transaction.Amount));
                stiReport1.RegBusinessObject("COUNT", list.Count());
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptArticleId == null)
                {
                    stiReport1.Load(Server.MapPath("~/Areas/Congress/Reports/" + (config ? "RptCongressArticleListWithpayment.mrt" : "RptCongressArticleList.mrt")));

                    var file = new File
                    {
                        Content = stiReport1.SaveToByteArray(),
                        ContentType = stiReport1.GetType().Name,
                        Extension = "mrt",
                        FileName = "RptArticleCertificate",

                    };

                    congressDefinition.RptArticleId = FileManagerComponent.Instance.FileFacade.InsertFile(file);
                    if (
                        !CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.Update(
                            congressDefinition))
                    {
                        ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Succeed);
                        return Content("false");
                    }
                }
                var fileid = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptArticleId);
                if (fileid != null)
                    stiReport1.Load(fileid.Content);

                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }

        }
        [HttpPost]
        public ActionResult PrintAllArticleCertificate(FormCollection collection)
        {

            try
            {
                var congressDefinition = CongressComponent.Instance.BaseInfoComponents.CongressDefinitionFacade.GetValidDefinition(this.Homa.Id);
                if (congressDefinition.RptArticleCertificateId == null)
                {
                    ShowMessage(string.Format(Resources.Congress.ArticleCertificateNotDesgineInCongress, this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                                  messageIcon: MessageIcon.Error);
                    return Content("false");
                }
                var file = FileManagerComponent.Instance.FileFacade.Get(congressDefinition.RptArticleCertificateId);
                if (file == null) return Content("false");

                var article=Areas.Congress.Tools.AppExtention.PrepareArticleSearch(collection);
               
                var postFormData = this.PostForFormGenerator(collection);
                var list = CongressComponent.Instance.BaseInfoComponents.ArticleFacade.GetAllArticleCertificate(this.Homa, article, collection["txtSearch"], postFormData, Enums.AscendingDescending.Ascending, Enums.SortAccordingToArticle.DateOfSendingArticle);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormData(AppExtention.CongressMoudelName + "-/Congress/ArticlePanel/Article", new List<object>(list), "Article", new[] { "Id" });
                var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(this.Homa.Id);
                var stiReport1 = new StiReport();
                stiReport1.Load(file.Content);
                stiReport1.RegBusinessObject("Model", model);
                stiReport1.RegBusinessObject("Title", homa);
                SessionParameters.Report = stiReport1;
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }

        #endregion



    }
}