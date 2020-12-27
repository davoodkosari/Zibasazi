using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Radyn.Congress;
using Radyn.ContentManager.DataStructure;
using Radyn.FormGenerator;
using Radyn.FormGenerator.ControlFactory.Base;
using Radyn.FormGenerator.DataStructure;
using Radyn.FormGenerator.ModelView;
using Radyn.FormGenerator.Tools;
using Radyn.Framework;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.Congress.Tools;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressFormsController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {

            var list = CongressComponent.Instance.BaseInfoComponents.CongressFormsFacade.Select(x => x.FormStructure,
                x => x.CongressId == this.Homa.Id);
            foreach (var item in list)
            {
                var userForm =
                    CongressComponent.Instance.BaseInfoComponents.UserFormsFacade.FirstOrDefault(
                        c => c.FormId == item.Id);
                if (userForm != null)
                    item.IsUserForms = true;
            }

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
            TempData["Containers"] =
                new SelectList(
                    CongressComponent.Instance.BaseInfoComponents.CongressContainerFacade.SelectKeyValuePair(
                        x => x.ContainerId, x => x.Container.Title, x => x.CongressId == this.Homa.Id), "Key", "Value");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var formStructure = new FormStructure();
            try
            {
                this.RadynTryUpdateModel(formStructure, collection);

                if (CongressComponent.Instance.BaseInfoComponents.CongressFormsFacade.Insert(this.Homa.Id, formStructure))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressForms/Edit?Id=" + formStructure.Id);

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressForms/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                TempData["Containers"] =
                    new SelectList(
                        CongressComponent.Instance.BaseInfoComponents.CongressContainerFacade.SelectKeyValuePair(
                            x => x.ContainerId, x => x.Container.Title, x => x.CongressId == this.Homa.Id), "Key",
                        "Value");
                return View(formStructure);
            }
        }


        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            ViewBag.IsForUser = CongressComponent.Instance.BaseInfoComponents.UserFormsFacade.Get(this.Homa.Id, Id) !=
                                null;
            var firstOrDefault =
                FormGeneratorComponent.Instance.FormAssigmentFacade.FirstOrDefault(x => x.FormStructureId == Id);
            ViewBag.Datas = new SelectList(AppExtention.GetFormList(), "Key", "Value",
                firstOrDefault != null ? firstOrDefault.Url : "");
            TempData["Containers"] =
                new SelectList(
                    CongressComponent.Instance.BaseInfoComponents.CongressContainerFacade.SelectKeyValuePair(
                        x => x.ContainerId, x => x.Container.Title, x => x.CongressId == this.Homa.Id), "Key", "Value");
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var formStructure = FormGeneratorComponent.Instance.FormStructureFacade.Get(Id);
            try
            {
                var forUser = collection["ForUser"].ToBool();
                var url = collection["Selected"];
                this.RadynTryUpdateModel(formStructure, collection);
                if (forUser)
                    url = null;
                if (CongressComponent.Instance.BaseInfoComponents.CongressFormsFacade.UpdateAndAssgine(this.Homa.Id,
                    formStructure, url, forUser))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressForms/Index");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressForms/Index");


            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                var firstOrDefault =
                    FormGeneratorComponent.Instance.FormAssigmentFacade.FirstOrDefault(x => x.FormStructureId == Id);
                ViewBag.Datas = new SelectList(AppExtention.GetFormList(), "Key", "Value",
                    firstOrDefault != null ? firstOrDefault.Url : "");
                TempData["Containers"] =
                    new SelectList(
                        CongressComponent.Instance.BaseInfoComponents.CongressContainerFacade.SelectKeyValuePair(
                            x => x.Container.Id, x => x.Container.Title, x => x.CongressId == this.Homa.Id), "Key",
                        "Value");
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
                if (CongressComponent.Instance.BaseInfoComponents.CongressFormsFacade.Delete(this.Homa.Id, Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                        messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressForms/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                    messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressForms/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }

        public ActionResult GetUserFormReport(Guid formId)
        {
            try
            {
                var source = CongressComponent.Instance.BaseInfoComponents.UserFormsFacade.ReportUserForms(formId, this.Homa.Id,SessionParameters.Culture);
                
                StiReport report = new StiReport { ScriptLanguage = StiReportLanguageType.CSharp };
                report.Dictionary.Synchronize();
                report.Load(Server.MapPath("~/Areas/Congress/Reports/RptUserForms.mrt"));
                StiPage page = report.Pages.Items[0];
                var p = page.GetComponents();
                StiTable table = (StiTable)p[0];
                table.ColumnCount = source.Columns.Count;
                table.RowCount = source.Rows.Count + 1;
                table.HeaderRowsCount = 1;
                table.Width = page.Width;
                table.Height = page.GridSize * 12;
                table.Name = "Table1";
                table.CreateCell();
                var indexHeaderCell = 0;
                foreach (DataColumn column in source.Columns)
                {
                    var headerCell = table.Components[indexHeaderCell] as StiTableCell;
                    if (headerCell != null)
                    {
                        headerCell.Text.Value = column.Caption;
                        headerCell.HorAlignment = StiTextHorAlignment.Center;
                        headerCell.VertAlignment = StiVertAlignment.Center;
                        headerCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(32, 178, 170), 1, StiPenStyle.Solid);
                    }

                    indexHeaderCell++;
                }
                

                foreach (DataRow row in source.Rows)
                {

                    foreach (DataColumn column in source.Columns)
                    {
                        StiTableCell headerCell = table.Components[indexHeaderCell] as StiTableCell;
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
                this.Redirect("/ReportBuilder/ReportView");
                return Content("true");

            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return Redirect("Index");
            }
        }
    }
}