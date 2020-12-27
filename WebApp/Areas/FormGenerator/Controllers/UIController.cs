using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Radyn.FileManager;
using Radyn.FormGenerator;
using Radyn.FormGenerator.ControlFactory.Base;
using Radyn.FormGenerator.DataStructure;
using Radyn.FormGenerator.ModelView;
using Radyn.FormGenerator.Tools;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.FormGenerator.Tools;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Control = Radyn.FormGenerator.ControlFactory.Base.Control;


namespace Radyn.WebApp.Areas.FormGenerator.Controllers
{
    public class UIController : LocalizedController
    {
        public ActionResult LookUpFormData(string refId, string objectname)
        {
            ViewBag.objectname = objectname;
            ViewBag.refId = refId;
            return View(FormGeneratorComponent.Instance.FormDataFacade.GetFormData(refId,  objectname));
        }
        public ActionResult LookUpFormDataList(Guid formId)
        {
            var list = FormGeneratorComponent.Instance.FormDataFacade.Where(x=>x.StructureId==formId);
            ViewBag.FormId = formId;
            return View(list);
        }
        public void DownLoadAsExcel(System.Data.DataTable myList, FormStructure homa)
        {
            string fileName = homa.Name + "DataList.xls";
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

        public ActionResult ExportToExcel(Guid formId)
        {
            try
            {
                var formStructure = FormGeneratorComponent.Instance.FormStructureFacade.Get(formId);
                var model = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataForExcel(formId,SessionParameters.Culture);
                DownLoadAsExcel(model, formStructure);
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInEdit + exception.Message, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Content("false");
            }
        }
        public ActionResult PrintFormDataList(Guid formId)
        {
            try
            {
                var source = FormGeneratorComponent.Instance.FormDataFacade.ReportFormDataForExcel(formId,SessionParameters.Culture);

                StiReport report = new StiReport {ScriptLanguage = StiReportLanguageType.CSharp};
                report.Dictionary.Synchronize();
                report.Load(Server.MapPath("~/Areas/FormGenerator/Reports/RptFormDataList.mrt"));
                StiPage page = report.Pages.Items[0];
                var pageConponents = page.GetComponents();
                StiTable table = (StiTable)pageConponents[0];
                table.ColumnCount = source.Columns.Count;
                table.RowCount = source.Rows.Count + 1;
                table.HeaderRowsCount = 1;
                table.Width = page.Width;
                table.Height = page.GridSize * 12;
                table.CreateCell();
                int indexHeaderCell = 0;
                foreach (DataColumn column in source.Columns)
                {
                    var headerCell = table.Components[indexHeaderCell] as StiTableCell;
                    if (headerCell != null)
                    {
                        headerCell.Text.Value = column.Caption;
                        headerCell.HorAlignment = StiTextHorAlignment.Center;
                        headerCell.VertAlignment = StiVertAlignment.Center;
                        headerCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(32, 178, 170), 1,
                            StiPenStyle.Solid);
                    }
                    indexHeaderCell++;
                }
                foreach (DataRow row in source.Rows)
                {
                    foreach (DataColumn column in source.Columns)
                    {
                        var headerCell = table.Components[indexHeaderCell] as StiTableCell;
                        if (headerCell != null)
                        {
                            headerCell.Text.Value = row[column].ToString();
                            headerCell.HorAlignment = StiTextHorAlignment.Center;
                            headerCell.VertAlignment = StiVertAlignment.Center;
                            headerCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(32, 178, 170), 1,
                                StiPenStyle.Solid);
                        }

                        indexHeaderCell++;
                    }
                }
                SessionParameters.Report = report;
                return Content("true");

            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return Content("false");
            }
        }
        public ActionResult LookUpForm(Guid Id)
        {
            return View(FormGeneratorComponent.Instance.FormStructureFacade.Get(Id));
        }
        public ActionResult RenderForm(string url,string modelname,string modelid)
        {
            ViewBag.Url = url;
            ViewBag.ModelName = modelname;
            ViewBag.ModelId = modelid;
            return PartialView("PVRenderForm");
        }
        public ActionResult View(Guid Id)
        {
            return View(FormGeneratorComponent.Instance.FormStructureFacade.Get(Id));
        }
        [HttpPost]
        public ActionResult View(FormCollection collection)
        {
            try
            {
                var id = collection["FormId"];
                var postForFormGenerator = this.PostForFormGenerator(collection);
                postForFormGenerator.RefId =Guid.NewGuid().ToString();
                
                if (!string.IsNullOrEmpty(postForFormGenerator.FillErrors))
                {
                    ShowMessage(postForFormGenerator.FillErrors, Resources.Common.MessaageTitle,
                       messageIcon: MessageIcon.Warning);
                    return Content("false");
                }
                if (FormGeneratorComponent.Instance.FormDataFacade.ModifyFormData(postForFormGenerator))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                new[] { Resources.Common.Ok, " window.location='" + Radyn.Web.Mvc.UI.Application.CurrentApplicationPath + "/FormGenerator/UI/View?Id=" + id + "'; " }, messageIcon: MessageIcon.Succeed);
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
       
        public ActionResult UploadFile()
        {
            foreach (var file in this.Request.Files.AllKeys.Where(x => x.StartsWith("Uploader-")))
            {
                var Id = file.Substring(9, file.Length - 9);
                var postedFileBase = Request.Files[file];
                if (postedFileBase == null || postedFileBase.ContentLength == 0) continue;
                if (postedFileBase.InputStream != null)
                {
                    Session[Id] = postedFileBase;
                }
            }
            return Content("");
        }

    }
}
