using System;
using Radyn.Framework;
using Radyn.Reservation.DataStructure;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressHall : DataStructureBase<CongressHall>
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

        private Guid _hallId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid HallId
        {
            get { return _hallId; }
            set { base.SetPropertyValue("HallId", value); }
        }
        [Assosiation(PropName = "HallId")]
        public Hall Hall { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Homa.CongressTitle; }
        }
    }
}
