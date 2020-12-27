using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class RefereePivot : DataStructureBase<RefereePivot>
    {
        private Guid _refereeId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid RefereeId
        {
            get
            {
                return this._refereeId;
            }
            set
            {
                base.SetPropertyValue("RefereeId", value);
                if (Referee == null)
                    this.Referee = new Referee { Id = value };
            }
        }

        [Assosiation(PropName = "RefereeId")]
        public Referee Referee { get; set; }

        private Guid _pivotId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid PivotId
        {
            get
            {
                return this._pivotId;
            }
            set
            {
                base.SetPropertyValue("PivotId", value);
                if (Pivot == null)
                    this.Pivot = new Pivot { Id = value };
            }
        }

        [Assosiation(PropName = "PivotId")]
        public Pivot Pivot { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Pivot.Title; }
        }

      
    }
}
