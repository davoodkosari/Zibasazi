using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class Pivot : DataStructureBase<Pivot>
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



        private Guid _pivotCategoryId;
        [DbType("uniqueidentifier")]
        public Guid PivotCategoryId
        {
            get
            {
                return this._pivotCategoryId;
            }
            set
            {
                base.SetPropertyValue("PivotCategoryId", value);
                if (PivotCategory == null)
                    this.PivotCategory = new PivotCategory { Id = value };
            }
        }

        [Assosiation(PropName = "PivotCategoryId")]
        public PivotCategory PivotCategory { get; set; }



        private string _title;
        [DbType("nvarchar(250)")]
        [MultiLanguage]
        public string Title
        {
            get { return _title; }
            set { base.SetPropertyValue("Title", value); }
        }

        private int _order;
        [DbType("int")]
        public int Order
        {
            get { return _order; }
            set { base.SetPropertyValue("Order", value); }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Title; }
        }
    }
}
