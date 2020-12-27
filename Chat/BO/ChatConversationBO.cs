using System.Collections.Generic;
using Radyn.Chat.DA;
using Radyn.Chat.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Chat.BO
{
internal class ChatConversationBO : BusinessBase<ChatConversation>
{
   
    public override bool Insert(IConnectionHandler connectionHandler, ChatConversation obj)
    {
        var id = obj.Id;
        BOUtility.GetGuidForId(ref  id);
        obj.Id = id;
        return base.Insert(connectionHandler, obj);
    }
}
}
