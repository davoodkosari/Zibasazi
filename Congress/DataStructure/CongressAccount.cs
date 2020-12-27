using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressAccount : DataStructureBase<CongressAccount>
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

        [Assosiation]
        public Homa Homa { get; set; }

        private Int16 _accountId;
        [Key(false)]
        [DbType("smallint")]
        public Int16 AccountId
        {
            get { return _accountId; }
            set
            {
                base.SetPropertyValue("AccountId", value);
                if (Account == null)
                    Account = new Radyn.Payment.DataStructure.Account() { Id = value };
            }
        }
        [Assosiation(PropName = "AccountId")]
        public Radyn.Payment.DataStructure.Account Account { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Account.AccountNo; }
        }
    }
}
