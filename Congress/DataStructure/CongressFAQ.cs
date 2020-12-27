using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressFAQ : DataStructureBase<CongressFAQ>
    {
        private Guid _congressId;
        [DbType("uniqueidentifier")]
        [Key(false)]
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

        private Guid _fAQId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid FAQId
        {
            get
            {
                return this._fAQId;
            }
            set
            {
                base.SetPropertyValue("FAQId", value);
                if (FAQ == null)
                    this.FAQ = new FAQ.DataStructure.FAQ { Id = value };
            }
        }

        [Assosiation(PropName = "FAQId")]
        public FAQ.DataStructure.FAQ FAQ { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.FAQ.Question; }
        }
    }
}
