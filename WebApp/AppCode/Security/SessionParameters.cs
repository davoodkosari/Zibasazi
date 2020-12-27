using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using Radyn.Common;
using Radyn.Security.DataStructure;
using Stimulsoft.Report;

namespace Radyn.WebApp.AppCode.Security
{
    public partial class SessionParameters
    {
        public static User User
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["user"] != null)
                    return (User)HttpContext.Current.Session["user"];
                return null;
            }
            set
            {

                HttpContext.Current.Session["user"] = value;
            }
        }
        public static KeyValuePair<string, string>? Error
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["Error"] != null)
                    return (KeyValuePair<string, string>)HttpContext.Current.Session["Error"];
                return null;
            }
            set
            {

                HttpContext.Current.Session["Error"] = value;
            }
        }
        public static StiReport Report
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["Report"] != null)
                    return (StiReport)HttpContext.Current.Session["Report"];
                return null;
            }
            set
            {

                HttpContext.Current.Session["Report"] = value;
            }
        }

        public static bool HasLoginPasswordError
        {
            get
            {
                if (HttpContext.Current.Session["HasLoginPasswordError"] != null)
                    return (bool)HttpContext.Current.Session["HasLoginPasswordError"];
                return false;
            }
            set
            {
                HttpContext.Current.Session["HasLoginPasswordError"] = value;

            }
        }
        public static List<Operation> UserOperation
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["Operations"] != null)
                    return (List<Operation>)HttpContext.Current.Session["Operations"];
                return null;
            }
            set
            {

                HttpContext.Current.Session["Operations"] = value;
            }
        }

        public static UserType UserType
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["UserType"] != null)
                    return (UserType)HttpContext.Current.Session["UserType"];
                return UserType.None;
            }
            set
            {

                HttpContext.Current.Session["UserType"] = value;
            }
        }

        public static string Culture
        {
            get
            {

                return HttpContext.Current.Session["culture"] != null ? HttpContext.Current.Session["culture"].ToString() : null;
            }
            set
            {

                HttpContext.Current.Session["culture"] = value;
                if (Thread.CurrentThread.CurrentUICulture.Name != value)
                    Thread.CurrentThread.CurrentUICulture =
                        CultureInfo.CreateSpecificCulture(SessionParameters.Culture);


            }
        }




    }

    public enum UserType
    {
        None,
        Host,
        Admin,
        User,
        CongressUser,
        CongressRefere,
    }
}