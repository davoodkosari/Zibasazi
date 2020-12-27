using System;
using System.Collections.Generic;
using Radyn.Chat.DataStructure;
using Radyn.Framework;

namespace Radyn.Chat.Facade.Interface
{
public interface IChatConversationFacade : IBaseFacade<ChatConversation>
{
    IEnumerable<ChatConversation> GetReport();
    bool DeleteForUser(Guid userId);
}
}
