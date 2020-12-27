using System;
using Radyn.Framework;
using Radyn.Payment.DataStructure;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressDiscountType : DataStructureBase<CongressDiscountType>
    {
        private Guid _congressId;
        [Key(false)]
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

        private Guid _discountTypeId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid DiscountTypeId
        {
            get
            {
                return this._discountTypeId;
            }
            set
            {
                base.SetPropertyValue("DiscountTypeId", value);
                if (DiscountType == null)
                    this.DiscountType = new DiscountType { Id = value };
            }
        }

        [Assosiation(PropName = "DiscountTypeId")]
        public DiscountType DiscountType { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.DiscountType.Title; }
        }
    }
}
