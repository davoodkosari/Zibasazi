using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class SecurityUser : DataStructureBase<SecurityUser>
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

        private Guid _userId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid UserId
        {
            get
            {
                return this._userId;
            }
            set
            {
                base.SetPropertyValue("UserId", value);
                if (User == null)
                    this.User = new Security.DataStructure.User { Id = value };
            }
        }

        [Assosiation(PropName = "UserId")]
        public Security.DataStructure.User User { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.User.DescriptionField; }
        }
    }
}
