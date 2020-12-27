﻿using System;
using Radyn.Security;
using Radyn.WebApp.AppCode.Security;

namespace Radyn.WebApp.AppCode.Filter
{
    public static class Autherized
    {
        public static bool IsAuthenticated()
        {
            

            switch (SessionParameters.UserType)
            {
                case UserType.None:
                    System.Web.Security.FormsAuthentication.SignOut();
                    return false;
                case UserType.Host:
                    return true;
                case UserType.User:
                    if (SessionParameters.User != null) return true;
                    System.Web.Security.FormsAuthentication.SignOut();
                    return false;
                case UserType.CongressUser:
                    if (SessionParameters.CongressUser != null) return true;
                    System.Web.Security.FormsAuthentication.SignOut();
                    return false;
                case UserType.CongressRefere:
                    if (SessionParameters.CongressReferee != null) return true;
                    System.Web.Security.FormsAuthentication.SignOut();
                    return false;


            }

            return false;
        }
        public static bool HasAccess(string url)
        {


            switch (SessionParameters.UserType)
            {
                case UserType.None:
                    System.Web.Security.FormsAuthentication.SignOut();
                    return false;
                case UserType.Host:
                    return true;
                case UserType.User:
                    if (SessionParameters.User == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        return false;
                    }
                    var hasAccess = SecurityComponent.Instance.UserFacade.AccessMenu(SessionParameters.User.Id, url);
                    return hasAccess != null;
                    break;
                case UserType.CongressUser:
                    if (SessionParameters.CongressUser == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        return false;
                    }

                    return true;
                    break;
                case UserType.CongressRefere:
                    if (SessionParameters.CongressReferee == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        return false;
                    }

                    return true;
                    break;

            }

            return false;
        }
    }
}