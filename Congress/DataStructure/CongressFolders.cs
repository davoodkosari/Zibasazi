using System;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressFolders : DataStructureBase<CongressFolders>
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

        private Guid _folderId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid FolderId
        {
            get
            {
                return this._folderId;
            }
            set
            {
                base.SetPropertyValue("FolderId", value);
                if (Folder == null)
                    this.Folder = new Folder { Id = value };
            }
        }

        [Assosiation(PropName = "FolderId")]
        public Folder Folder { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return Folder.Title; }
        }
    }
}
