
using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class ConfigurationSupportType : DataStructureBase<ConfigurationSupportType>
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
                if (Configuration == null)
                    this.Configuration = new Configuration { CongressId = value };
            }
        }

        [Assosiation(PropName = "CongressId", FillData = false)]
        public Configuration Configuration { get; set; }

        private Int16 _supportTypeId;
        [Key(false)]
        [DbType("smallint")]
        public Int16 SupportTypeId
        {
            get
            {
                return this._supportTypeId;
            }
            set
            {
                base.SetPropertyValue("SupportTypeId", value);
                if (SupportType == null)
                    this.SupportType = new SupportType { Id = value };
            }
        }

        [Assosiation(PropName = "SupportTypeId")]
        public SupportType SupportType { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return ""; }
        }
    }
}
