using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class ChipsFoodUser : DataStructureBase<ChipsFoodUser>
    {
        private Guid _chipsFoodId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid ChipsFoodId
        {
            get
            {
                return this._chipsFoodId;
            }
            set
            {
                base.SetPropertyValue("ChipsFoodId", value);
                if (ChipsFood == null)
                    this.ChipsFood = new ChipsFood { Id = value };
            }
        }

        [Assosiation(PropName = "ChipsFoodId")]
        public ChipsFood ChipsFood { get; set; }

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
                    this.User = new User { Id = value };
            }
        }

        [Assosiation(PropName = "UserId")]
        public User User { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.ChipsFood.Name; }
        }
    }
}
