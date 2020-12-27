using System;

namespace Radyn.Chat.Tools
{
   public class ModelView
    {
       public sealed class ChatModel
       {
           #region Primitive Properties

           public Guid Id { get; set; }

           public Guid? SenderId { get; set; }

           public string SenderUsername { get; set; }

           public Guid? ReceiverId { get; set; }

           public string ReceiverUsername { get; set; }

           public string Message { get; set; }

           public DateTime Time { get; set; }

           public Guid SessionId { get; set; }

           #endregion
       }
    }
}
