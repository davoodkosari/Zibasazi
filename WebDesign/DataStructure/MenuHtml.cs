using System;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.WebDesign.DataStructure
{
    [Serializable]
    [Schema("WebDesign")]
    public sealed class MenuHtml : DataStructureBase<MenuHtml>
    {
        private Guid _webSiteId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid WebSiteId
        {
            get
            {
                return this._webSiteId;
            }
            set
            {
                base.SetPropertyValue("WebSiteId", value);
                if (WebSite == null)
                    this.WebSite = new WebSite() { Id = value };
            }
        }

        [Assosiation(PropName = "WebSiteId", FillData = false)]
        public WebSite WebSite { get; set; }

        private Guid _menuHtmlId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid MenuHtmlId
        {
            get
            {
                return this._menuHtmlId;
            }
            set
            {
                base.SetPropertyValue("MenuHtmlId", value);
                if (WebSiteMenuHtml == null)
                    this.WebSiteMenuHtml = new ContentManager.DataStructure.MenuHtml { Id = value };
            }
        }

        [Assosiation(PropName = "MenuHtmlId")]
        public ContentManager.DataStructure.MenuHtml WebSiteMenuHtml { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return ""; }
        }
    }
}
