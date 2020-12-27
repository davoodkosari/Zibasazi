using System;
using Radyn.Framework;

namespace Radyn.Chat.DataStructure
{
    [Serializable]
    [Schema("Chat")]
    public sealed class ChatConversation : DataStructureBase<ChatConversation>
    {
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private Guid _sessionId;
        [DbType("uniqueidentifier")]
        public Guid SessionId
        {
            get { return _sessionId; }
            set { base.SetPropertyValue("SessionId", value); }
        }

        private Guid _senderId;
        [DbType("uniqueidentifier")]
        public Guid SenderId
        {
            get { return _senderId; }
            set { base.SetPropertyValue("SenderId", value); }
        }

        private string _senderUsername;
        [DbType("varchar(50)")]
        public string SenderUsername
        {
            get { return _senderUsername; }
            set { base.SetPropertyValue("SenderUsername", value); }
        }

        private Guid? _receiverId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? ReceiverId
        {
            get { return _receiverId; }
            set { base.SetPropertyValue("ReceiverId", value); }
        }

        private string _receiverUsername;
        [DbType("varchar(50)")]
        public string ReceiverUsername
        {
            get { return _receiverUsername; }
            set { base.SetPropertyValue("ReceiverUsername", value); }
        }

        private string _message;
        [DbType("nvarchar(500)")]
        public string Message
        {
            get { return _message; }
            set { base.SetPropertyValue("Message", value); }
        }

        private DateTime _time;
        [DbType("datetime")]
        public DateTime Time
        {
            get { return _time; }
            set { base.SetPropertyValue("Time", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Message; }
        }
    }
}
