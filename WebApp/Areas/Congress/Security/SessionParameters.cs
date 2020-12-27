using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;

namespace Radyn.WebApp.AppCode.Security
{
    public partial class SessionParameters
    {
        public static User CongressUser
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["CongressUser"] != null)
                    return (User)HttpContext.Current.Session["CongressUser"];
                return null;
            }
            set
            {

                HttpContext.Current.Session["CongressUser"] = value;
            }
        }
         
        public static Referee CongressReferee
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["CongressReferee"] != null)
                    return (Referee)HttpContext.Current.Session["CongressReferee"];
                return null;
            }
            set
            {

                HttpContext.Current.Session["CongressReferee"] = value;
            }
        }
        public static Homa CurrentCongress
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["currentCongress"] != null)
                    return (Homa)HttpContext.Current.Session["currentCongress"];
                return null;
            }
            set
            {

                HttpContext.Current.Session["currentCongress"] = value;
            }
        }
        public static bool CongressSessionStarted
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["CongressSessionStarted"] != null)
                    return (bool)HttpContext.Current.Session["CongressSessionStarted"];
                return false;
            }
            set
            {

                HttpContext.Current.Session["CongressSessionStarted"] = value;
            }
        }
        

    }
}