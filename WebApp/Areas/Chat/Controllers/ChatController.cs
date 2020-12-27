using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Chat;
using Radyn.Security;
using Radyn.Security.DataStructure;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Filter;
using Radyn.WebApp.AppCode.Security;
using Radyn.Utility;

namespace Radyn.WebApp.Areas.Chat.Controllers
{
    public class ChatController : LocalizedController
    {
        [RadynAuthorize]
        public ActionResult Index(string user, string sessionId)
        {
            ChatComponent.Instance.ChatManager.AddUser(SessionParameters.User);
            this.ViewBag.Users = ChatComponent.Instance.ChatManager.AllChatUsers(SessionParameters.User);
            this.ViewBag.ChatUser = user;
            User firstOrDefault = null;
            if (!string.IsNullOrEmpty(user))
            {
                firstOrDefault =
                    SecurityComponent.Instance.UserFacade.FirstOrDefault(user1 => user1.Username == user);
            }
            this.ViewBag.ReciverId = firstOrDefault!=null?firstOrDefault.Id:(Guid?) null;
            return View(string.IsNullOrEmpty(sessionId) ? Guid.NewGuid() : Guid.Parse(sessionId));
        }

        [RadynAuthorize]
        public ActionResult UserOnline(string user)
        {
            if (SessionParameters.User != null)
            {
                this.ViewBag.Users = ChatComponent.Instance.ChatManager.AllChatUsers(SessionParameters.User);
                this.ViewBag.OnlineUsers = ChatComponent.Instance.ChatManager.OnlineUsers(SessionParameters.User);
                this.ViewBag.ChatUser = user;
                return PartialView("OnlineUsers");
            }
            return null;
        }

        [RadynAuthorize(DoAuthorize = false)]
        public JsonResult AddMessage(FormCollection collection)
        {
            ChatComponent.Instance.ChatManager.AddMessage(SessionParameters.User.Username, SessionParameters.User.Id,
                                                          collection["receiver"], Guid.Parse(collection["reciverId"]),
                                                          collection["message"].Replace("\n", "<br/>"), Guid.Parse(collection["sessionId"]),
                                                          save: false);
            var result = new { message = "Success" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetStatus(string receiver, string status)
        {
            if (SessionParameters.User != null)
                ChatComponent.Instance.ChatManager.SetStatus(SessionParameters.User.Username, receiver, status);
            var result = new { message = "Success" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatus(string receiver)
        {
            var msg = "";
            if (SessionParameters.User != null)
                msg = ChatComponent.Instance.ChatManager.GetStatus(SessionParameters.User.Username, receiver);
            var result = new { message = msg };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NoCache]
        [RadynAuthorize(DoAuthorize = false)]
        [HandleError]
        public JsonResult GetMessages(string receiver)
        {
            var result = new LinkedList<object>();
            if (!string.IsNullOrEmpty(receiver) && SessionParameters.User != null)
            {
                var receiveMessage = ChatComponent.Instance.ChatManager.ReceiveMessage(SessionParameters.User.Username, receiver);
                foreach (var msg in receiveMessage)
                {
                    result.AddLast(new { Username = receiver, MessageBody = msg.Message, Time = msg.Time.GetTime() + ":" + msg.Time.Second });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NoCache]
        [RadynAuthorize(DoAuthorize = false)]
        [HandleError]
        public JsonResult GetOtherPeople(string receiver)
        {
            var result = new LinkedList<object>();
            if (SessionParameters.User != null)
            {
                var otherMessage = ChatComponent.Instance.ChatManager.ReceiveOtherMessage(SessionParameters.User.Username, receiver);

                foreach (var username in otherMessage)
                {
                    result.AddLast(new { Username = username.SenderUsername, SessionId = username.SessionId.ToString() });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [RadynAuthorize]
        public ActionResult ExitChat()
        {
            ChatComponent.Instance.ChatManager.RemoveUser(SessionParameters.User);
            return View();
        }
        [RadynAuthorize]
        public ActionResult SignOut()
        {
            ChatComponent.Instance.ChatManager.RemoveUser(SessionParameters.User);
            Session.Abandon();
            return RedirectToAction("Login", "User", new { area = "Security" });
        }

        [RadynAuthorize]
        public ActionResult ChatReport()
        {
            var coversations = ChatComponent.Instance.ChatConversationFacade.GetReport();
            return View(coversations);
        }

        [RadynAuthorize]
        public ActionResult ConversationDetail(Guid sId)
        {
            return View(ChatComponent.Instance.ChatConversationFacade.GetAll());//.OrderBy(x => x.Time).Where(x => x.SessionId.Equals(sId)));
        }
    }
}
