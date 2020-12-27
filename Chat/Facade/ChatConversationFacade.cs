using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Radyn.Chat.BO;
using Radyn.Chat.DataStructure;
using Radyn.Chat.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Chat.Facade
{
    internal sealed class ChatConversationFacade : ChatBaseFacade<ChatConversation>, IChatConversationFacade
    {
        internal ChatConversationFacade() { }

        internal ChatConversationFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

        public IEnumerable<ChatConversation> GetReport()
        {
            try
            {
                var chatConversations = new ChatConversationBO().GetAll(this.ConnectionHandler);
                var result = (from row in chatConversations
                              group row by row.SessionId
                                  into g
                                  select g.FirstOrDefault()).ToList();
                return result.OrderByDescending(x => x.Time);
            }

            catch (KnownException knownException)
            {
                Log.Save(knownException.Message, LogType.ApplicationError, knownException.Source, knownException.StackTrace);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool DeleteForUser(Guid userId)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var chatConversationBO = new ChatConversationBO();
                var list = chatConversationBO.Where(ConnectionHandler, conversation =>
                conversation.SenderId == userId&&conversation.ReceiverId == userId
                );
                foreach (var chatConversation in list)
                {
                    if (!chatConversationBO.Delete(this.ConnectionHandler, chatConversation))
                        throw new Exception("خطایی در حذف مکالمه وجود دارد");
                }
                base.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(knownException.Message, LogType.ApplicationError, knownException.Source, knownException.StackTrace);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
