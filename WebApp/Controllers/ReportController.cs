using System;
using System.Data;
using System.Web.Mvc;

using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report.Web;

namespace Radyn.WebApp.Controllers
{
    public class ReportBuilderController : LocalizedController
    {

        public  ActionResult ReportView()
        {

            return View();

        }public  ActionResult ReportDesign()
        {

            return View();

        }
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
        public ActionResult PrintReport()
        {
           return StiMvcViewer.PrintReportResult(SessionParameters.Report);
        }
        public ActionResult ExportReport()
        {
           return StiMvcViewer.ExportReportResult(SessionParameters.Report);
        }

        public ActionResult GetReport()
        {
            return StiMvcViewer.GetReportResult(SessionParameters.Report);
        }
        public ActionResult GetDesignReport()
        {

            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["DataMethod"]))
                {
                    var info = new ReportProvider().GetType().GetMethod(Request.QueryString["DataMethod"]);
                    if (info != null)
                    {
                        var invoke = info.Invoke(info, string.IsNullOrEmpty(Request.QueryString["DataMethodParamets"]) ? null : new[] { Request.QueryString["DataMethodParamets"] });
                        if (invoke != null)
                        {
                            
                            var report = new StiReport();
                          report.Load((byte[])invoke);
                          return StiMvcDesigner.GetReportResult(report);

                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
               
               
                return null;

            }

            
        }
        public ActionResult SaveReport()
        {
            try
            {
                StiReport report = StiMvcDesigner.GetReportObject();
                if (!string.IsNullOrEmpty(Request.QueryString["UpdateMethod"]))
                {
                    var info = new ReportProvider().GetType().GetMethod(Request.QueryString["UpdateMethod"]);
                    if (info != null)
                    {


                        info.Invoke(info, string.IsNullOrEmpty(Request.QueryString["UpdateParametrs"]) ? null : new object[] { Request.QueryString["UpdateParametrs"], report.SaveToByteArray() });
                    }
                }
                return StiMvcDesigner.SaveReportResult();
            }
            catch (Exception ex)
            {
                
                return null;

            }
          
           
            
        }
        public ActionResult DesignerEvent()
        {
            return StiMvcDesigner.DesignerEventResult();
        }
        public ActionResult PreviewReport()
        {
            
            return StiMvcDesigner.GetReportResult(SessionParameters.Report);
        }
    }
}
