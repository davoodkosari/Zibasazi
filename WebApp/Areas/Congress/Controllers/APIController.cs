using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Serialization;
using Radyn.Congress;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.XmlModel;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class APIController : CongressBaseController
    {
        public ActionResult GetAttendanceXmlFile()
        {
            try
            {
                var objectToSerialize = CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetXmlData();
                return Json(objectToSerialize.JsonSerializer(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public ActionResult DownloadAttendanceXmlFile()
        {

            try
            {
                var objectToSerialize = CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetXmlData();
                var ser = new XmlSerializer(objectToSerialize.GetType());
                var stream = new MemoryStream();
                ser.Serialize(stream, objectToSerialize);
                stream.Position = 0;
                return File(stream, "application/xml", "AttendanceXmlFile.xml");
                
            }
            catch (Exception ex)
            {

                return null;
            }
          
        }
       [HttpPost]
        public ActionResult SetAttendanceXmlFile()
        {
            try
            {
                string Json;
                StreamReader rdr = new StreamReader(this.Request.InputStream, Encoding.UTF8);
                Json = rdr.ReadToEnd();
               return Content(CongressComponent.Instance.BaseInfoComponents.HomaFacade.SetXmlData(Json) ? "true" : "false");
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}