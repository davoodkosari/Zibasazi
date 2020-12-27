using System;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class VIP : DataStructureBase<VIP>
    {
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get
            {
                return this._id;
            }
            set
            {
                base.SetPropertyValue("Id", value);
                if (EnterpriseNode == null)
                    this.EnterpriseNode = new EnterpriseNode.DataStructure.EnterpriseNode { Id = value };
            }
        }

        [Assosiation(PropName = "Id")]
        public EnterpriseNode.DataStructure.EnterpriseNode EnterpriseNode { get; set; }

        private Guid _congressId;
        [DbType("uniqueidentifier")]
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

        private string _abstract;
        [Layout(Caption = "توضيحات كوتاه")]
        [IsNullable]
        [MultiLanguage]
        [DbType("nvarchar(1500)")]
        public string Abstract
        {
            get { return _abstract; }
            set { base.SetPropertyValue("Abstract", value); }
        }

        private string _remark;
        [Layout(Caption = "توصيحات كامل")]
        [IsNullable]
        [MultiLanguage]
        [DbType("ntext")]
        public string Remark
        {
            get { return _remark; }
            set { base.SetPropertyValue("Remark", value); }
        }

        private string _presentDate;
        [Layout(Caption = "تاريخ ارائه سخنراني")]
        [IsNullable]
        [DbType("char(10)")]
        public string PresentDate
        {
            get { return _presentDate; }
            set { base.SetPropertyValue("PresentDate", value); }
        }

        private Guid? _resumeFileId;
        [Layout(Caption = "رزومه كاري ميهمان")]
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? ResumeFileId
        {
            get
            {
                return this._resumeFileId;
            }
            set
            {
                base.SetPropertyValue("ResumeFileId", value);
                if (File == null)
                    this.File = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "رزومه كاري ميهمان")]
        [Assosiation(PropName = "ResumeFileId")]
        public File File { get; set; }

        private string _role;
        [Layout(Caption = "پست سازماني")]
        [IsNullable]
        [DbType("nvarchar(150)")]
        public string Role
        {
            get { return _role; }
            set { base.SetPropertyValue("Role", value); }
        }

        private Int16 _sort;
        [Layout(Caption = "ترتیب")]
        [DbType("smallint")]
        public Int16 Sort
        {
            get { return _sort; }
            set { base.SetPropertyValue("Sort", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.EnterpriseNode.DescriptionField; }
        }
    }
}
