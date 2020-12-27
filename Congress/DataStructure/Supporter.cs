using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class Supporter : DataStructureBase<Supporter>
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
            }
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

        private Int16 _supportTypeId;
        [Layout(Caption = "نوع حمايت")]
        [DbType("smallint")]
        public Int16 SupportTypeId
        {
            get
            {
                return this._supportTypeId;
            }
            set
            {
                base.SetPropertyValue("SupportTypeId", value);
                if (SupportType == null)
                    this.SupportType = new SupportType { Id = value };
            }
        }

        [Layout(Caption = "نوع حمايت")]
        [Assosiation(PropName = "SupportTypeId")]
        public SupportType SupportType { get; set; }

        private Int16 _sort;
        [DbType("smallint")]
        public Int16 Sort
        {
            get { return _sort; }
            set { base.SetPropertyValue("Sort", value); }
        }
        private string _webSite;
        [DbType("varchar(200)")]
        public string WebSite
        {
            get { return _webSite; }
            set { base.SetPropertyValue("WebSite", value); }
        }






        private string _title;
        [DbType("nvarchar(200)")]
        [MultiLanguage]
        public string Title
        {
            get { return _title; }
            set { base.SetPropertyValue("Title", value); }
        }
        private string _image;
        [DbType("varchar(50)")]
        [MultiLanguage]
        public string Image
        {
            get { return _image; }
            set { base.SetPropertyValue("Image", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Title; }
        }
    }
}
