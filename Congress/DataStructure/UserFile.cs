using System;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class UserFile : DataStructureBase<UserFile>
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

        private Guid _fileId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid FileId
        {
            get
            {
                return this._fileId;
            }
            set
            {
                base.SetPropertyValue("FileId", value);
                if (File == null)
                    this.File = new File { Id = value };
            }
        }

        [Assosiation(PropName = "FileId")]
        public File File { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.File.FileName; }
        }
    }
}
