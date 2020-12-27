using System;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.WebDesign.DataStructure
{
    [Serializable]
    [Schema("WebDesign")]
    public sealed class Html : DataStructureBase<Html>
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

        private Guid _htmlDesginId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid HtmlDesginId
        {
            get
            {
                return this._htmlDesginId;
            }
            set
            {
                base.SetPropertyValue("HtmlDesginId", value);
                this.HtmlDesgin = new HtmlDesgin { Id = value };
            }
        }

        [Assosiation(PropName = "HtmlDesginId")]
        public HtmlDesgin HtmlDesgin { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.WebSite.Title; }
        }
    }
}
