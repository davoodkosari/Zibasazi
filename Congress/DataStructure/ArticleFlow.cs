using System;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class ArticleFlow : DataStructureBase<ArticleFlow>
    {
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private Guid _articleId;
        [DbType("uniqueidentifier")]
        public Guid ArticleId
        {
            get
            {
                return this._articleId;
            }
            set
            {
                base.SetPropertyValue("ArticleId", value);
                if (Article == null)
                    this.Article = new Article { Id = value };
            }
        }

        [Assosiation(PropName = "ArticleId")]
        public Article Article { get; set; }

        private Guid _senderId;
        [DbType("uniqueidentifier")]
        public Guid SenderId
        {
            get
            {
                return this._senderId;
            }
            set
            {
                base.SetPropertyValue("SenderId", value);
                if (Sender == null)
                    this.Sender = new EnterpriseNode.DataStructure.EnterpriseNode { Id = value };
            }
        }
        [Assosiation(PropName = "SenderId")]
        public EnterpriseNode.DataStructure.EnterpriseNode Sender { get; set; }


        private byte? _status;
        [IsNullable]
        [DbType("tinyint")]
        public byte? Status
        {
            get { return _status; }
            set { base.SetPropertyValue("Status", value); }
        }


      

        private Guid? _receiverId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? ReceiverId
        {
            get
            {
                return this._receiverId;
            }
            set
            {
                base.SetPropertyValue("ReceiverId", value);
                if (Receiver == null)
                    this.Receiver = value.HasValue ? new EnterpriseNode.DataStructure.EnterpriseNode { Id = value.Value } : null;
            }
        }

        [Assosiation(PropName = "ReceiverId")]
        public EnterpriseNode.DataStructure.EnterpriseNode Receiver { get; set; }

        private string _saveDate;
        [DbType("char(10)")]
        public string SaveDate
        {
            get { return _saveDate; }
            set { base.SetPropertyValue("SaveDate", value); }
        }

        private string _saveTime;
        [DbType("char(5)")]
        public string SaveTime
        {
            get { return _saveTime; }
            set { base.SetPropertyValue("SaveTime", value); }
        }

        private string _remark;
        [IsNullable]
        [DbType("nvarchar(4000)")]
        public string Remark
        {
            get { return _remark; }
            set { base.SetPropertyValue("Remark", value); }
        }

        private Guid? _attachmentFileId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? AttachmentFileId
        {
            get
            {
                return this._attachmentFileId;
            }
            set
            {
                base.SetPropertyValue("AttachmentFileId", value);
                if (AttachmentFile == null)
                    this.AttachmentFile = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "فرم داوری")]
        [Assosiation(PropName = "AttachmentFileId")]
        public File AttachmentFile { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string LastRefreeView { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return ""; }
        }
    }
}
