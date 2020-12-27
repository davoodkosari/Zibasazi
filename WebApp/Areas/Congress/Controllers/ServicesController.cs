using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Message;
using Radyn.Utility;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class ServicesController : Controller
    {


        //  [HttpPost]
        public ContentResult Login(Guid congressId, string username, string password)
        {
            try
            {
                var login = CongressComponent.Instance.BaseInfoComponents.UserFacade.Login(username, password, congressId);
                return Content(login == null ? "کاربری یافت نشد" : "");
            }
            catch (Exception ex)
            {

                return Content(ex.Message);
            }
        }

        [System.Web.Http.HttpPost]
        public ContentResult LogOut(string userId)
        {
            try
            {
                var user = CongressComponent.Instance.BaseInfoComponents.UserFacade.Get(userId);
                user.FbTokenId = null;
                CongressComponent.Instance.BaseInfoComponents.UserFacade.Update(user);
                return Content("");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [System.Web.Http.HttpPost]
        public JsonResult LoginUser(string tokenId, string congresscode, string username, string password)
        {
            try
            {
                var obj = CongressComponent.Instance.BaseInfoComponents.ConfigurationFacade.FirstOrDefault(c => c.CongressCode == congresscode);
                if (obj == null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                var congressId = obj.CongressId;
                var login = CongressComponent.Instance.BaseInfoComponents.UserFacade.Login(username, password, congressId);
                if (login == null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                var pass = login.Password;
                var objuser = CongressComponent.Instance.BaseInfoComponents.UserFacade.FirstOrDefault(c => c.Username == username & c.Password == pass);
                objuser.FbTokenId = tokenId;
                CongressComponent.Instance.BaseInfoComponents.UserFacade.Update(objuser);
                //return Content(new
                //{
                //    Id = objuser.Id.ToString(),


                //})

                var objLogined = new Logined()
                {
                    UserId = objuser.Id.ToString(),
                    FirstName = objuser.EnterpriseNode.RealEnterpriseNode.FirstName,
                    LastName = objuser.EnterpriseNode.RealEnterpriseNode.LastName
                };
                return Json(objLogined, JsonRequestBehavior.AllowGet);






                //return Content(objuser.Id.ToString());
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public class Inbox
        {
            public string Subject { get; set; }
            public string Id { get; set; }
            public string Body { get; set; }
            public string ReciveDateTime { get; set; }
            public string Item { get; set; }
            public string Priority { get; set; }
            public string RequestType { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public int ChatCount { get; set; }
        }

        public class Logined
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }




        [System.Web.Http.HttpPost]
        public JsonResult GetUserMessages(string userId)
        {
            var objects = new List<Inbox>();
            try
            {
//                Log.Save("reza", LogType.ApplicationError, userId.ToString(), userId.ToString());
                var list = MessageComponenet.SentInternalMessageInstance.MailBoxFacade.Where(c => c.ToId == userId.ToGuid()).OrderByDescending(c=>c.MailInfo.Date);
                foreach (var mailBox in list)
                {
                    objects.Add(
                        new Inbox() { Id = mailBox.Id.ToString(), Subject = mailBox.MailInfo.Subject, Body = Utils.ConvertHtmlToString(mailBox.MailInfo.Body), UserId = userId.ToString(), ReciveDateTime = mailBox.MailInfo.Date.ShamsiDate() });
                }
                return Json(objects, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(objects, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
