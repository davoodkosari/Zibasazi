using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Radyn.WebApp.AppCode.Security
{
    public static class Settings
    {
        public static bool ShowMessageInnerException
        {
            get
            {
                if (ConfigurationManager.AppSettings["ShowInnerExcption"] != null)
                    return bool.Parse(ConfigurationManager.AppSettings["ShowInnerExcption"]);
                return false;
            }
        }

    }
}