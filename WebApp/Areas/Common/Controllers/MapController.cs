using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Common.Definition;
using Radyn.Utility;

namespace Radyn.WebApp.Areas.Common.Controllers
{
    public class MapController : Controller
    {
        // GET: Common/Map
        public ActionResult FullSearch(string points, string hidenId, string defaultpoint, int? maxdistant, string callback, PageMode pageMode)
        {
            
            if (string.IsNullOrEmpty(points))
            {
                if (!string.IsNullOrEmpty(defaultpoint))
                    points = defaultpoint;
                else points = "35.69944714864398,51.33785222951042,15";
            }
            var val = points.Split(',');
            ViewBag.Lat = val[0];
            ViewBag.Lon = val[1];
            ViewBag.Zoom = val[2];
            ViewBag.hidenId = hidenId;
            ViewBag.callback = callback;
            ViewBag.SelectMode = (pageMode == PageMode.Edit || pageMode == PageMode.Create);
            ViewBag.defaultpoint = defaultpoint;
            ViewBag.maxdistant = maxdistant;
            return View("PVFullSearch");
        }
        public ActionResult GetLocationSearchControl(string points, string defaultpoint, int? maxdistant, string callback, string deletecallback, string hidenId, PageMode pageMode)
        {


            ViewBag.points = points;
            ViewBag.hidenId = hidenId;
            ViewBag.deletecallback = deletecallback;
            ViewBag.callback = callback;
            ViewBag.pageMode = pageMode;
            ViewBag.defaultpoint = defaultpoint;
            ViewBag.maxdistant = maxdistant;
            return PartialView("PVLocationSearchControl");
        }
    }
}