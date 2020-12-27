using System.Configuration;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Chat
{
    public abstract class ChatBaseFacade<T> : BaseFacade<T> where T : class
    {

        protected ChatBaseFacade()
            : base(new ChatConnectionHandler(), false)
        {

        }
        protected ChatBaseFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {

        }
    }
    public class ChatConnectionHandler : ConnectionHandler
    {
        public ChatConnectionHandler()
        {
            base.ConnectionString = ConfigurationManager.ConnectionStrings["ChatEntities"].ConnectionString;
        }
    }


}
