using System;
using Radyn.Common.DataStructure;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class ConfigurationContent : DataStructureBase<ConfigurationContent>
    {
        private Guid _configurationId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid ConfigurationId
        {
            get
            {
                return this._configurationId;
            }
            set
            {
                base.SetPropertyValue("ConfigurationId", value);
                if (Configuration == null)
                    this.Configuration = new Configuration { CongressId = value };
            }
        }

        [Assosiation(PropName = "ConfigurationId", FillData = false)]
        public Configuration Configuration { get; set; }

        private string _languageId;
        [Key(false)]
        [DbType("char(5)")]
        public string LanguageId
        {
            get
            {
                return this._languageId;
            }
            set
            {
                base.SetPropertyValue("LanguageId", value);
                if (Language == null)
                    this.Language = new Language { Id = value };
            }
        }

        [Assosiation(PropName = "LanguageId")]
        public Language Language { get; set; }




        private Guid? _attachRefereeFileId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? AttachRefereeFileId
        {
            get
            {
                return this._attachRefereeFileId;
            }
            set
            {
                base.SetPropertyValue("AttachRefereeFileId", value);
                if (AttachRefereeFile == null)
                    this.AttachRefereeFile = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "فرم داوری")]
        [Assosiation(PropName = "AttachRefereeFileId")]
        public File AttachRefereeFile { get; set; }


        private Guid? _boothMapAttachmentId;
        [Layout(Caption = "پیوست نقشه نمایشگاه")]
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? BoothMapAttachmentId
        {
            get
            {
                return this._boothMapAttachmentId;
            }
            set
            {
                base.SetPropertyValue("BoothMapAttachmentId", value);
                if (BoothMapAttachment == null)
                    this.BoothMapAttachment = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "نقشه نمایشگاه")]
        [Assosiation(PropName = "BoothMapAttachmentId")]
        public File BoothMapAttachment { get; set; }

        private Guid? _orginalPosterId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? OrginalPosterId
        {
            get
            {
                return this._orginalPosterId;
            }
            set
            {
                base.SetPropertyValue("OrginalPosterId", value);
                if (OrginalPoster == null)
                    this.OrginalPoster = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "پوستر اصلی")]
        [Assosiation(PropName = "OrginalPosterId")]
        public File OrginalPoster { get; set; }

        private Guid? _miniPosterId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? MiniPosterId
        {
            get
            {
                return this._miniPosterId;
            }
            set
            {
                base.SetPropertyValue("MiniPosterId", value);
                if(MiniPoster==null)
                this.MiniPoster = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "پوستر کوچک")]
        [Assosiation(PropName = "MiniPosterId")]
        public File MiniPoster { get; set; }

        private Guid? _headerId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? HeaderId
        {
            get
            {
                return this._headerId;
            }
            set
            {
                base.SetPropertyValue("HeaderId", value);
                if (Header == null)
                    this.Header = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "هدر سایت")]
        [Assosiation(PropName = "HeaderId")]
        public File Header { get; set; }


        private Guid? _logoId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? LogoId
        {
            get
            {
                return this._logoId;
            }
            set
            {
                base.SetPropertyValue("LogoId", value);
                if (Logo == null)
                    this.Logo = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "فوتر سایت")]
        [Assosiation(PropName = "LogoId")]
        public File Logo { get; set; }

        private Guid? _footerId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? FooterId
        {
            get
            {
                return this._footerId;
            }
            set
            {
                base.SetPropertyValue("FooterId", value);
                if (Footer == null)
                    this.Footer = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "لوگو سایت")]
        [Assosiation(PropName = "FooterId")]
        public File Footer { get; set; }


        private Int32? _contentManagerHeaderId;
        [IsNullable]
        [DbType("int")]
        public Int32? ContentManagerHeaderId
        {
            get { return _contentManagerHeaderId; }
            set { base.SetPropertyValue("ContentManagerHeaderId", value); }
        }

        private Int32? _contentManagerFooterId;
        [IsNullable]
        [DbType("int")]
        public Int32? ContentManagerFooterId
        {
            get { return _contentManagerFooterId; }
            set { base.SetPropertyValue("ContentManagerFooterId", value); }
        }

        private Guid? _hallMapId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? HallMapId
        {
            get
            {
                return this._hallMapId;
            }
            set
            {
                base.SetPropertyValue("HallMapId", value);
                if (HallMap == null)
                    this.HallMap = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "فوتر سایت")]
        [Assosiation(PropName = "HallMapId")]
        public File HallMap { get; set; }

        private string _cartTypeEmptyValue;
        [IsNullable]
        [DbType("nvarchar(100)")]
        public string CartTypeEmptyValue
        {
            get { return _cartTypeEmptyValue; }
            set { base.SetPropertyValue("CartTypeEmptyValue", value); }
        }


      



        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return string.Empty; }
        }

        private bool _headerFromContentManager;

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool HeaderFromContentManager
        {
            get
            {
                if (ContentManagerHeaderId.HasValue && HeaderId == null) return true;
                return _headerFromContentManager;
            }
            set { _headerFromContentManager = value; }
        }

        private bool _footerFromContentManager;

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool FooterFromContentManager
        {
            get
            {
                if (ContentManagerFooterId.HasValue && FooterId == null) return true;
                return _footerFromContentManager;
            }
            set { _footerFromContentManager = value; }
        }
    }
}
