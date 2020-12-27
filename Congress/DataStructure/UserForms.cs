using System;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class UserForms : DataStructureBase<UserForms>
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

        private Guid _formId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid FormId
        {
            get
            {
                return this._formId;
            }
            set
            {
                base.SetPropertyValue("FormId", value);
                if (FormStructure == null)
                    this.FormStructure = new FormStructure { Id = value };
            }
        }

        [Assosiation(PropName = "FormId")]
        public FormStructure FormStructure { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.FormStructure.Name; }
        }
    }
}
