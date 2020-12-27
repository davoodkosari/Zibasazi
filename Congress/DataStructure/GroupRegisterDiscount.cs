using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class GroupRegisterDiscount : DataStructureBase<GroupRegisterDiscount>
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

        private Int32? _from;
        [IsNullable]
        [DbType("int")]
        public Int32? From
        {
            get { return _from; }
            set { base.SetPropertyValue("From", value); }
        }

        private Int32? _to;
        [IsNullable]
        [DbType("int")]
        public Int32? To
        {
            get { return _to; }
            set { base.SetPropertyValue("To", value); }
        }

        private bool _isPrecent;
        [DbType("bit")]
        public bool IsPrecent
        {
            get { return _isPrecent; }
            set { base.SetPropertyValue("IsPrecent", value); }
        }


        private bool _enable;
        [DbType("bit")]
        public bool Enable
        {
            get { return _enable; }
            set { base.SetPropertyValue("Enable", value); }
        }

        private string _startDate;
        [IsNullable]
        [DbType("char(10)")]
        public string StartDate
        {
            get { return _startDate; }
            set { base.SetPropertyValue("StartDate", value); }
        }

        private string _endDate;
        [IsNullable]
        [DbType("char(10)")]
        public string EndDate
        {
            get { return _endDate; }
            set { base.SetPropertyValue("EndDate", value); }
        }
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
        public override string DescriptionField
        {
            get { return this.ValidAmount; }
        }
    }
}
