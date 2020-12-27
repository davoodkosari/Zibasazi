using System;
using Radyn.Framework;
using Radyn.Utility;

namespace Radyn.Reservation.DataStructure
{
    [Serializable]
    [Schema("Reservation")]
    public sealed class ChairType : DataStructureBase<ChairType>
    {
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private Guid _hallId;
        [DbType("uniqueidentifier")]
        public Guid HallId
        {
            get
            {
                return this._hallId;
            }
            set
            {
                base.SetPropertyValue("HallId", value);
                if (Hall == null)
                    this.Hall = new Hall { Id = value };
            }
        }

        [Assosiation(PropName = "HallId")]
        public Hall Hall { get; set; }


        private string _title;
        [IsNullable]
        [MultiLanguage]
        [DbType("nvarchar(50)")]
        public string Title
        {
            get { return _title; }
            set { base.SetPropertyValue("Title", value); }
        }

        private string _currencyType;
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        [MultiLanguage]
        public string CurrencyType
        {
            get { return string.IsNullOrEmpty(_currencyType) ? "0" : _currencyType; }
            set { _currencyType = value; }
        }

        //private bool _enabled;
        //[Framework.DbType("bit")]
        //public bool Enabled
        //{
        //    get { return _enabled; }
        //    set { base.SetPropertyValue("Enabled", value); }
        //}

        private string _refId;
        [Framework.DbType("varchar(100)")]
        public string RefId
        {
            get { return _refId; }
            set { base.SetPropertyValue("RefId", value); }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        [MultiLanguage]
        public string ValidAmount { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Title + " " + "(" + ValidAmount.ToDecimal().ToString("n0") + " " + CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>().GetDescriptionInLocalization() + ")"; }
        }
    }
}
