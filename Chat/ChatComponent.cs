using Radyn.Chat.Facade;
using Radyn.Chat.Facade.Interface;
using Radyn.Chat.Manager;

namespace Radyn.Chat
{
    public class ChatComponent
    {
        private ChatComponent()
        {

        }

        private static ChatComponent _instance;
        public static ChatComponent Instance
        {
            get
            {
                return _instance ?? (_instance = new ChatComponent());
            }
        }

        private readonly ChatManager chatManager = new ChatManager();
        public ChatManager ChatManager
        {
            get
            {
                return this.chatManager;
            }
        }
        public IChatConversationFacade ChatConversationFacade
        {
            get
            {
                return new ChatConversationFacade();
            }
        }

    }
}
