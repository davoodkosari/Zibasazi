using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Tools;

namespace Radyn.WebApp.Areas.Congress.Security.Filter
{
    [CongressHost]
    public class CongressBaseController : LocalizedController
    {
        public Homa Homa
        {
            get
            {
                if (Session == null) return null;
                AppExtention.GetHoma(this.HttpContext);
                return SessionParameters.CurrentCongress;

            }


        }

       
    }
    [CongressHost]
    public class CongressBaseController<T> : LocalizedController<T> where T : class
    {
        public Homa Homa
        {
            get
            {
                if (Session == null) return null;
                AppExtention.GetHoma(this.HttpContext);
                return SessionParameters.CurrentCongress;

            }


        }


    }
}