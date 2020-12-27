using System;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class Teacher : DataStructureBase<Teacher>
    {
        private Guid _id;
        [Layout(Caption = "كد")]
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get
            {
                return this._id;
            }
            set
            {
                base.SetPropertyValue("Id", value);
                if (EnterpriseNode == null)
                    this.EnterpriseNode = new EnterpriseNode.DataStructure.EnterpriseNode { Id = value };
            }
        }

        [Layout(Caption = "كد")]
        [Assosiation(PropName = "Id")]
        public EnterpriseNode.DataStructure.EnterpriseNode EnterpriseNode { get; set; }

        private Guid _congressId;
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

        private Guid? _resumeAttachId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? ResumeAttachId
        {
            get
            {
                return this._resumeAttachId;
            }
            set
            {
                base.SetPropertyValue("ResumeAttachId", value);
                if (Resume == null)
                    this.Resume = value.HasValue ? new File { Id = value.Value } : null;
            }
        }

        [Assosiation(PropName = "ResumeAttachId")]
        public File Resume { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.EnterpriseNode.DescriptionField; }
        }
    }
}
