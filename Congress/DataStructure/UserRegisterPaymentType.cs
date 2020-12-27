using System;
using Radyn.Framework;
using Radyn.Utility;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class UserRegisterPaymentType : DataStructureBase<UserRegisterPaymentType>
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
        [DbType("nvarchar(50)")]
        [MultiLanguage]
        public string Title
        {
            get { return _title; }
            set { base.SetPropertyValue("Title", value); }
        }


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
        private bool _printable;
        [Layout(Caption = "قابل چاپ باشد")]
        [DbType("bit")]
        public bool Printable
        {
            get { return _printable; }
            set { base.SetPropertyValue("Printable", value); }
        }
        private bool _canUserSelect;
        [DbType("bit")]
        public bool CanUserSelect
        {
            get { return _canUserSelect; }
            set { base.SetPropertyValue("CanUserSelect", value); }
        }

        private int _order;
        [DbType("int")]
        public int Order
        {
            get { return _order; }
            set { base.SetPropertyValue("Order", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        [MultiLanguage]
        public string DaysInfo { get; set; }


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
        public string ValidAmount { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string GetAmount
        {
            get
            {
                if (string.IsNullOrEmpty(DaysInfo))
                    return ValidAmount;
                var split = DaysInfo.Split('-');
                decimal amount = 0;
                foreach (var value in split)
                {
                    if (string.IsNullOrEmpty(value)) continue;
                    var strings = value.Split(',');
                    amount += strings[1].ToDecimal();
                }
                return amount.ToString();
            }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Title; }
        }
    }
}
