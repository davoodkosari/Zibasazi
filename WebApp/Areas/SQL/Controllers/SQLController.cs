using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Radyn.SQL.Enums;
using Radyn.WebApp.AppCode.Security;

namespace Radyn.WebApp.Areas.SQL.Controllers
{
    public class SQLController : Controller
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            ViewBag.Connections = new SelectList(ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>(),
                                                   "ConnectionString", "Name");
            ViewBag.OutType = SqlOutType.None;
            return View();
        }

        [HttpPost]
        public ActionResult GetResult(FormCollection collection)
        {
            var connection = "";
            var command = "";
            var entityConnection = collection["connection"];
            try
            {
                if (!string.IsNullOrEmpty(entityConnection))
                {
                    connection = Utility.Utils.GetSimpleConnectionString(entityConnection);
                    if (String.IsNullOrEmpty(connection))
                        connection = entityConnection;
                    command = collection["commandValue"];
                }

                var outType = SqlOutType.None;

                var output = Radyn.SQL.SQLComponent.Instance.SQLFacade.Execute(connection, command, ref outType);
                ViewBag.OutType = outType;
                ViewBag.Connections = new SelectList(ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>(),
                                           "ConnectionString", "Name", entityConnection);
                ViewBag.command = command.Trim();
                return this.PartialView("PVResult", output);
            }
            catch (Exception exception)
            {
                ViewBag.OutType = SqlOutType.None;
                ViewBag.Connections = new SelectList(ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>(),
                                           "ConnectionString", "Name", entityConnection);

                ViewBag.Message = exception.Message;
                ViewBag.command = command;
                if (exception.InnerException != null)
                    ViewBag.Message = exception.InnerException.Message;
                return this.PartialView("PVResult", "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: SQL Network Interfaces, error: 26 - Error Locating Server/Instance Specified)");
            }
        }

        [HttpPost]
        public ActionResult GetWizardResult(FormCollection collection)
        {
            var connection = "";
            var command = "";
            var entityConnection = collection["connection"];
            try
            {
                if (!string.IsNullOrEmpty(entityConnection))
                {
                    connection = Utility.Utils.GetSimpleConnectionString(entityConnection);
                    if (String.IsNullOrEmpty(connection))
                        connection = entityConnection;
                }
                command = string.Format("SELECT {0} FROM [{1}].[{2}]", collection["col"], collection["schema"],
                    collection["table"]);
                var outType = SqlOutType.None;

                var output = Radyn.SQL.SQLComponent.Instance.SQLFacade.Execute(connection, command, ref outType);
                ViewBag.OutType = outType;
                ViewBag.Connections = new SelectList(ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>(),
                                           "ConnectionString", "Name", entityConnection);
                ViewBag.command = command.Trim();
                return this.PartialView("PVResult", output);
            }
            catch (Exception exception)
            {
                ViewBag.OutType = SqlOutType.None;
                ViewBag.Connections = new SelectList(ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>(),
                                           "ConnectionString", "Name", entityConnection);

                ViewBag.Message = exception.Message;
                ViewBag.command = command;
                if (exception.InnerException != null)
                    ViewBag.Message = exception.InnerException.Message;
                return this.PartialView("PVResult", "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: SQL Network Interfaces, error: 26 - Error Locating Server/Instance Specified)");
            }
        }

        public JsonResult GetSchema(string connection)
        {
            var outType = SqlOutType.None;
            var output = Radyn.SQL.SQLComponent.Instance.SQLFacade.Execute(connection, "select * from INFORMATION_SCHEMA.SCHEMATA where SCHEMA_OWNER='dbo'", ref outType);
            var listout = new List<Object>();
            foreach (var s in ((DataTable)output).Rows.Cast<DataRow>())
            {
                listout.Add(new { id = s["Schema_Name"] });
            }
            return Json(listout, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetTables(string connection, string schema)
        {
            var outType = SqlOutType.None;
            var output = Radyn.SQL.SQLComponent.Instance.SQLFacade.Execute(connection, "select * from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA='" + schema + "'", ref outType);
            var listout = new List<Object>();
            foreach (var s in ((DataTable)output).Rows.Cast<DataRow>())
            {
                listout.Add(new { id = s["Table_Name"] });
            }
            return Json(listout, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetColumns(string connection, string schema,string table)
        {
            var outType = SqlOutType.None;
            var output = Radyn.SQL.SQLComponent.Instance.SQLFacade.Execute(connection, "select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='"+table+"' and TABLE_SCHEMA ='"+schema+"'", ref outType);
            var listout = new List<Object>();
            foreach (var s in ((DataTable)output).Rows.Cast<DataRow>())
            {
                listout.Add(new { id = s["Column_Name"] });
            }
            return Json(listout, JsonRequestBehavior.AllowGet);
        }
    }
}
