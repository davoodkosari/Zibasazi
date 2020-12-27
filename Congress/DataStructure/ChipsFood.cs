using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class ChipsFood : DataStructureBase<ChipsFood>
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

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        [MultiLanguage]
        public string Name { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        [MultiLanguage]
        public string Description { get; set; }

        private Int32 _capacity;
        [Layout(Caption = "ظرفيت")]
        [DbType("int")]
        public Int32 Capacity
        {
            get { return _capacity; }
            set { base.SetPropertyValue("Capacity", value); }
        }

        private Int32 _baseCapacity;
        [Layout(Caption = "ظرفيت اولیه")]
        [DbType("int")]
        public Int32 BaseCapacity
        {
            get { return _baseCapacity; }
            set { base.SetPropertyValue("BaseCapacity", value); }
        }

        private string _daysInfo;
        [IsNullable]
        [DbType("nvarchar(1000)")]
        public string DaysInfo
        {
            get { return _daysInfo; }
            set { base.SetPropertyValue("DaysInfo", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Name; }
        }
    }
}
