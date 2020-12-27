using System.Web.Mvc;
using Radyn.Statistics;
using Radyn.WebApp.AppCode.Base;

namespace Radyn.WebApp.Areas.Statistics.Controllers
{
    public class LogPanelController : LocalizedController
    {
      
        public ActionResult GetStatitics()
        {
            try
            {
                var str = Request.Url.AbsolutePath;
                var authority =Request.Url.Authority;
                var result = StatisticComponents.Instance.LogFacade.GetStatisticsModel(authority + str);
                return PartialView("PartialViewStatitics", result);
            }
            catch (System.Exception ex)
            {

                return null;
            }
        }
    }
}
