using System;
using Radyn.Congress.Tools;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;
using Radyn.Payment.DataStructure;
using Radyn.Utility;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Description("مقاله/ایده/اثر")]
    [Schema("Congress")]
    [Track]
    public sealed class Article : DataStructureBase<Article>
    {
     
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        [TrackMaster(typeof(Article))]
        [Layout(Caption = "شناسه")]
        public Guid Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private Int64 _code;
        [DbType("bigint")]
        [Identity(true)]
        [Layout(Caption = "کد")]
        public Int64 Code
        {
            get { return _code; }
            set { base.SetPropertyValue("Code", value); }
        }

        private Guid _congressId;
        [DbType("uniqueidentifier")]
        [Layout(Caption = "شناسه همایش")]
        public Guid CongressId
        {
            get
            {
                return this._congressId;
            }
            set
            {
                base.SetPropertyValue("CongressId", value);
                if (Homa == null)
                    this.Homa = new Homa { Id = value };
            }
        }

        [Assosiation(PropName = "CongressId", FillData = false)]
        public Homa Homa { get; set; }

        private string _title;
        [DbType("nvarchar(300)")]
        [Layout(Caption = "عنوان")]
        [MultiLanguage(FillAnyLanguage = true)]
        public string Title
        {
            get { return _title; }
            set { base.SetPropertyValue("Title", value); }
        }

        [Layout(Caption = "تاریخ آخرین ویرایش")]
        [IsNullable]
        [DbType("char(10)")]
        public string EditDate { get; set; }

        [Layout(Caption = "زمان آخرین ویرایش")]
        [IsNullable]
        [DbType("char(10)")]
        public string EditTime { get; set; }

        private string _abstract;
        [IsNullable]
        [DbType("ntext")]
        [Layout(Caption = "متن چکیده")]
        public string Abstract
        {
            get { return _abstract; }
            set { base.SetPropertyValue("Abstract", value); }
        }


        private string _articleOrginalText;
        [IsNullable]
        [DbType("ntext")]
        [Layout(Caption = "متن ")]
        public string ArticleOrginalText
        {
            get { return _articleOrginalText; }
            set { base.SetPropertyValue("ArticleOrginalText", value); }
        }


        private string _publishDate;
        [DbType("char(10)")]
        [Layout(Caption = "تاریخ انتشار")]
        public string PublishDate
        {
            get { return _publishDate; }
            set { base.SetPropertyValue("PublishDate", value); }
        }

        private Int32 _visitCount;
        [DbType("int")]
        [Layout(Caption = "تعداد بازدیدکننده")]
        public Int32 VisitCount
        {
            get { return _visitCount; }
            set { base.SetPropertyValue("VisitCount", value); }
        }

        private Guid? _abstractFileId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        [Layout(Caption = "فایل چکیده")]
        public Guid? AbstractFileId
        {
            get
            {
                return this._abstractFileId;
            }
            set
            {
                base.SetPropertyValue("AbstractFileId", value);
                if (AbstractFile == null)
                    this.AbstractFile = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "چکیده ")]
        [Assosiation(PropName = "AbstractFileId", FillData = false)]
        public File AbstractFile { get; set; }

        [Layout(Caption = "آرشیو")]
        [IsNullable]
        [DbType("bit")]
        public bool? IsArchive { get; set; }


        [Layout(Caption = "اشتراک گذاری")]
        [DbType("bit")]
        public bool IsShare{ get; set; }

        private Guid? _orginalFileId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? OrginalFileId
        {
            get
            {
                return this._orginalFileId;
            }
            set
            {
                base.SetPropertyValue("OrginalFileId", value);
                if (OrginalFile == null)
                    this.OrginalFile = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "فایل اصل ")]
        [Assosiation(PropName = "OrginalFileId", FillData = false)]
        public File OrginalFile { get; set; }

        private Guid _userId;
        [DbType("uniqueidentifier")]
        [Layout(Caption = "کاربر")]
        public Guid UserId
        {
            get
            {
                return this._userId;
            }
            set
            {
                base.SetPropertyValue("UserId", value);
                if (User == null)
                    this.User = new User { Id = value };
            }
        }

        [Assosiation(PropName = "UserId")]
        public User User { get; set; }



        private Guid _pivotId;
        [DbType("uniqueidentifier")]
        [Layout(Caption = "محور")]
        public Guid PivotId
        {
            get
            {
                return this._pivotId;
            }
            set
            {
                base.SetPropertyValue("PivotId", value);
                if (Pivot == null)
                    this.Pivot = new Pivot { Id = value };


            }
        }

        [Assosiation(PropName = "PivotId")]
        public Pivot Pivot { get; set; }

        private byte _status;
        [DbType("tinyint")]
        [Layout(Caption = "وضعیت")]
        public byte Status
        {
            get { return _status; }
            set { base.SetPropertyValue("Status", value); }
        }

        private byte _finalState;
        [DbType("tinyint")]
        [Layout(Caption = "وضعیت نهایی")]
        public byte FinalState
        {
            get { return _finalState; }
            set { base.SetPropertyValue("FinalState", value); }
        }

        private byte? _payStatus;
        [DbType("tinyint")]
        [Layout(Caption = "وضعیت پرداخت")]
        public byte? PayStatus
        {
            get { return _payStatus; }
            set { base.SetPropertyValue("PayStatus", value); }
        }

        private Guid? _typeId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        [Layout(Caption = "نوع ")]
        public Guid? TypeId
        {
            get
            {
                return this._typeId;
            }
            set
            {
                base.SetPropertyValue("TypeId", value);
                if (ArticleType == null)
                    this.ArticleType = value.HasValue ? new ArticleType { Id = value.Value } : null;
            }
        }

        [Assosiation(PropName = "TypeId")]
        public ArticleType ArticleType { get; set; }

        private string _keyword;
        [DbType("nvarchar(max)")]
        [Layout(Caption = "کلمات کلیدی")]
        public string Keyword
        {
            get { return _keyword; }
            set { base.SetPropertyValue("Keyword", value); }
        }

        private string _authors;
        [DbType("nvarchar(max)")]
        [Layout(Caption = "نویسنده")]
        public string Authors
        {
            get { return _authors; }
            set { base.SetPropertyValue("Authors", value); }
        }

        private string _description;
        [DbType("nvarchar(max)")]
        [Layout(Caption = "توضیحات")]
        public string Description
        {
            get { return _description; }
            set { base.SetPropertyValue("Description", value); }
        }

        private Guid? _transactionId;
        [DbType("uniqueidentifier")]
        [Layout(Caption = "کد تراکنش")]
        public Guid? TransactionId
        {
            get { return _transactionId; }
            set
            {
                base.SetPropertyValue("TransactionId", value);
                if (Transaction == null)
                    this.Transaction = value.HasValue ? new Transaction() { Id = value.Value } : null;
            }
        }
        [Assosiation(PropName = "TransactionId")]
        public Transaction Transaction { get; set; }

        private Guid? _tempId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        [Layout(Caption = "پرداخت")]
        public Guid? TempId
        {
            get { return _tempId; }
            set { base.SetPropertyValue("TempId", value); }
        }

        private Guid? _paymentTypeId;
        [DbType("uniqueidentifier")]
        [Layout(Caption = "نوع پرداخت")]
        public Guid? PaymentTypeId
        {
            get
            {
                return this._paymentTypeId;
            }
            set
            {
                base.SetPropertyValue("PaymentTypeId", value);
                if (ArticlePaymentType == null)
                    this.ArticlePaymentType = value.HasValue ? new ArticlePaymentType { Id = value.Value } : null;
            }
        }


        [Assosiation(PropName = "PaymentTypeId")]
        public ArticlePaymentType ArticlePaymentType { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public byte? FinalStateNullable { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public byte? StateNullable { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string Statustostring { get { return ((Enums.ArticleState)Status).GetDescriptionInLocalization(); } }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string PaymentStatus { get { return PayStatus == null ? "" : ((Enums.ArticlepayState)PayStatus).GetDescriptionInLocalization(); } }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string AuthorsToString { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool HasRefereeAttachment { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool HasRefereeOpinion { get; set; }



        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool AllowSent { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool AllowDelete { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool PaymentVisibility { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool AllowPrintCertificate { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Title; }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string FirstName { get { return  ((this.User!=null&& this.User.EnterpriseNode!=null)?this.User.EnterpriseNode.RealEnterpriseNode.FirstName:""); } set { } }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string LastName { get { return ((this.User != null && this.User.EnterpriseNode != null) ? this.User.EnterpriseNode.RealEnterpriseNode.LastName:""); } set { } }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string FirstNameAndLastName
        {
            get { return this.User!=null?this.User.EnterpriseNode.DescriptionField:""; }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Int32 RefreeAttachmentCount { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string RefreeTitle { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string Email { get; set; }



        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool SendForReferee { get; set; }


    }
}
