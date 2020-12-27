using System;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class WorkShop : DataStructureBase<WorkShop>
    {
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

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

        private string _subject;
        [Layout(Caption = "موضوع كارگاه")]
        [IsNullable]
        [MultiLanguage]
        [DbType("nvarchar(300)")]
        public string Subject
        {
            get { return _subject; }
            set { base.SetPropertyValue("Subject", value); }
        }

        private Guid? _programAttachId;
        [Layout(Caption = "پيوست برنامه كارگاه")]
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? ProgramAttachId
        {
            get
            {
                return this._programAttachId;
            }
            set
            {
                base.SetPropertyValue("ProgramAttachId", value);
                if (ProgramAttach == null)
                    this.ProgramAttach = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "پيوست برنامه كارگاه")]
        [Assosiation(PropName = "ProgramAttachId")]
        public File ProgramAttach { get; set; }

        private string _comment;
        [Layout(Caption = "توضيحات")]
        [IsNullable]
        [MultiLanguage]
        [DbType("ntext")]
        public string Comment
        {
            get { return _comment; }
            set { base.SetPropertyValue("Comment", value); }
        }



        private int _capacity;
        [Layout(Caption = "ظرفيت")]
        [IsNullable]
        [DbType("int")]
        public int Capacity
        {
            get { return _capacity; }
            set { base.SetPropertyValue("Capacity", value); }
        }

        private Guid? _fileAttachId;
        [Layout(Caption = "پيوست فايل كارگاه")]
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? FileAttachId
        {
            get
            {
                return this._fileAttachId;
            }
            set
            {
                base.SetPropertyValue("FileAttachId", value);
                if (FileAttach == null)
                    this.FileAttach = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "پيوست فايل كارگاه")]
        [Assosiation(PropName = "FileAttachId")]
        public File FileAttach { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public int FreeCapicity { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public int RezervCount { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public decimal ReservAmount { get; set; }

        private string _currencyType;
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        [MultiLanguage]
        public string CurrencyType
        {
            get { return string.IsNullOrEmpty(_currencyType) ? "0" : _currencyType; }
            set { _currencyType = value; }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        [MultiLanguage]
        public string ValidCost { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Subject; }
        }
    }
}
