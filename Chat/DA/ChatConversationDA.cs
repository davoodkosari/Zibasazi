using Radyn.Chat.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Chat.DA
{
    public sealed class ChatConversationDA : DALBase<ChatConversation>
    {
        public ChatConversationDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

    }
    internal class ChatConversationCommandBuilder
    {

    }
}
