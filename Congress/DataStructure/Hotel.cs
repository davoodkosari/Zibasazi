using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class Hotel : DataStructureBase<Hotel>
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

        private string _name;
        [Layout(Caption = "نام مكان")]
        [DbType("nvarchar(100)")]
        [MultiLanguage]
        public string Name
        {
            get { return _name; }
            set { base.SetPropertyValue("Name", value); }
        }



        private string _address;
        [Layout(Caption = "آدرس")]
        [DbType("nvarchar(200)")]
        public string Address
        {
            get { return _address; }
            set { base.SetPropertyValue("Address", value); }
        }

        private string _remark;
        [Layout(Caption = "توضيحات")]
        [IsNullable]
        [DbType("nvarchar(1000)")]
        public string Remark
        {
            get { return _remark; }
            set { base.SetPropertyValue("Remark", value); }
        }

        private Int32 _capacity;
        [Layout(Caption = "ظرفيت")]
        [DbType("int")]
        public Int32 Capacity
        {
            get { return _capacity; }
            set { base.SetPropertyValue("Capacity", value); }
        }
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
            get { return this.Name; }
        }
    }
}
