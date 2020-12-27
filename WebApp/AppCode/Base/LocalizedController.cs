using System.Web.Mvc;
using Radyn.Congress.DataStructure;
using Radyn.WebApp.AppCode.Filter;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Tools;

namespace Radyn.WebApp.AppCode.Base
{
    [Localization]
    public class LocalizedController : BaseController
    {
       
    }
    [Localization]
    public class LocalizedController<T> : BaseController<T> where T : class
    {
      
    }
}