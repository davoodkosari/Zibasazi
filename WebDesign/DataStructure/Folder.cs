using System;
using Radyn.Framework;

namespace Radyn.WebDesign.DataStructure
{
    [Serializable]
    [Schema("WebDesign")]
    public sealed class Folder : DataStructureBase<Folder>
    {
        private Guid _webId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid WebId
        {
            get
            {
                return this._webId;
            }
            set
            {
                base.SetPropertyValue("WebId", value);
                this.WebSite = new WebSite { Id = value };
            }
        }

        [Assosiation]
        public WebSite WebSite { get; set; }

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
                this.WebSiteFolder = new Radyn.FileManager.DataStructure.Folder { Id = value };
            }
        }

        [Assosiation(PropName = "FolderId")]
        public Radyn.FileManager.DataStructure.Folder WebSiteFolder { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.WebSiteFolder.Title; }
        }
    }
}
