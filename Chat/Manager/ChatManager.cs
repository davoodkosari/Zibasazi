using System;
using System.Collections.Generic;
using Radyn.Chat.DataStructure;
using Radyn.Chat.Facade;
using System.Linq;
using Radyn.Chat.Tools;
using Radyn.Security;
using Radyn.Security.DataStructure;

namespace Radyn.Chat.Manager
{
    public sealed class ChatManager
    {
        private readonly List<User> onlineUsers = new List<User>();

        private readonly List<ModelView.ChatModel> chatList = new List<ModelView.ChatModel>();

        private readonly List<ModelView.ChatModel> chatStatus = new List<ModelView.ChatModel>();

        public List<User> OnlineUsers()
        {
            return this.onlineUsers;
        }

        public List<User> OnlineUsers(User expectUser)
        {
            var second = new List<User> { expectUser };
            return this.onlineUsers.Except(second).ToList();
        }

        public void AddUser(User user)
        {
            if (user != null)
                if (!onlineUsers.Any(x => x != null && x.Username.Equals(user.Username)))
                    this.onlineUsers.Add(user);
        }

        public void RemoveUser(User user)
        {
            var find = this.onlineUsers.Find(x => x.Id.Equals(user.Id));
            if (find != null)
                this.onlineUsers.RemoveAt(this.onlineUsers.IndexOf(find));
        }

        public void AddMessage(string sender, string receiver, string message)
        {
            var chat = new ModelView.ChatModel
                           {
                               SenderUsername = sender,
                               ReceiverUsername = receiver,
                               Message = message,
                               Time = DateTime.Now
                           };

            this.chatList.Add(chat);
        }

        public void AddMessage(string sender, string receiver, string message, ref Guid? sessionId, bool save)
        {
            var chat = new ModelView.ChatModel
                           {
                               SenderUsername = sender,
                               ReceiverUsername = receiver,
                               Message = message,
                               Time = DateTime.Now
                           };

            this.chatList.Add(chat);
            if (sessionId == null)
                sessionId = Guid.NewGuid();

            if (!save) return;

            var facade = new ChatConversationFacade();
            facade.Insert(new ChatConversation
                              {
                                  SessionId = sessionId.Value,
                                  Message = message,
                                  ReceiverUsername = receiver,
                                  SenderUsername = sender
                              });
        }

        public void AddMessage(string sender, Guid senderId, string receiver, Guid receiverId, string message, Guid sessionId, bool save)
        {
            var chat = new ModelView.ChatModel
                           {
                               SenderUsername = sender,
                               ReceiverUsername = receiver,
                               Message = message,
                               Time = DateTime.Now,
                               SessionId = sessionId
                           };

            this.chatList.Add(chat);
            if (sessionId == null)
                sessionId = Guid.NewGuid();

            if (!save) return;

            var facade = new ChatConversationFacade();
            facade.Insert(new ChatConversation
                              {
                                  Id = Guid.NewGuid(),
                                  SessionId = sessionId,
                                  Message = message,
                                  ReceiverUsername = receiver,
                                  ReceiverId = receiverId,
                                  SenderUsername = sender,
                                  SenderId = senderId,
                                  Time = DateTime.Now
                              });
        }

        public IEnumerable<ModelView.ChatModel> ReceiveMessage(string sender, string receiver)
        {
            var list = this.chatList.Where(x => x.SenderUsername.Equals(receiver) && x.ReceiverUsername.Equals(sender)).ToList();
            foreach (var chatModel in list)
            {
                this.chatList.Remove(chatModel);
            }
            return list.Select(chatModel => chatModel);
        }

        public IEnumerable<ModelView.ChatModel> ReceiveOtherMessage(string sender, string receiver)
        {
            List<ModelView.ChatModel> list = !string.IsNullOrEmpty(receiver)
                                       ? this.chatList.Where(
                                           x => !x.SenderUsername.Equals(receiver) && x.ReceiverUsername.Equals(sender))
                                             .Distinct()
                                             .ToList()
                                       : this.chatList.Where(x => x.ReceiverUsername.Equals(sender)).Distinct().ToList();

            return list.Select(chatModel => chatModel).Distinct();
        }

        public IEnumerable<User> AllChatUsers(User user)
        {
            if (user == null) return null;
            return SecurityComponent.Instance.UserFacade.GetAll();
            //return SecurityComponent.Instance.UserFacade.Get(user.Id).Users;

        }

        public void SetStatus(string sender, string reciever, string status)
        {
            var model = this.chatStatus.Find(x => x.SenderUsername.Equals(sender) && !string.IsNullOrEmpty(x.ReceiverUsername) && x.ReceiverUsername.Equals(reciever));
            if (model == null)
            {
                model = new ModelView.ChatModel
                          {
                              SenderUsername = sender,
                              ReceiverUsername = reciever,
                              Message = status
                          };
                this.chatStatus.Add(model);
            }
            else
            {
                model.Message = status;
            }
        }

        public string GetStatus(string sender, string reciever)
        {
            var model = this.chatStatus.Find(x => x.SenderUsername.Equals(reciever) && !string.IsNullOrEmpty(x.ReceiverUsername) && x.ReceiverUsername.Equals(sender));
            return model != null ? model.Message : string.Empty;
        }
    }
}
