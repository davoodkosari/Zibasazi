using System;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressForms : DataStructureBase<CongressForms>
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

        private Guid _fomId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid FomId
        {
            get
            {
                return this._fomId;
            }
            set
            {
                base.SetPropertyValue("FomId", value);
                if (FormStructure == null)
                    this.FormStructure = new FormStructure { Id = value };
            }
        }

        [Assosiation(PropName = "FomId")]
        public FormStructure FormStructure { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.FormStructure.Name; }
        }
    }
}
