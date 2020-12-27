using Radyn.Framework;
using System;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class PivotCategory : DataStructureBase<PivotCategory>
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


        private string _title;
        [DbType("nvarchar(250)")]
        [MultiLanguage]
        public string Title
        {
            get { return _title; }
            set { base.SetPropertyValue("Title", value); }
        }

        private string _description;
        [DbType("nvarchar(Max)")]
        [IsNullable]
        public string Description
        {
            get { return _description; }
            set { base.SetPropertyValue("Description", value); }
        }

        private int _order;
        [DbType("int")]
        public int Order
        {
            get { return _order; }
            set { base.SetPropertyValue("Order", value); }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField { get; }
    }
}
