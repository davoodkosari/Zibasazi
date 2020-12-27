using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class Resource : DataStructureBase<Resource>
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


        private byte _type;
        [DbType("tinyint")]
        public byte Type
        {
            get { return _type; }
            set { base.SetPropertyValue("Type", value); }
        }

        private bool _enabled;
        [DbType("bit")]
        public bool Enabled
        {
            get { return _enabled; }
            set { base.SetPropertyValue("Enabled", value); }
        }

        private string _fileId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        [MultiLanguage]
        public string FileId
        {
            get { return _fileId; }
            set
            {
                base.SetPropertyValue("FileId", value);
              
            }

        }

        private string _useLayoutId;
        [DbType("varchar(30)")]
        public string UseLayoutId
        {
            get { return _useLayoutId; }
            set { base.SetPropertyValue("UseLayoutId", value); }
        }


        private string _title;
        [IsNullable]
        [DbType("nvarchar(1000)")]
        [MultiLanguage]
        public string Title
        {
            get { return _title; }
            set { base.SetPropertyValue("Title", value); }
        }

        private string _text;
        [IsNullable]
        [DbType("nvarchar(1000)")]
        [MultiLanguage]
        public string Text
        {
            get { return _text; }
            set { base.SetPropertyValue("Text", value); }
        }


        private byte _order;
        [DbType("tinyint")]
        public byte Order
        {
            get { return _order; }
            set { base.SetPropertyValue("Order", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Order.ToString(); }
        }

        
    }
}
