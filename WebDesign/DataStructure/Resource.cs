using System;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;

namespace Radyn.WebDesign.DataStructure
{
    [Serializable]
    [Schema("WebDesign")]
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

        private Guid _webId;
        [DbType("uniqueidentifier")]
        public Guid WebId
        {
            get
            {
                return this._webId;
            }
            set
            {
                base.SetPropertyValue("WebId", value);
                this.WebSite = new WebSite { Id = value };
            }
        }

        [Assosiation]
        public WebSite WebSite { get; set; }

        private byte _type;
        [DbType("tinyint")]
        public byte Type
        {
            get { return _type; }
            set { base.SetPropertyValue("Type", value); }
        }

        private Guid? _fileId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? FileId
        {
            get { return _fileId; }
            set
            {
                base.SetPropertyValue("FileId", value);
                this.File = value.HasValue ? new File { Id = value.Value } : null;
            }

        }

        [Assosiation]
        public File File { get; set; }

        private string _lanuageId;
        [IsNullable]
        [DbType("char(5)")]
        public string LanuageId
        {
            get { return _lanuageId; }
            set { base.SetPropertyValue("LanuageId", value); }
        }

        private string _text;
        [IsNullable]
        [DbType("nvarchar(1000)")]
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
