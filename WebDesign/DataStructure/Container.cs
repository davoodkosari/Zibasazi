using System;
using Radyn.Framework;

namespace Radyn.WebDesign.DataStructure
{
    [Serializable]
    [Schema("WebDesign")]
    public sealed class Container : DataStructureBase<Container>
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
                this.WebSiteContainer = new ContentManager.DataStructure.Container { Id = value };
            }
        }

        [Assosiation(PropName= "ContainerId")]
        public ContentManager.DataStructure.Container WebSiteContainer { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.WebSiteContainer.DescriptionField; }
        }
    }
}
