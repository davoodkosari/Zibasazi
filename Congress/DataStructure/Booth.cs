using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class Booth : DataStructureBase<Booth>
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

        private Guid? _categoryId;
        [Layout(Caption = "گروه")]
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? CategoryId
        {
            get
            {
                return this._categoryId;
            }
            set
            {
                base.SetPropertyValue("CategoryId", value);
                if (BoothCatgory == null)
                    this.BoothCatgory = value.HasValue ? new BoothCatgory { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "گروه")]
        [Assosiation(PropName = "CategoryId")]
        public BoothCatgory BoothCatgory { get; set; }

        private string _location;
        [Layout(Caption = "مكان غرفه")]
        [IsNullable]
        [DbType("nvarchar(200)")]
        public string Location
        {
            get { return _location; }
            set { base.SetPropertyValue("Location", value); }
        }


        private Int32 _reservCapacity;
        [Layout(Caption = "ظرفيت")]
        [IsNullable]
        [DbType("int")]
        public Int32 ReservCapacity
        {
            get { return _reservCapacity; }
            set { base.SetPropertyValue("ReservCapacity", value); }
        }



        private string _code;
        [Layout(Caption = "كد")]
        [IsNullable]
        [DbType("nvarchar(50)")]
        public string Code
        {
            get { return _code; }
            set { base.SetPropertyValue("Code", value); }
        }

        private string _currencyType;
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        [MultiLanguage]
        public string CurrencyType
        {
            get { return string.IsNullOrEmpty(_currencyType) ? "0" : _currencyType; }
            set { _currencyType = value; }
        }


        private Int32? _maxBoothOfficerCount;
        [IsNullable]
        [DbType("int")]
        public Int32? MaxBoothOfficerCount
        {
            get { return _maxBoothOfficerCount; }
            set { base.SetPropertyValue("MaxBoothOfficerCount", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        [MultiLanguage]
        public string ValidCost { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public int FreeCapicity { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public int RezervCount { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public decimal ReservAmount { get; set; }




        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Code; }
        }

    }
}
