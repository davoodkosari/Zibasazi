using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using Radyn.Common.Component;
using Radyn.Common.DataStructure;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.WebApp.AppCode.Security;
using Resources;
using CommonComponent = Radyn.Common.CommonComponent;

namespace Radyn.WebApp.Areas.Common.Tools
{
    public static class AppExtentions
    {
        public static string GetCurrentUserImageUrl()
        {
            if (SessionParameters.User == null)
                return "/Content/Images/man.png";

            if (SessionParameters.User.EnterpriseNode == null ||
                SessionParameters.User.EnterpriseNode.RealEnterpriseNode == null)
                return "/Content/Images/man.png";

            if (SessionParameters.User.EnterpriseNode.PictureId.HasValue)
                return FileManagerContants.FileHandlerRoot + SessionParameters.User.EnterpriseNode.PictureId;
            if (SessionParameters.User.EnterpriseNode.RealEnterpriseNode.Gender == null)
                return "/Content/Images/man.png";
            return SessionParameters.User.EnterpriseNode.RealEnterpriseNode.Gender == true
                ? "/Content/Images/man.png"
                : "/Content/Images/woman.png";
        }
        public static List<string> GetAllAreaNames()
        {
            return RouteTable.Routes.OfType<Route>()
                 .Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area"))
                 .Select(r => (string)r.DataTokens["area"]).ToList();

        }
       
      
       

       
    }
}