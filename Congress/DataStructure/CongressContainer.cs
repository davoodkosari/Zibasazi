using System;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressContainer : DataStructureBase<CongressContainer>
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

        private Guid _containerId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid ContainerId
        {
            get
            {
                return this._containerId;
            }
            set
            {
                base.SetPropertyValue("ContainerId", value);
                if (Container == null)
                    this.Container = new Container { Id = value };
            }
        }

        [Assosiation(PropName = "ContainerId")]
        public Container Container { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Container.Title; }
        }
    }
}
